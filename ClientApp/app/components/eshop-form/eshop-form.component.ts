import { SaveEshop, Eshop } from './../../models/eshop';

import * as _ from 'underscore'; 
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, Router } from '@angular/router';
import { EshopService } from './../../services/eshop.service';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from "ng2-toasty";
import 'rxjs/add/Observable/forkJoin';

@Component({
    selector: 'app-eshop-form',
    templateUrl: './eshop-form.component.html',
    styleUrls: ['./eshop-form.component.css']
  })

  export class EshopFormComponent implements OnInit {
   
    
    eshop: SaveEshop = {
      id: 0, 
      name: '',
      administrationId: 0, 
      percents: 0 
      };

      constructor(  
        private route: ActivatedRoute,
        private router: Router,
        private eshopService: EshopService,
        private toastyService: ToastyService) {
    
          route.params.subscribe(p => {
            this.eshop.id = +p['id'] || 0;
          });
        }

        ngOnInit() {
                       
            if (this.eshop.id) {
                this.eshopService.getScrapable(this.eshop.id).subscribe(p=>{this.eshop = p;});
                err => {
                  if (err.status == 404)
                    this.router.navigate(['/eshops']);
                };
        }
       
        }

        private setScrapable(p: Eshop) {
            this.eshop.id = p.id;
            this.eshop.name = p.name; 
            this.eshop.administrationId = p.administrationId;
            this.eshop.percents = p.percents;
          }
          
          submit() {
            var result$ =  this.eshopService.update(this.eshop); 
            result$.subscribe(eshop => {
              this.toastyService.success({
                title: 'Success', 
                msg: 'Duomenys išsaugoti sėkmingai.',
                theme: 'bootstrap',
                showClose: true,
                timeout: 5000
              });
              this.router.navigate(['/eshops'])
            });
          }
}