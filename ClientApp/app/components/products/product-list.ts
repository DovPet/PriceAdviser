import { ProductService } from './../../services/product.service';
import { Product } from './../../models/product';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from 'ng2-toasty';
import { Router, ActivatedRoute } from '@angular/router';
import { Price } from '../../models/price';

@Component({
    templateUrl: 'product-list.html'
  })

  export class ProductListComponent implements OnInit {
    
        product: Product[];
        prices: Price[];
    
        constructor(private productService: ProductService) { }
    
        ngOnInit() {
            this.productService.getProducts().subscribe(product => this.product = product);
            this.productService.getPrices()
            .subscribe(prices => this.prices = prices);
        }
      }