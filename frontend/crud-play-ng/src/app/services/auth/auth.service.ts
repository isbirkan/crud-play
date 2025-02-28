import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, delay, tap } from 'rxjs/operators';

import { environment } from '@/environments/environment';

import { loginResponse, refreshTokenResponse, registerResponse } from '../../mocks/auth.mocks';

import { getStoredToken, removeTokens, saveTokens } from './auth.helper';
import { AuthRequest, AuthResponse, RegisterResponse } from './auth.types';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.API_BASE_URL;
  private authEndpoint = environment.AUTH_API;
  private _isAuthenticated = new BehaviorSubject<boolean>(!!getStoredToken());

  constructor(private http: HttpClient) {}

  /** Exposes authentication state as Observable */
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
        })
      );
    }

    return this.http.post<AuthResponse>(`${this.apiUrl}/${this.authEndpoint}/login`, credentials).pipe(
      tap((res) => {
        saveTokens(res);
        this._isAuthenticated.next(true);
      }),
      catchError(this.handleError)
    );
  }

  /** Logs out user */
  logout(): void {
    removeTokens();
    this._isAuthenticated.next(false);
  }

  /**
   * Registers new user
   * @param AuthRequest - Object containing `email` and `password`.
   */
  register(credentials: AuthRequest): Observable<RegisterResponse> {
    if (environment.USE_MOCK_DATA) {
      console.log('🟡 Using mock Register response');
      return registerResponse.pipe(
        delay(500),
        tap((res) => console.log('Registration Successful:', res))
      );
    }

    return this.http.post<RegisterResponse>(`${this.apiUrl}/${this.authEndpoint}/register`, credentials).pipe(
      tap((res) => console.log('Registration Successful:', res)),
      catchError(this.handleError)
    );
  }

  /**
   * Refreshes authentication token
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
        })
      );
    }

    return this.http.post<AuthResponse>(`${this.apiUrl}/${this.authEndpoint}/refreshToken`, { refreshToken }).pipe(
      tap((res) => {
        saveTokens(res);
        this._isAuthenticated.next(true);
      }),
      catchError(this.handleError)
    );
  }

  /** Handles API Errors */
  private handleError(error: any): Observable<never> {
    console.error('AuthService Error:', error);
    return throwError(() => new Error(error?.error?.message || 'Something went wrong, please try again.'));
  }
}
