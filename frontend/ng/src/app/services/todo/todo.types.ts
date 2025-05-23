export interface Todo {
  id: string;
  title: string;
  description?: string;
  isCompleted: boolean;
  dueDate: Date;
  priority?: number;
}
