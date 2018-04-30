import { Product } from './../../models/product';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from 'ng2-toasty';
import { Router, ActivatedRoute } from '@angular/router';
import { Price,SavePrice } from '../../models/price';
import { ExportService } from '../../services/export.service';

@Component({
    
    templateUrl: 'export.html',
    styleUrls: ['export.css']
  })

  export class ExportComponent implements OnInit {
    
        
        prices: Price[];
        products: Product[];

        price: any = {
            id: 0, 
            value: 0,
            updatedAt: '',
            eshopId: 1,
            edited: 0,
            code: ''
         };
       
        constructor(private exportService: ExportService,
            private route: ActivatedRoute,
            private router: Router,     
            private toastyService: ToastyService) { }
    
        ngOnInit() {
            this.exportService.getPrices().subscribe(prices => this.prices = prices);
                      
        }
        selectExport(id) {
            var pric = this.prices.find(m => m.id == id);
            this.price = pric;
            
            if (confirm("Ar tikrai norite pašalinti "+ this.price.code +" iš eksportavimo failo?")) 
            {
                this.price.edited = false;
                var result$ =  this.exportService.update(this.price);
                result$.subscribe(msg => {
                  this.toastyService.success({
                    title: 'Success', 
                    msg: 'Produktas '+this.price.code+' pašalintas sėkmingai.',
                    theme: 'bootstrap',
                    showClose: true,
                    timeout: 5000
                  });
                  window.location.reload()
                });

            }
        }

        export() {
            if (confirm("Ar tikrai norite ekportuoti žemiau pateiktus produktus \n"
                            +"Failą galėsite rasti /Links direktorijoje")) 
            {
                var result$ =  this.exportService.export();
                result$.subscribe(msg => {
                  this.toastyService.success({
                    title: 'Success', 
                    msg: 'Sąrašas eksportuotas sėkmingai.',
                    theme: 'bootstrap',
                    showClose: true,
                    timeout: 5000
                  });
                  window.location.reload()
                });

            }
        }
      }