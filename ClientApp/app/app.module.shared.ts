import { ScraperService } from './services/scraper.service';

import { ProductService } from './services/product.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { EshopFormComponent } from './components/eshop-form/eshop-form.component';
import { EshopListComponent } from './components/eshop/eshop-list';
import { ProductListComponent } from './components/products/product-list';
import { EshopService } from './services/eshop.service';
import { ToastyModule } from 'ng2-toasty';
import { ProductFormComponent } from './components/price-form/price-form.component';


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,
        EshopFormComponent,
        EshopListComponent,
        ProductListComponent,
        ProductFormComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        ToastyModule.forRoot(),
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },

            { path: 'eshop/edit/:id', component: EshopFormComponent},
            { path: 'eshops', component: EshopListComponent },
           // { path: 'api/SampleData/skytech', component: FetchDataComponent },
            
            { path: 'products/edit/:id', component: ProductFormComponent},
            { path: 'products', component: ProductListComponent },

            { path: '**', redirectTo: 'home' }

        ])
    ],
    providers: [
    EshopService,
    ProductService,
    ScraperService
    ]
})
export class AppModuleShared {
}
