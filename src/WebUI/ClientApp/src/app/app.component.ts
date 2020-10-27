import { Component, OnInit } from '@angular/core';
import { MapsAPILoader } from '@agm/core';
import { clearLocation, getAddressFromCoordinates, saveCurrentUserLocation } from './utils';
import { RatingToProductDto, RatingToProductsClient } from './rentasgt-api';
import { MatDialog } from '@angular/material/dialog';
import { RateProductComponent } from './app-common/rate-product/rate-product.component';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { Router } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';

declare var cordova;
declare var screen;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  constructor(
    private authService: AuthorizeService,
    public oidcSecurityService: OidcSecurityService,
    private router: Router,
    private mapsAPILoader: MapsAPILoader,
    private ratingToProductsClient: RatingToProductsClient,
    private matDialog: MatDialog,
  ) {
    this.authService.loadUser();
  }

  ngOnInit(): void {
    this.addDeviceReadyEvent();
    clearLocation();
    this.getUserLocation();
    // this.checkIfThereIsPendingProductRating();
  }

  private addDeviceReadyEvent(): void {
    document.addEventListener('deviceready', () => {
      if (cordova) {
        (window as any).handleOpenURL = async (url: string) => {
          // await this.authService.completeSignIn(url);
          this.oidcSecurityService.checkAuth(url).subscribe(res => {
            if (res === true) {
              this.oidcSecurityService.userData$.subscribe(u => {
                (<any>navigator).showToast("Bienvenido!");
              });
            }
          }, console.error);

          this.router.navigate(['/articulos']);
        };
      }
      if (screen) {
        screen.orientation.lock('portrait');
      }
    } , false)
  }

  private getUserLocation(): void {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition(async (position) => {
        const loc = await getAddressFromCoordinates(position.coords.latitude, position.coords.longitude, this.mapsAPILoader);
        saveCurrentUserLocation(loc);
      }, (error) => {
        if (error.code === error.PERMISSION_DENIED) {
        }
      });
    }
  }

  private checkIfThereIsPendingProductRating(): void {
    this.ratingToProductsClient.getPending()
      .subscribe((res) => {
        this.askUserForProductRating(res);
      }, console.error);
  }

  private askUserForProductRating(pendingRating: RatingToProductDto): void {
    const ref = this.matDialog.open(RateProductComponent, {
      width: '350px',
      panelClass: 'mat-dialog-panel',
      data: pendingRating
    });
  }

}
