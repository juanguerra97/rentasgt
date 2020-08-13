
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
