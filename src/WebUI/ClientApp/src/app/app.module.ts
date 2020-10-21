import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizationGuard } from './auth/authorization.guard';
import { OnlyAdminGuard } from './auth/only-admin.guard';
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
import { OnlyModeradorGuard } from './auth/only-moderador.guard';
import { AppLayoutComponent } from './app-layout/app-layout.component';
import { API_BASE_URL } from './rentasgt-api';
import { AuthModule, EventTypes, LogLevel, OidcConfigService, PublicEventsService } from 'angular-auth-oidc-client';
import { environment } from '../environments/environment';
import { filter } from 'rxjs/operators';
import { AuthInterceptor } from './auth.interceptor';

export function configureAuth(oidcConfigService: OidcConfigService) {
  return () =>
      oidcConfigService.withConfig({
          stsServer: 'https://rentasguatemala.com',
          redirectUrl: 'rentasgt://callback',
          postLogoutRedirectUri: 'rentasgt://callback',
          clientId: 'rentasgt.MobileApp',
          scope: 'openid profile rentasgt.WebUIAPI',
          responseType: 'code',
          silentRenew: true,
          silentRenewUrl: `${window.location.origin}/silent-renew.html`,
          renewTimeBeforeTokenExpiresInSeconds: 10,
          logLevel: environment.production ? LogLevel.None : LogLevel.Debug,
      });
}

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
            canActivate: [AuthorizationGuard], canActivateChild: [AuthorizationGuard]
          },
          { path: 'articulos', component: ProductsComponent },
          { path: 'articulos/detalle/:id', component: ProductDetailComponent },
          { path: 'mensajes', component: ChatsComponent, canActivate: [AuthorizationGuard] },
          { path: 'solicitudes', component: RentRequestsRequestorComponent, canActivate: [AuthorizationGuard] },
          { path: 'rentas', component: RentsComponent, canActivate: [AuthorizationGuard] },
          { path: 'perfil', component: UserProfileComponent, canActivate: [AuthorizationGuard] },
        ]},

    ], { scrollPositionRestoration: 'enabled' }),
    AuthModule.forRoot(),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: API_BASE_URL, useValue: 'https://rentasguatemala.com' },
    OidcConfigService,
        {
            provide: APP_INITIALIZER,
            useFactory: configureAuth,
            deps: [OidcConfigService],
            multi: true,
        },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor(private readonly eventService: PublicEventsService) {
    this.eventService
        .registerForEvents()
        .pipe(filter((notification) => notification.type === EventTypes.ConfigLoaded))
        .subscribe((config) => console.log('ConfigLoaded', config));
}
}
