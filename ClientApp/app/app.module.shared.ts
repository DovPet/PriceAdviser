import { AuthGuardService } from './services/auth-guard.service';
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
import { ExportService } from './services/export.service';
import { ExportComponent } from './components/export/export';
import { CreateProductComponent } from './components/create-product/create-product';
import { AuthService } from './services/auth.service';
import { HttpClientModule } from '@angular/common/http';
import { AUTH_PROVIDERS } from "angular2-jwt/angular2-jwt";

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
        ProductFormComponent,
        ExportComponent,
        CreateProductComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        HttpClientModule,
        ToastyModule.forRoot(),
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuardService]},

            { path: 'eshop/edit/:id', component: EshopFormComponent, canActivate: [AuthGuardService]},
            { path: 'eshops', component: EshopListComponent, canActivate: [AuthGuardService] },
            
            { path: 'products/new', component: CreateProductComponent },
            { path: 'products/prices/edit/:id', component: ProductFormComponent},
            { path: 'products', component: ProductListComponent },

            { path: 'export', component: ExportComponent },


            { path: '**', redirectTo: 'home' }

        ])
    ],
    providers: [
    AuthService,
    AUTH_PROVIDERS,
    AuthGuardService,

    EshopService,
    ProductService,
    ExportService,
    ScraperService
    
    ]
})
export class AppModuleShared {
    constructor(public auth: AuthService) {
        auth.handleAuthentication();
      }
}
