import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import ROUTES from '@/app/constants/routes.constants';
import TOAST_TYPE from '@/app/constants/toast-types.constants';
import { AuthService } from '@/app/services/auth/auth.service';
import { ToastService } from '@/app/services/toast/toast.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.authService.isAuthenticated$.pipe(
      map((isAuthenticated) => {
        if (!isAuthenticated) {
          const errorMessage = 'AuthGuard Error: User not authenticated, redirecting to login.';
          console.log(`🔴 ${errorMessage}`);
          this.toastService.showToast(TOAST_TYPE.ERROR, errorMessage);

          this.router.navigate([ROUTES.LogIn.path], { queryParams: { returnUrl: state.url } });
          return false;
        }

        return true;
      })
    );
  }
}
