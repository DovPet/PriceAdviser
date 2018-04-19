import { SaveEshop } from './../models/eshop';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http'; 
import 'rxjs/add/operator/map';


@Injectable()
export class EshopService {
  private readonly eshopEndpoint = '/api/eshop';

  constructor(private http: Http) { }
  getScrapable(id) {
    return this.http.get(this.eshopEndpoint + '/' + id)
      .map(res => res.json());
  }
  getScrapables() {
    return this.http.get(this.eshopEndpoint)
      .map(res => res.json());
  }
  update(eshop: SaveEshop) {
    return this.http.put(this.eshopEndpoint + '/' + eshop.id, eshop)
      .map(res => res.json());
  }
}