import { ProductService } from './../../services/product.service';
import { Product } from './../../models/product';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from 'ng2-toasty';
import { Router, ActivatedRoute } from '@angular/router';
import { Price } from '../../models/price';
import { Eshop } from '../../models/eshop';
import { ChangeDetectorRef } from '@angular/core';
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
        curPage : number;
        pageSize : number;
        pages: number;
        constructor(private productService: ProductService, private cd: ChangeDetectorRef) { 
            this.curPage = 1;
            this.pageSize = 10; 

        }
    
        ngOnInit() {
            this.productService.getProducts().subscribe(product => this.product = this.allProducts = product);
            this.cd.markForCheck();
        }

        onFilterChange(){
            var products = this.allProducts;
             if (this.filter.code)
                products = products.filter(p=> p.code.startsWith(this.filter.code))
                this.cd.markForCheck();
            this.product = products;
            this.curPage = 1;
            this.pages = Math.ceil(products.length / this.pageSize);
        }

        onFilterClear(){
            this.product = this.allProducts;
        }


        numberOfPages(){
            this.pages = Math.ceil(this.product.length / this.pageSize);
            return this.pages;
          };
      }