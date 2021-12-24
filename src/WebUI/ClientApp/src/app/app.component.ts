import { Component, OnInit } from '@angular/core';
import { clearLocation, getAddressFromCoordinates, saveCurrentUserLocation } from './utils';
import { RatingToProductDto, RatingToProductsClient } from './rentasgt-api';
import { MatDialog } from '@angular/material/dialog';
import { RateProductComponent } from './app-common/rate-product/rate-product.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  constructor(
    private ratingToProductsClient: RatingToProductsClient,
    private matDialog: MatDialog,
  ) {
  }

  ngOnInit(): void {
    clearLocation();
    this.getUserLocation();
    this.checkIfThereIsPendingProductRating();
  }

  private getUserLocation(): void {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition(async (position) => {
        const loc = await getAddressFromCoordinates(position.coords.latitude, position.coords.longitude);
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
