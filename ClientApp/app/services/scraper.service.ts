import { Injectable } from '@angular/core';
import { Http } from '@angular/http'; 
import 'rxjs/add/operator/map';

@Injectable()
export class ScraperService {
  private readonly productsEndpoint = '/api/SampleData';

  constructor(private http: Http) { }
  skytech() {
    return this.http.get(this.productsEndpoint + '/skytech');
  }
  kilobaitas() {
    return this.http.get(this.productsEndpoint + '/kilobaitas');
  }
  fortakas() {
    return this.http.get(this.productsEndpoint + '/fortakas');
  }
  topocentras() {
    return this.http.get(this.productsEndpoint + '/topocentras');
  }
  all() {
    return this.http.get(this.productsEndpoint + '/all');
  }

}