import { Directive } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appAlphanumeric]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: AlphanumericDirective, multi: true }
  ]
})
export class AlphanumericDirective implements Validator {

  validate(control: AbstractControl): ValidationErrors | null {
    const value = control.value;

    if (!value) {
      return null; 
    }
    const isValidFormat = /^[a-zA-Z0-9]{10}$/.test(value);
    return isValidFormat ? null : { alphanumeric: true };
  }
}
