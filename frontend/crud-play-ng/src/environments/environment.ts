export const baseEnvironment = {
  production: false,

  API_BASE_URL: 'https:localhost:7257/api',

  AUTH_API: 'auth',
  TODO_API: 'v1/todo',

  USE_MOCK_DATA: true
};

export const environment = {
  ...baseEnvironment
};
