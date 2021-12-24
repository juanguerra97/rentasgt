import { AfterViewInit, Component, ElementRef, EventEmitter, Input, NgZone, OnInit, Output, ViewChild } from '@angular/core';
import { MapMarker } from '@angular/google-maps';
import { LocationInfo } from '../../models/LocationInfo';
import { getAddressFromCoordinates, getLocationFromStorage, isValidLocation, saveCurrentUserLocation } from '../../utils';

@Component({
  selector: 'app-select-location',
  templateUrl: './select-location.component.html',
  styleUrls: ['./select-location.component.css']
})
export class SelectLocationComponent implements OnInit, AfterViewInit {

  @Output() onLocationSelected: EventEmitter<LocationInfo> = new EventEmitter<LocationInfo>();
  @Input() public latitude: number = null;
  @Input() public longitude: number = null;

  mapOptions: google.maps.MapOptions = {
    center: { lat: this.latitude, lng: this.longitude },
    streetViewControl: false,
    fullscreenControl: false,
    mapTypeControl: false,
    mapTypeId: google.maps.MapTypeId.ROADMAP,
    gestureHandling: 'cooperative',
    restriction: { latLngBounds: {north: 17.806880, south: 13.748223, west: -92.210494, east: -88.177616 }, strictBounds: false}
  };

  markerOptions: google.maps.MarkerOptions = {
    draggable: true,
  };

  public gettingCoordinates = false;
  public zoom: number = 10;
  private geoCoder;
  private validatingMarkerPosition = false;

  private startLat: number = null;
  private startLng: number = null;

  @ViewChild('search')
  public searchElementRef: ElementRef;

  @ViewChild(MapMarker) mapMarker: MapMarker;

  constructor(
    private ngZone: NgZone,
  ) { }

  ngOnInit(): void {
    this.setCurrentLocation();
    this.geoCoder = new google.maps.Geocoder;
    
  }

  ngAfterViewInit() {

    this.mapOptions = Object.assign({}, this.mapOptions, { center: { lat: this.latitude, lng: this.longitude } });
    this.markerOptions = Object.assign({}, this.markerOptions, { position: { lat: this.latitude, lng: this.longitude } } )

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
          saveCurrentUserLocation(await getAddressFromCoordinates(this.latitude, this.longitude));
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

  public async markerDragEnd($event: google.maps.MapMouseEvent): Promise<any> {
    this.validatingMarkerPosition = true;
    const lat = $event.latLng.lat();
    const lng = $event.latLng.lng();
    const loc = await getAddressFromCoordinates(lat, lng);
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

  public markerDragStart($event: google.maps.MapMouseEvent): void {
    this.startLat = $event.latLng.lat();
    this.startLng = $event.latLng.lng();
  }

  public async onSelectLocation(): Promise<any> {
    const latitude = this.mapMarker.getPosition().lat();
    const longitude = this.mapMarker.getPosition().lng();
    try {
      this.onLocationSelected.emit(await getAddressFromCoordinates(latitude, longitude));
    } catch (error) {
      console.error(error);
    }
  }

}
