const todo = {
  id: '0b6032c2-fb52-4c8b-81d6-e96509098448',
  title: 'React to a TODO',
  description: 'This is the time to react to a TODO, which is present to you with the help of React.js',
  isCompleted: false,
  dueDate: new Date(),
  priority: 999
}

export const getTodoList = {
  ok: true,
  status: 200,
  json: () =>
    Promise.resolve([todo])
};

export const getTodoById = {
  ok: true,
  status: 200,
  json: () =>
    Promise.resolve(todo)
};

export const getTodoByUserId = {
  ok: true,
  status: 200,
  json: () =>
    Promise.resolve(todo)
};

export const postTodo = {
  ok: true,
  status: 200,
  json: () =>
    Promise.resolve()
};

export const patchTodo = {
  ok: true,
  status: 200,
  json: () =>
    Promise.resolve()
};

export const deleteTodo = {
  ok: true,
  status: 200,
  json: () =>
    Promise.resolve()
};