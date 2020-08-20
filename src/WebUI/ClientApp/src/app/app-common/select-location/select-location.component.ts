import { Component, ElementRef, EventEmitter, Input, NgZone, OnInit, Output, ViewChild } from '@angular/core';
import { MapsAPILoader } from '@agm/core';
import { LocationInfo } from '../../models/LocationInfo';
import { getAddressFromCoordinates, getLocationFromStorage, isValidLocation, saveCurrentUserLocation } from '../../utils';

@Component({
  selector: 'app-select-location',
  templateUrl: './select-location.component.html',
  styleUrls: ['./select-location.component.css']
})
export class SelectLocationComponent implements OnInit {

  @Output() onLocationSelected: EventEmitter<LocationInfo> = new EventEmitter<LocationInfo>();
  @Input() public latitude: number = null;
  @Input() public longitude: number = null;

  public gettingCoordinates = false;
  public zoom: number;
  private geoCoder;
  private validatingMarkerPosition = false;

  private startLat: number = null;
  private startLng: number = null;

  @ViewChild('search')
  public searchElementRef: ElementRef;

  constructor(
    private mapsAPILoader: MapsAPILoader,
    private ngZone: NgZone,
  ) { }

  ngOnInit(): void {
    this.mapsAPILoader.load().then(() => {
      this.setCurrentLocation();
      this.geoCoder = new google.maps.Geocoder;

      const autocomplete = new google.maps.places.Autocomplete(this.searchElementRef.nativeElement);
      autocomplete.setComponentRestrictions({
        country: ['gt']
      });
      autocomplete.addListener('place_changed', () => {
        this.ngZone.run(() => {
          const place: google.maps.places.PlaceResult = autocomplete.getPlace();
          if (place.geometry === undefined || place.geometry === null) {
            return;
          }
          this.latitude = place.geometry.location.lat();
          this.longitude = place.geometry.location.lng();
          this.zoom = 10;
        });
      });
    });
  }

  private setCurrentLocation() {
    if (this.latitude === null || this.longitude === null) {
      const location = getLocationFromStorage();
      if (isValidLocation(location)) {
        this.latitude = location.latitude;
        this.longitude = location.longitude;
      } else if ('geolocation' in navigator) {
        this.gettingCoordinates = true;
        navigator.geolocation.getCurrentPosition(async (position) => {
          this.latitude = position.coords.latitude;
          this.longitude = position.coords.longitude;
          saveCurrentUserLocation(await getAddressFromCoordinates(this.latitude, this.longitude, this.mapsAPILoader));
          this.zoom = 15;
          this.gettingCoordinates = false;
        }, (error) => {
          if (error.code === error.PERMISSION_DENIED) {
            this.latitude = 14.6385986;
            this.longitude = -90.5137278;
          }
          this.gettingCoordinates = false;
        });
      }
    }
    this.zoom = 15;
  }

  public async markerDragEnd($event: any): Promise<any> {
    this.validatingMarkerPosition = true;
    const lat = $event.latLng.lat();
    const lng = $event.latLng.lng();
    const loc = await getAddressFromCoordinates(lat, lng, this.mapsAPILoader);
    if (!isValidLocation(loc)) {
      this.updateLatLng(this.startLat, this.startLng);
    } else {
      this.updateLatLng(lat, lng);
    }
    this.validatingMarkerPosition = false;
  }

  private updateLatLng(lat: number, lng: number): void {
    this.latitude = lat;
    this.longitude = lng;
  }

  public markerDragStart($event: any): void {
    this.startLat = $event.latLng.lat();
    this.startLng = $event.latLng.lng();
  }

  public async onSelectLocation(): Promise<any> {
    try {
      this.onLocationSelected.emit(await getAddressFromCoordinates(this.latitude, this.longitude, this.mapsAPILoader));
    } catch (error) {
      console.error(error);
    }
  }

}
