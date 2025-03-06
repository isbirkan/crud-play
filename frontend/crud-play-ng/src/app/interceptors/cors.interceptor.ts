import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

import TOAST_TYPE from '@/app/constants/toast-types.constants';
import { ToastService } from '@/app/services/toast/toast.service';

export const corsInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  console.log('🟡 CORS Interceptor');

  const toastService = inject(ToastService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 0) {
        const errorMessage = `CORS Error: ${error?.error?.message || 'Preflight request blocked.'}`;
        console.error(`🔴 ${errorMessage}`);
        toastService.showToast(TOAST_TYPE.ERROR, errorMessage);
      }

      return throwError(() => error);
    })
  );
};
