import { ScraperService } from './../../services/scraper.service';
import { Router } from '@angular/router';
import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { ToastyService } from 'ng2-toasty';

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html',
    styleUrls: ['fetchdata.css']
})
export class FetchDataComponent {
    
    isValid: boolean = true;
    constructor(private router: Router, private scraperService: ScraperService,private toastyService: ToastyService) {
    }
    isValidForm() {
        return this.isValid;
    }
 skytech() {
    var result$ =  this.scraperService.skytech(); 
      this.router.navigate(['/fetch-data'])
      result$.subscribe(skytech => {
       
    });
    this.toastyService.success({
        title: 'Scraper', 
        msg: 'Duomenys gaunami iš "Skytech". Norėdami sekti kas vyksta spauskite meniu: Procesai',
        theme: 'bootstrap',
        showClose: true,
        timeout: 5000
      });
  }
  kilobaitas() {
    var result$ =  this.scraperService.kilobaitas(); 
      this.router.navigate(['/fetch-data'])
      result$.subscribe(kilobaitas => {
        this.toastyService.success({
          title: 'Scraper', 
          msg: 'Duomenys gaunami iš "Kilobaitas". Norėdami sekti kas vyksta spauskite meniu: Procesai',
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        });
    });
    }
  
  fortakas() {
    var result$ =  this.scraperService.fortakas(); 
      this.router.navigate(['/fetch-data'])
      result$.subscribe(fortakas => {
        this.toastyService.success({
          title: 'Scraper', 
          msg: 'Duomenys gaunami iš "Fortakas". Norėdami sekti kas vyksta spauskite meniu: Procesai',
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        });
    });
  }
  topocentras() {
    var result$ =  this.scraperService.topocentras(); 
      this.router.navigate(['/fetch-data'])
      result$.subscribe(topocentras => {
        this.toastyService.success({
          title: 'Scraper', 
          msg: 'Duomenys gaunami iš "Topo centras". Norėdami sekti kas vyksta spauskite meniu: Procesai',
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        });
    });
  }
  all() {
    var result$ =  this.scraperService.all(); 
      this.router.navigate(['/fetch-data'])
      result$.subscribe(all => {
        this.toastyService.success({
          title: 'Scraper', 
          msg: 'Duomenys gaunami iš visų parduotuvių. Norėdami sekti kas vyksta spauskite meniu: Procesai',
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        });
    });
  }
}

