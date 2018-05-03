import { SaveEshop } from './../models/eshop';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http'; 
import { AuthHttp } from "angular2-jwt/angular2-jwt";
import 'rxjs/add/operator/map';


@Injectable()
export class EshopService {
  private readonly eshopEndpoint = '/api/eshop';

  constructor(private http: Http,private authHttp: AuthHttp) { }
  getEshop(id) {
    return this.authHttp.get(this.eshopEndpoint + '/' + id)
      .map(res => res.json());
  }
  getEshops() {
    return this.http.get(this.eshopEndpoint)
      .map(res => res.json());
  }
  update(eshop: SaveEshop) {
    return this.authHttp.put(this.eshopEndpoint + '/' + eshop.id, eshop)
      .map(res => res.json());
  }
}