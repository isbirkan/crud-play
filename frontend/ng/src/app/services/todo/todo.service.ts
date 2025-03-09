import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import TOAST_TYPE from '@/app/constants/toast-types.constants';
import { getTodosResponse } from '@/app/mocks/todo.mocks';
import { ToastService } from '@/app/services/toast/toast.service';
import { environment } from '@/environments/environment';

import { Todo } from './todo.types';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  private apiUrl = environment.API_BASE_URL;
  private todoEndpoint = environment.TODO_API;

  constructor(
    private http: HttpClient,
    private toastService: ToastService
  ) {}

  /** Get all Todos */
  getTodos(): Observable<Todo[]> {
    if (environment.USE_MOCK_DATA) {
      console.log('🟡 Using mock getTodos response');
      return getTodosResponse.pipe(
        tap(() => this.toastService.showToast(TOAST_TYPE.INFO, 'Todos loaded successfully!')),
        catchError(this.handleError)
      );
    }

    return this.http.get<Todo[]>(`${this.apiUrl}/${this.todoEndpoint}`).pipe(
      tap(() => this.toastService.showToast(TOAST_TYPE.INFO, 'Todos loaded successfully!')),
      catchError(this.handleError)
    );
  }

  /** Get Todos by User ID */
  getTodosByUserId(userId: string): Observable<Todo[]> {
    return this.http.get<Todo[]>(`${this.apiUrl}/${this.todoEndpoint}?userId=${userId}`).pipe(
      tap(() => this.toastService.showToast(TOAST_TYPE.INFO, 'User Todos loaded!')),
      catchError(this.handleError)
    );
  }

  /** Get Todo by Id */
  getTodoById(id: string): Observable<Todo> {
    return this.http.get<Todo>(`${this.apiUrl}/${this.todoEndpoint}/${id}`).pipe(
      tap(() => this.toastService.showToast(TOAST_TYPE.INFO, 'Todo loaded!')),
      catchError(this.handleError)
    );
  }

  /** Create a new Todo */
  createTodo(todo: Partial<Todo>): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${this.todoEndpoint}`, todo).pipe(
      tap(() => this.toastService.showToast(TOAST_TYPE.SUCCESS, 'Todo created!')),
      catchError(this.handleError)
    );
  }

  /** Update a Todo */
  updateTodo(id: string, todo: Partial<Todo>): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${this.todoEndpoint}/${id}`, todo).pipe(
      tap(() => this.toastService.showToast(TOAST_TYPE.SUCCESS, 'Todo updated!')),
      catchError(this.handleError)
    );
  }

  /** Delete a Todo */
  deleteTodo(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${this.todoEndpoint}/${id}`).pipe(
      tap(() => this.toastService.showToast(TOAST_TYPE.SUCCESS, 'Todo deleted!')),
      catchError(this.handleError)
    );
  }

  authDebug(): Observable<any> {
    console.log('🟢 Calling Auth Debug Endpoint...');
    return this.http.get<any>(`${this.apiUrl}/${this.todoEndpoint}/auth-debug`).pipe(
      tap((res) => console.log('🟢 Auth Debug:', res)),
      catchError(this.handleError)
    );
  }

  /** Handle API Errors */
  private handleError(error: any): Observable<never> {
    console.error('🔴 TodoService Error:', error);
    const errorMessage = error?.error?.message || 'Something went wrong, please try again.';
    this.toastService.showToast(TOAST_TYPE.ERROR, errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
