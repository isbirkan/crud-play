import { Routes } from '@angular/router';

import ROUTES from '@/app/constants/routes.constants';
import { AuthGuard } from '@/app/guards/auth.guard';

import { LayoutComponent } from './pages/layout/layout.component';
import { LoginComponent } from './pages/login/login.component';
import { TodoListComponent } from './pages/todo-list/todo-list.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: ROUTES.TodoList.path, pathMatch: 'full' },
      {
        path: ROUTES.TodoList.path,
        component: TodoListComponent,
        canActivate: [AuthGuard],
        data: { title: ROUTES.TodoList.title }
      }
    ]
  },
  { path: ROUTES.LogIn.path, component: LoginComponent, data: { title: ROUTES.LogIn.title } },
  { path: '**', redirectTo: '' }
];
