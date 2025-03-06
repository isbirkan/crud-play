import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, delay, tap } from 'rxjs/operators';

import TOAST_TYPE from '@/app/constants/toast-types.constants';
import { ToastService } from '@/app/services/toast/toast.service';
import { environment } from '@/environments/environment';

import { getStoredToken, removeTokens, saveTokens } from '../../helpers/auth.helper';
import { loginResponse, refreshTokenResponse, registerResponse } from '../../mocks/auth.mocks';

import { AuthRequest, AuthResponse, RegisterResponse } from './auth.types';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.API_BASE_URL;
  private authEndpoint = environment.AUTH_API;
  private _isAuthenticated = new BehaviorSubject<boolean>(!!getStoredToken());

  constructor(
    private http: HttpClient,
    private toastService: ToastService
  ) {}

  /** Exposes authentication state as Observable. */
  get isAuthenticated$(): Observable<boolean> {
    return this._isAuthenticated.asObservable();
  }

  /**
   * Logs in user
   * @param AuthRequest - Object containing `email` and `password`.
   */
  login(credentials: AuthRequest): Observable<AuthResponse> {
    if (environment.USE_MOCK_DATA) {
      console.log('🟡 Using mock Login response');
      return loginResponse.pipe(
        delay(500),
        tap((res) => {
          saveTokens(res);
          this._isAuthenticated.next(true);
          this.toastService.showToast(TOAST_TYPE.SUCCESS, 'Login successful!');
        })
      );
    }

    return this.http.post<AuthResponse>(`${this.apiUrl}/${this.authEndpoint}/login`, credentials).pipe(
      tap((res) => {
        saveTokens(res);
        this._isAuthenticated.next(true);
        this.toastService.showToast(TOAST_TYPE.SUCCESS, 'Login successful!');
      }),
      catchError(this.handleError)
    );
  }

  /** Logs out user. */
  logout(): void {
    removeTokens();
    this._isAuthenticated.next(false);
    this.toastService.showToast(TOAST_TYPE.INFO, 'You have been logged out.');
  }

  /**
   * Registers new user.
   * @param AuthRequest - Object containing `email` and `password`.
   */
  register(credentials: AuthRequest): Observable<RegisterResponse> {
    if (environment.USE_MOCK_DATA) {
      console.log('🟡 Using mock Register response');
      return registerResponse.pipe(
        delay(500),
        tap(() => this.toastService.showToast(TOAST_TYPE.SUCCESS, 'Registration successful!'))
      );
    }

    return this.http.post<RegisterResponse>(`${this.apiUrl}/${this.authEndpoint}/register`, credentials).pipe(
      tap(() => this.toastService.showToast('success', 'Registration successful!')),
      catchError(this.handleError)
    );
  }

  /**
   * Refreshes authentication token.
   * @param refresh_token - string containing the initially generated Refresh Token.
   */
  refreshToken(refreshToken: string): Observable<AuthResponse> {
    if (environment.USE_MOCK_DATA) {
      console.log('🟡 Using mock Refresh Token response');
      return refreshTokenResponse.pipe(
        delay(500),
        tap((res) => {
          saveTokens(res);
          this._isAuthenticated.next(true);
          this.toastService.showToast(TOAST_TYPE.INFO, 'Token refreshed!');
        })
      );
    }

    return this.http.post<AuthResponse>(`${this.apiUrl}/${this.authEndpoint}/refreshToken`, { refreshToken }).pipe(
      tap((res) => {
        saveTokens(res);
        this._isAuthenticated.next(true);
        this.toastService.showToast(TOAST_TYPE.INFO, 'Token refreshed!');
      }),
      catchError(this.handleError)
    );
  }

  /** Handles API Errors. */
  private handleError(error: any): Observable<never> {
    console.error('🔴 AuthService Error:', error);
    const errorMessage = error?.error?.message || 'Something went wrong, please try again.';

    this.toastService.showToast(TOAST_TYPE.ERROR, errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
