import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import ROUTES from '@/app/constants/routes.constants';
import { AuthService } from '@/app/services/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {
    let isAuthenticated = false;

    this.authService.isAuthenticated$.subscribe((authStatus) => {
      isAuthenticated = authStatus;
    });

    if (!isAuthenticated) {
      this.router.navigate([ROUTES.LogIn.path]);
      return false;
    }

    return true;
  }
}
