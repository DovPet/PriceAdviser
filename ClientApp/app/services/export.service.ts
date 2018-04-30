import { Injectable } from '@angular/core';
import { Http } from '@angular/http'; 
import 'rxjs/add/operator/map';

@Injectable()
export class ExportService {
  private readonly exportEndpoint = '/api/export';
  private readonly pricesEndpoint = 'api/products/prices';

  constructor(private http: Http) { }
  export() {
    return this.http.get(this.exportEndpoint);
  }
  getPrices() {
    return this.http.get(this.exportEndpoint + '/prices')
      .map(res => res.json());
  }
  update(price: any) {
    return this.http.put(this.pricesEndpoint + '/' + price.id, price)
      .map(res => res.json());
  }
}