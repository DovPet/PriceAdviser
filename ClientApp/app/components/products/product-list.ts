import { ProductService } from './../../services/product.service';
import { Product } from './../../models/product';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from 'ng2-toasty';
import { Router, ActivatedRoute } from '@angular/router';
import { Price } from '../../models/price';
import { Eshop } from '../../models/eshop';

@Component({
    
    templateUrl: 'product-list.html',
    styleUrls: ['product-list.css']
  })

  export class ProductListComponent implements OnInit {
    
        product: Product[];
        allProducts: Product[];
        prices: Price[];
        eshops: Eshop[];
        filter: any = {};
        constructor(private productService: ProductService) { }
    
        ngOnInit() {
            this.productService.getProducts().subscribe(product => this.product = this.allProducts = product);
            this.productService.getPrices().subscribe(prices => this.prices = prices);
            this.productService.getEshops().subscribe(eshops => this.eshops = eshops);
           
        }

        onFilterChange(){
            var products = this.allProducts;
             if (this.filter.code)
                products = products.filter(p=> p.code.startsWith(this.filter.code))
            
            this.product = products;
        }
      }