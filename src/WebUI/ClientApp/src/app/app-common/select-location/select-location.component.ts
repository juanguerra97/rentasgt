import { Component, ElementRef, EventEmitter, Input, NgZone, OnInit, Output, ViewChild } from '@angular/core';
import { MapsAPILoader } from '@agm/core';
import { LocationInfo } from '../../models/LocationInfo';
import { getAddressFromCoordinates } from '../../utils';

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
    if ('geolocation' in navigator && (this.latitude === null || this.longitude === null)) {
      this.gettingCoordinates = true;
      navigator.geolocation.getCurrentPosition((position) => {
        this.latitude = position.coords.latitude;
        this.longitude = position.coords.longitude;
        this.zoom = 15;
        this.gettingCoordinates = false;
      }, (error) => {
        if (error.code === error.PERMISSION_DENIED) {
          this.latitude = 14.6385986;
          this.longitude = -90.5137278;
          this.zoom = 15;
        }
        this.gettingCoordinates = false;
      });
    } else {
      this.zoom = 15;
    }
  }

  public markerDragEnd($event: any) {
    console.log($event);
    this.latitude = $event.latLng.lat();
    this.longitude = $event.latLng.lng();
  }

  public async onSelectLocation(): Promise<any> {
    try {
      this.onLocationSelected.emit(await getAddressFromCoordinates(this.latitude, this.longitude));
    } catch (error) {
      console.error(error);
    }
  }

}
