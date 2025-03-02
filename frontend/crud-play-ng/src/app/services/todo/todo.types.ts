export interface Todo {
  id: string;
  title: string;
  description?: string;
  isCompleted: boolean;
  dueDate: Date;
  Priority?: number;
}
