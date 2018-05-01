import { SaveProduct, Product } from './../../models/product';
import * as _ from 'underscore'; 
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from './../../services/product.service';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from "ng2-toasty";
import 'rxjs/add/Observable/forkJoin';
import { SavePrice } from '../../models/price';

@Component({
    selector: 'app-create-product',
    templateUrl: './create-product.html',
    styleUrls: ['./create-product.css']
  })

  export class CreateProductComponent implements OnInit {
    
    product: SaveProduct = {
      id: 0,
      code: '',
      name: ''
    };
    products : Product[];
    
    price: SavePrice = {
    id: 0, 
    value: 0,   
    eshopId: 1, 
    updatedAt: '2018-04-16T22:41:59',
    productId: this.product.id,   
    edited: true
      };

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private productService: ProductService,
        private toastyService: ToastyService) {
    
          route.params.subscribe(p => {
            this.product.id = +p['id'] || 0;
            this.price.id =+p['id'] || 0;
          });

          
        }

    ngOnInit()  {

           
            if (this.product.id) {
                this.productService.getProduct(this.product.id).subscribe(p=>{this.product = p;});   
        }
        err => {
          if (err.status == 404)
            this.router.navigate(['/products']);
        };
    }

    private setProduct(p: SaveProduct) {
        this.product.id = p.id;
        this.product.name = p.name;
        this.product.code = p.code;
        
      } 

      submit() {
        var result$ = this.productService.createProduct(this.product);
        result$.subscribe(product => {
          this.toastyService.success({
            title: 'Success', 
            msg: 'Produktas sukurtas sėkmingai, galite nustatyti jam kainą',
            theme: 'bootstrap',
            showClose: true,
            timeout: 5000
          });

          this.price.productId = product.id;
          var result2$ = this.productService.createPrice(this.price);
        result2$.subscribe(price => {
            this.toastyService.success({
              title: 'Success', 
              msg: 'Produktas sukurtas sėkmingai, galite nustatyti jam kainą',
              theme: 'bootstrap',
              showClose: true,
              timeout: 5000
            });
          });
        });
        this.router.navigate(['/products'])        
      }
      
}