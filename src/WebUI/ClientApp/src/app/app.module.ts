import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { OnlyAdminGuard } from '../api-authorization/only-admin.guard';
import { AppCommonModule } from './app-common/app-common.module';
import { ProductsComponent } from './products/products.component';
import { ProductsListComponent } from './products-list/products-list.component';
import { ProductsFilterComponent } from './products-filter/products-filter.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ProductsListComponent,
    ProductsFilterComponent,
    ProductsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    FontAwesomeModule,
    HttpClientModule,
    ApiAuthorizationModule,
    AppCommonModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule),
        canActivate: [OnlyAdminGuard], canActivateChild: [OnlyAdminGuard], },
      { path: 'propietario', loadChildren: () => import('./owner/owner.module').then(m => m.OwnerModule),
        canActivate: [AuthorizeGuard], canActivateChild: [AuthorizeGuard]
      },
      { path: 'articulos', component: ProductsComponent },
    ]),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
