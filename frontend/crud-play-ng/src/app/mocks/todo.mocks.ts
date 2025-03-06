import { of } from 'rxjs';

import { Todo } from '../services/todo/todo.types';

let isCompleted = false;

const todo = {
  id: '1',
  title: "Ain't I a beautiful title?",
  description: 'I would like to think the same about my description!',
  isCompleted: false,
  dueDate: new Date(),
  priority: 1
} satisfies Todo;

const buildTodo = (index: number): Todo => ({
  id: index.toString(),
  title: `The higher entity chose ${index} as my title number!`,
  description: `The higher entity chose ${index} as my description number!`,
  isCompleted: (isCompleted = !isCompleted),
  dueDate: new Date(),
  priority: index
});

export const getTodosResponse = of(Array.from({ length: 10 }, (_, i) => buildTodo(i)));
