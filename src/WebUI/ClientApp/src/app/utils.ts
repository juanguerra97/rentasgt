
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
