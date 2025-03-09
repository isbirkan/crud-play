import { of } from 'rxjs';

export const loginResponse = of({
  access_token:
    'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlMzVkZjYxYy1mNDI3LTQ0YWEtOGZiOS1jOTdjYWQwZmU0NDIiLCJ1bmlxdWVfbmFtZSI6ImJpcmthbkB0b2RvLnBsYXkiLCJlbWFpbCI6ImJpcmthbkB0b2RvLnBsYXkiLCJleHAiOjE3MzkzMjEwNTIsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNTciLCJhdWQiOiJDcnVkUGxheS5BcGkifQ.Lmj8AU5HCvlz85ica5waqnCrlxvgFMjDjK64w6J_6m8',
  token_type: 'bearer',
  expires_in: 3599,
  refresh_token: 'r9Ri+4Z4gy1jS+Utxk+RAhyhxEe1wZZbVvC6XVdyuyo='
});

export const registerResponse = of('User registered successfully');

export const refreshTokenResponse = of({
  access_token:
    'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlMzVkZjYxYy1mNDI3LTQ0YWEtOGZiOS1jOTdjYWQwZmU0NDIiLCJ1bmlxdWVfbmFtZSI6ImJpcmthbkB0b2RvLnBsYXkiLCJlbWFpbCI6ImJpcmthbkB0b2RvLnBsYXkiLCJleHAiOjE3MzkzMjEwNTIsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNTciLCJhdWQiOiJDcnVkUGxheS5BcGkifQ.Lmj8AU5HCvlz85ica5waqnCrlxvgFMjDjK64w6J_6m8',
  token_type: 'bearer',
  expires_in: 3599,
  refresh_token: 'r9Ri+4Z4gy1jS+Utxk+RAhyhxEe1wZZbVvC6XVdyuyo='
});
