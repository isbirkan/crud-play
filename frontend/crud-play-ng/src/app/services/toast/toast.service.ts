import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { ToastMessage, ToastType } from './toast.types';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toasts: ToastMessage[] = [];
  private toastsSubject = new BehaviorSubject<ToastMessage[]>([]);
  toasts$ = this.toastsSubject.asObservable();

  /**
   * Presents a new toast message to the user.
   * @param type - Type of the toast message.
   * @param message - The message to display.
   * @param duration - Duration in milliseconds before the toast is removed.
   */
  showToast(type: ToastType, message: string, duration = 5000): void {
    const id = Date.now();
    const newToast: ToastMessage = { id, type, message };

    this.toasts.push(newToast);
    this.toastsSubject.next([...this.toasts]);

    setTimeout(() => this.removeToast(id), duration);
  }

  /**
   * Removes the selected toast message from the list.
   * @param id - Unique identifier of the toast message.
   */
  removeToast(id: number): void {
    this.toasts = this.toasts.filter((toast) => toast.id !== id);
    this.toastsSubject.next([...this.toasts]);
  }
}
