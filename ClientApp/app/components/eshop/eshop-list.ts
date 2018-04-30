import { Eshop, SaveEshop } from './../../models/eshop';
import { EshopService } from './../../services/eshop.service';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from 'ng2-toasty';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
    templateUrl: 'eshop-list.html',
    styleUrls: ['eshop-list.css']
  })

  export class EshopListComponent implements OnInit {

    eshop: Eshop[];

    constructor(private eshopService: EshopService) { }

    ngOnInit() {
        this.eshopService.getScrapables().subscribe(eshop => this.eshop = eshop)
    }
    
  
  }