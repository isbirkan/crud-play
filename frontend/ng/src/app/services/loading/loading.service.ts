import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private isLoading = new BehaviorSubject<boolean>(false);

  /** Observable for components to subscribe to */
  get isLoading$(): Observable<boolean> {
    return this.isLoading.asObservable();
  }

  /** Show the loading overlay */
  show(): void {
    this.isLoading.next(true);
  }

  /** Hide the loading overlay */
  hide(): void {
    this.isLoading.next(false);
  }
}
