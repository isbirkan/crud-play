export type TodoItem = {
  id: string;
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate: string;
  priority: number;
}

export type TodoList = TodoItem[];

export type TodoRequest = {
  userId: string;
  title: string;
  description: string;
  isCompleted: string;
  dueDate: string;
  priority: number;
}