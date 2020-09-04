import { LocationInfo } from './models/LocationInfo';
import { MapsAPILoader } from '@agm/core';

export function getErrorsFromResponse(errorResponse): string[] {
  const errors: string[] = [];
  if (errorResponse.errors) {
    for (const prop in errorResponse.errors) {
      for (const error of errorResponse.errors[prop]) {
        errors.push(error);
      }
    }
  } else if (errorResponse.detail) {
    errors.push(errorResponse.detail);
  }
  return errors;
}

export function imgBlobToBase64(imgBlob: Blob): Promise<string|ArrayBuffer> {
  return new Promise<string|ArrayBuffer>((resolve, reject) => {
    const reader = new FileReader();
    reader.onloadend = () => {
      resolve(reader.result);
    };
    reader.onerror = reject;
    reader.readAsDataURL(imgBlob);
  });
}

export function getAddressFromCoordinates(latitude: number, longitude: number, mapsAPILoader: MapsAPILoader): Promise<LocationInfo> {
  return new Promise<LocationInfo>((resolve, reject) => {
      mapsAPILoader.load().then(() => {
        const geoCoder = new google.maps.Geocoder;
        geoCoder.geocode({ 'location': { lat: latitude, lng: longitude } }, (results, status) => {
          if (status === 'OK') {
            if (results[0]) {
              // this.address = results[0].formatted_address;
              const countryComponent = results[0].address_components.find(c => c.types.includes('country'));
              const stateComponent = results[0].address_components.find(c => c.types.includes('administrative_area_level_1'));
              const cityComponent = results[0].address_components.find(c => c.types.includes('locality'));
              resolve({
                formattedAddress: results[0].formatted_address,
                latitude: latitude,
                longitude: longitude,
                country: countryComponent.long_name,
                state: stateComponent.long_name,
                city: cityComponent.long_name,
              });
            } else {
              reject();
            }
          } else {
            reject();
          }
        });
      }).catch(reject);
  });
}

export function saveCurrentUserLocation(location: LocationInfo): void {
  localStorage.setItem('rentasgt.userLoc', JSON.stringify(location));
}

export function getLocationFromStorage(): LocationInfo {
  return JSON.parse(localStorage.getItem('rentasgt.userLoc'));
}

export function clearLocation(): void {
  localStorage.removeItem('rentasgt.userLoc');
}

export function isValidLocation(location: LocationInfo): boolean {
  return location !== null && location.country === 'Guatemala';
}
