import { SavePrice } from './../models/price';
import { Product, SaveProduct } from './../models/product';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http'; 
import 'rxjs/add/operator/map';


@Injectable()
export class ProductService {
  private readonly productsEndpoint = '/api/products';
  private readonly pricesEndpoint = 'api/products/prices';

  constructor(private http: Http) { }
  getProduct(id) {
    return this.http.get(this.productsEndpoint + '/' + id)
      .map(res => res.json());
  }
  getProducts() {
    return this.http.get(this.productsEndpoint)
      .map(res => res.json());
  }
   update(product: SaveProduct) {
    return this.http.put(this.productsEndpoint + '/' + product.id, product)
      .map(res => res.json());
  }

  getPrice(id) {
    return this.http.get(this.pricesEndpoint + '/' + id)
      .map(res => res.json());
  }
  updatePrice(price: SavePrice) {
    return this.http.put(this.pricesEndpoint + '/' + price.id, price)
      .map(res => res.json());
  }
  getPrices() {
    return this.http.get(this.pricesEndpoint)
      .map(res => res.json());
  }
  getEshops() {
    return this.http.get('api/eshop')
      .map(res => res.json());
  }
}