import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, catchError, throwError } from 'rxjs';

import ROUTES from '@/app/constants/routes.constants';
import TOAST_TYPE from '@/app/constants/toast-types.constants';
import { AuthService } from '@/app/services/auth/auth.service';
import { ToastService } from '@/app/services/toast/toast.service';

import { getStoredToken } from '../helpers/auth.helper';

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  console.log('🟡 Auth Interceptor');

  const router = inject(Router);
  const authService = inject(AuthService);
  const toastService = inject(ToastService);
  const token = getStoredToken();

  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        const errorMessage = `Auth Interceptor Error: ${error?.error?.message || 'Unauthorized!'}`;
        console.error(`🔴 ${errorMessage}`);
        toastService.showToast(TOAST_TYPE.ERROR, errorMessage, 10000);

        authService.logout();
        router.navigate([ROUTES.LogIn.path]);
      }
      return throwError(() => error);
    })
  );
};
