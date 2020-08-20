import { Component, OnInit } from '@angular/core';
import { MapsAPILoader } from '@agm/core';
import { clearLocation, getAddressFromCoordinates, saveCurrentUserLocation } from './utils';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  constructor(
    private mapsAPILoader: MapsAPILoader,
  ) {
  }

  ngOnInit(): void {
    clearLocation();
    this.getUserLocation();
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

}
