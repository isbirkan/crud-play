import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import ROUTES from '@/app/constants/routes.constants';
import { AuthService } from '@/app/services/auth/auth.service';
import { ValidationService } from '@/app/services/validation/validation.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string | null = null;
  isLoading = false;
  returnUrl: string | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    public validationService: ValidationService
  ) {
    if (this.authService.isAuthenticated$) {
      this.router.navigate([ROUTES.TodoList.path]);
    }

    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
    this.validationService.setValidationMessages({
      required: 'Oops! You forgot this field.',
      email: 'Please enter a proper email.',
      minlength: 'The password is too short!'
    });

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || ROUTES.TodoList.path;
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  login(): void {
    if (this.loginForm.invalid) {
      this.errorMessage = 'Please fill in all required fields correctly.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    this.authService.login(this.loginForm.value).subscribe({
      next: () => {
        this.router.navigate([this.returnUrl!]);
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Login failed. Please try again.';
        this.isLoading = false;
      }
    });
  }
}
