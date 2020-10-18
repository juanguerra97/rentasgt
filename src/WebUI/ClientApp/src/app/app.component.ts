import { Component, OnInit } from '@angular/core';
import { MapsAPILoader } from '@agm/core';
import { clearLocation, getAddressFromCoordinates, saveCurrentUserLocation } from './utils';
import { RatingToProductDto, RatingToProductsClient } from './rentasgt-api';
import {MatDialog} from '@angular/material/dialog';
import {RateProductComponent} from './app-common/rate-product/rate-product.component';

declare var cordova;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  constructor(
    private mapsAPILoader: MapsAPILoader,
    private ratingToProductsClient: RatingToProductsClient,
    private matDialog: MatDialog,
  ) {
  }

  ngOnInit(): void {
    this.addDeviceReadyEvent();
    clearLocation();
    this.getUserLocation();
    this.checkIfThereIsPendingProductRating();
  }

  private addDeviceReadyEvent(): void {
    document.addEventListener('deviceready', () => {
      if (cordova) {
        (window as any).handleOpenURL = (url: string) => {
          console.log(url);
        };
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
