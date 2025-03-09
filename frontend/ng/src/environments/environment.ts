export const baseEnvironment = {
  production: false,

  API_BASE_URL: 'https:localhost:7257',

  AUTH_API: 'api/auth',
  TODO_API: 'api/v1/todo',

  USE_MOCK_DATA: false
};

export const environment = {
  ...baseEnvironment
};
