import { SaveProduct, Product } from './../../models/product';
import { SavePrice } from './../../models/price';
import * as _ from 'underscore'; 
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from './../../services/product.service';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from "ng2-toasty";
import 'rxjs/add/Observable/forkJoin';
import { DatePipe } from '@angular/common/src/pipes';


@Component({
    selector: 'app-price-form',
    templateUrl: './price-form.component.html',
    styleUrls: ['./price-form.component.css']
  })

  export class ProductFormComponent implements OnInit {
    
    prices: SavePrice [];

     product: SaveProduct = {
       id: 0, 
       edited: true,
       prices: []
    };

    price: SavePrice = {
        id: 0, 
        value: 0,
        updatedAt: 0,
        productId: 0,
        eshopId: 0
     };
 
        constructor(  
            private route: ActivatedRoute,
            private router: Router,
            private productService: ProductService,
            private toastyService: ToastyService) {
        
              route.params.subscribe(p => {
                this.product.id = +p['id'] || 0;
              });
            }
    
            ngOnInit() {


                    this.productService.getProduct(this.product.id).subscribe(p=>{this.product = p;});
                    this.productService.getPrices().subscribe(prices => this.prices = prices);
                    var pric = this.product.prices.find(p=>p.eshopId==1);
                    this.productService.getProduct(pric).subscribe(p=>{this.product = p;});
                    err => {
                      if (err.status == 404)
                        this.router.navigate(['/products']);
                    };
                   
               
            }  
            
            private setProdut(p: SaveProduct) {
                this.product.id = p.id;
                this.product.edited = p.edited; 
              } 
              private setPrice(p: SavePrice) {
                this.price.id = p.id;
                this.price.value = p.value; 
                this.price.updatedAt = p.updatedAt;
              }
              submit() {
                var result$ =  this.productService.update(this.product);
                result$ =  this.productService.updatePrice(this.price);
                result$.subscribe(eshop => {
                  this.toastyService.success({
                    title: 'Success', 
                    msg: 'Duomenys išsaugoti sėkmingai.',
                    theme: 'bootstrap',
                    showClose: true,
                    timeout: 5000
                  });
                  this.router.navigate(['/products'])
                });
              }    
    }