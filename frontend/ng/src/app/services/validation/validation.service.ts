import { Injectable } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ValidationService {
  /** Default validation messages */
  private validationMessages: Record<string, string> = {
    required: 'This field is required.',
    email: 'Enter a valid email address.',
    minlength: 'Minimum length not met.',
    maxlength: 'Maximum length exceeded.'
  };

  /**
   * Allows overriding validation messages per component
   * @param customMessages Custom validation messages dictionary
   */
  setValidationMessages(customMessages: Record<string, string>) {
    this.validationMessages = { ...this.validationMessages, ...customMessages };
  }

  /**
   * Retrieves all validation error messages for a form field \
   * @param control Form control to retrieve error messages from
   */
  getErrorMessages(control: AbstractControl | null): string[] {
    if (!control || !control.errors || !control.invalid) return [];
    return Object.keys(control.errors).map((key) => this.validationMessages[key] || 'Invalid field.');
  }
}
