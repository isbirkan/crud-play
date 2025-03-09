const ROUTES = {
  Home: { path: '/', title: 'crud-play' },
  LogIn: { path: 'login', title: 'crud-play - LogIn' },
  Register: { path: 'register', title: 'crud-play - Register' },
  TodoList: { path: 'todo-list', title: 'crud-play - Todo List' },
  TodoItem: { path: 'todo-item', title: 'crud-play - Todo: %%todo-title%%' }
} as const;

export default ROUTES;
