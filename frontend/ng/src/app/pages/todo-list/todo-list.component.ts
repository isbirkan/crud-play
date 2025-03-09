import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';

import { LoadingService } from '@/app/services/loading/loading.service';
import { TodoService } from '@/app/services/todo/todo.service';
import { Todo } from '@/app/services/todo/todo.types';

@Component({
  selector: 'app-todo-list',
  imports: [CommonModule],
  templateUrl: './todo-list.component.html',
  styleUrl: './todo-list.component.scss'
})
export class TodoListComponent implements OnInit {
  todos: Todo[] = [];

  constructor(
    private loadingService: LoadingService,
    private todoService: TodoService
  ) {}

  ngOnInit(): void {
    this.loadTodos();
  }

  loadTodos(): void {
    this.loadingService.show();
    this.todoService.getTodos().subscribe({
      next: (data) => {
        this.todos = data;
        this.loadingService.hide();
      },
      error: () => this.loadingService.hide()
    });
  }

  deleteTodo(id: string): void {
    this.todoService.deleteTodo(id).subscribe(() => {
      this.todos = this.todos.filter((todo) => todo.id !== id);
    });
  }
}
