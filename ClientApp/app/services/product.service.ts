import { Product } from './../models/product';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http'; 
import 'rxjs/add/operator/map';


@Injectable()
export class ProductService {
  private readonly productsEndpoint = '/api/products';

  constructor(private http: Http) { }
  getProduct(id) {
    return this.http.get(this.productsEndpoint + '/' + id)
      .map(res => res.json());
  }
  getProducts() {
    return this.http.get(this.productsEndpoint)
      .map(res => res.json());
  }
  update(product: Product) {
    return this.http.put(this.productsEndpoint + '/' + product.id, product)
      .map(res => res.json());
  }

  getPrices() {
    return this.http.get('api/products/prices')
      .map(res => res.json());
  }
  getEshops() {
    return this.http.get('api/eshop')
      .map(res => res.json());
  }
}