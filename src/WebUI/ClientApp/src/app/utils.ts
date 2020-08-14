import { LocationInfo } from './models/LocationInfo';

export function getErrorsFromResponse(errorResponse): string[] {
  const errors: string[] = [];
  if (errorResponse.errors) {
    for (const prop in errorResponse.errors) {
      for (const error of errorResponse.errors[prop]) {
        errors.push(error);
      }
    }
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

export function getAddressFromCoordinates(latitude: number, longitude: number): Promise<LocationInfo> {
  return new Promise<LocationInfo>((resolve, reject) => {
    this.geoCoder.geocode({ 'location': { lat: latitude, lng: longitude } }, (results, status) => {
      if (status === 'OK') {
        if (results[0]) {
          this.zoom = 15;
          // this.address = results[0].formatted_address;
          const countryComponent = results[0].address_components.find(c => c.types.includes('country'));
          const stateComponent = results[0].address_components.find(c => c.types.includes('administrative_area_level_1'));
          resolve({
            formattedAddress: results[0].formatted_address,
            latitude: latitude,
            longitude: longitude,
            country: countryComponent.long_name,
            state: stateComponent.long_name
          });
        }
      }
      reject();
    });
  });
}
