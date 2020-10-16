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
import { ToastrModule } from 'ngx-toastr';
import { AppCommonModule } from './app-common/app-common.module';
import { ProductsComponent } from './products/products.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ChatsComponent } from './chats/chats.component';
import { RentRequestsRequestorComponent } from './rent-requests-requestor/rent-requests-requestor.component';
import { RentsComponent } from './rents/rents.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { ProfileEditComponent } from './profile-edit/profile-edit.component';
import { DpiEditComponent } from './dpi-edit/dpi-edit.component';
import { AddressEditComponent } from './address-edit/address-edit.component';
import { OnlyModeradorGuard } from '../api-authorization/only-moderador.guard';
import { AppLayoutComponent } from './app-layout/app-layout.component';
import { API_BASE_URL } from './rentasgt-api';

@NgModule({
  declarations: [
    AppComponent,
    AppLayoutComponent,
    NavMenuComponent,
    HomeComponent,
    ProductsComponent,
    ProductDetailComponent,
    ChatsComponent,
    RentRequestsRequestorComponent,
    RentsComponent,
    UserProfileComponent,
    ProfileEditComponent,
    DpiEditComponent,
    AddressEditComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    FontAwesomeModule,
    HttpClientModule,
    ApiAuthorizationModule,
    AppCommonModule,
    ToastrModule.forRoot(),
    RouterModule.forRoot([
      { path: '', pathMatch: 'full', redirectTo: 'articulos' },
      { path: '', component: AppLayoutComponent, children: [
          { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule),
            canActivate: [OnlyAdminGuard], canActivateChild: [OnlyAdminGuard], },
          { path: 'moderador', loadChildren: () => import('./moderador/moderador.module').then(m => m.ModeradorModule),
            canActivate: [OnlyModeradorGuard], canActivateChild: [OnlyModeradorGuard], },
          { path: 'propietario', loadChildren: () => import('./owner/owner.module').then(m => m.OwnerModule),
            canActivate: [AuthorizeGuard], canActivateChild: [AuthorizeGuard]
          },
          { path: 'articulos', component: ProductsComponent },
          { path: 'articulos/detalle/:id', component: ProductDetailComponent },
          { path: 'mensajes', component: ChatsComponent, canActivate: [AuthorizeGuard] },
          { path: 'solicitudes', component: RentRequestsRequestorComponent, canActivate: [AuthorizeGuard] },
          { path: 'rentas', component: RentsComponent, canActivate: [AuthorizeGuard] },
          { path: 'perfil', component: UserProfileComponent, canActivate: [AuthorizeGuard] },
        ]},

    ], { scrollPositionRestoration: 'enabled' }),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    { provide: API_BASE_URL, useValue: 'https://rentasguatemala.com' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
