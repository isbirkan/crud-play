import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';

import ROUTES from '@/app/constants/routes.constants';
import TOAST_TYPE from '@/app/constants/toast-types.constants';
import { AuthService } from '@/app/services/auth/auth.service';
import { ToastService } from '@/app/services/toast/toast.service';

@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, CommonModule, RouterModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  isAuthenticated$: Observable<boolean>;

  routes = ROUTES;
  isMobileMenuOpen = false;

  constructor(
    private router: Router,
    private authService: AuthService,
    private toastService: ToastService
  ) {
    this.isAuthenticated$ = this.authService.isAuthenticated$;
  }

  toggleMenu(): void {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  showTestToast(): void {
    this.toastService.showToast(TOAST_TYPE.INFO, 'This is a test toast!');
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate([ROUTES.LogIn]);
  }
}
