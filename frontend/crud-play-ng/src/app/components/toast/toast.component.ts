import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

import TOAST_TYPE from '@/app/constants/toast-types.constants';
import { ToastService } from '@/app/services/toast/toast.service';

import { ErrorIconComponent } from './icons/error-icon.component';
import { InfoIconComponent } from './icons/info-icon.component';
import { SuccessIconComponent } from './icons/success-icon.component';
import { WarningIconComponent } from './icons/warning-icon.component';

@Component({
  selector: 'app-toast',
  imports: [CommonModule, SuccessIconComponent, ErrorIconComponent, WarningIconComponent, InfoIconComponent],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.scss'
})
export class ToastComponent {
  toastTypes = TOAST_TYPE;

  constructor(private toastService: ToastService) {}

  /** Getter for Toasts array */
  get toasts$() {
    return this.toastService.toasts$;
  }

  /** Closes the selected toast message */
  closeToast(id: number) {
    this.toastService.removeToast(id);
  }
}
