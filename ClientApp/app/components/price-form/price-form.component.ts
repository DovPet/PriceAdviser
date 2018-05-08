import { Eshop } from './../../models/eshop';
import { SaveProduct, Product } from './../../models/product';
import { SavePrice, Price } from './../../models/price';
import * as _ from 'underscore'; 
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from './../../services/product.service';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from "ng2-toasty";
import 'rxjs/add/Observable/forkJoin';
import { Location } from '@angular/common';

@Component({
    selector: 'app-price-form',
    templateUrl: './price-form.component.html',
    styleUrls: ['./price-form.component.css']
  })

  export class ProductFormComponent implements OnInit {
    
    prices: Price[];
    eshops: Eshop[];

    
    price: SavePrice = {
        id: 0, 
        value: 0,
        updatedAt: '',
        eshopId: 1,
        productId: 0,
        edited: true
     };

     product: SaveProduct ={
      id: this.price.productId , 
      code: '', 
      name: ''
    } 
        constructor(  
            private route: ActivatedRoute,
            private router: Router,
            private productService: ProductService,
            private toastyService: ToastyService,
            private _location: Location
            ) {
        
              route.params.subscribe(p => {
                this.price.id = +p['id'] || 0;
              });
            }
    
            ngOnInit() {
              this.productService.getPrice(this.price.id).subscribe(p=>{this.price = p;});
              
                    err => {
                      if (err.status == 404)
                        this.router.navigate(['/products']);
                    };
            }  

              submit() {
                var date = new Date();  
                var day = date.getDate();       // yields date
                var month = date.getMonth() + 1;    // yields month (add one as '.getMonth()' is zero indexed)
                var year = date.getFullYear();  // yields year
                var hour = date.getHours();     // yields hours 
                var minute = date.getMinutes(); // yields minutes
                var second = date.getSeconds();
                var time = year + "-" + month + "-" + day+ "T" + hour + ':' + minute + ':' + second;   
                this.price.updatedAt =  time;
                this.price.edited = true;
                var result$ =  this.productService.updatePrice(this.price);
                result$.subscribe(price => {
                  this.toastyService.success({
                    title: 'Success', 
                    msg: 'Kaina išsaugota sėkmingai.',
                    theme: 'bootstrap',
                    showClose: true,
                    timeout: 5000
                  });
                  this._location.back();
                });
              }
    }