import { AxiosRequestConfig, AxiosResponse } from 'axios';

import { AuthRequest, AuthResponse, TokenRefreshPromise } from './authorization.types';

import * as authMock from '@/api/authorization/authorization.mocks';
import { axiosClient } from '@/axios/client';
import { apiBaseUrl, authApi , shouldUseMockData } from '@/config';
import { saveTokens } from '@/helpers/tokenHelper';

let refreshTokenPromise: TokenRefreshPromise | null = null;

/**
 * Generates new access tokens for the user
 * @param loginRequest - Login request, containing email and password pair
 * @returns New access and refresh tokens.
 */
export const login = async (loginRequest: AuthRequest): Promise<AuthResponse> => {
  if (shouldUseMockData) {
    return authMock.login.json() as Promise<AuthResponse>;
  }

  const response = await sendLoginRequest(loginRequest);
  saveTokens(response.data);
  return response.data;
 }

 /**
 * Registers a new user.
 * @param registerRequest - Register request, containing email and password pair
 * @returns Succesful message
 */
 export const register = async (registerRequest: AuthRequest): Promise<string> => {
  if (shouldUseMockData) {
    return authMock.register.json() as Promise<string>;
  }

  const response = await sendRegisterRequest(registerRequest);
  return response.data;
 }

/**
 * Refreshes the access token using the provided refresh token
 * Ensures only one request is made at a time by caching the promise
 * @param refreshToken - The refresh token
 * @returns New access and refresh tokens
 */
export const refreshToken = async (refreshToken: string): Promise<AuthResponse> => {
  if (shouldUseMockData) {
    return authMock.refreshToken.json() as Promise<AuthResponse>; 
  }

  if (refreshTokenPromise) {
    return (await refreshTokenPromise).data;
  }

  refreshTokenPromise = sendRefreshTokenRequest(refreshToken);

  try {
    const response = await refreshTokenPromise;
    saveTokens(response.data);
    return response.data;
  } finally {
    refreshTokenPromise = null;
  }
};

/**
 * Sends an HTTP request in order to authenticate the user
 * @param loginRequest - The user login request
 * @returns Axios response containing tokens and expiry
 */
const sendLoginRequest = ({email, password}: AuthRequest): Promise<AxiosResponse<AuthResponse>> => {
  const url = `${apiBaseUrl}/${authApi}/login`;
  const config: AxiosRequestConfig & { withoutAccessToken: boolean } = {
    withoutAccessToken: true
  };

  return axiosClient.post<AuthResponse>(url, { email, password }, config);
};

/**
 * Sends an HTTP request in order to register a new user
 * @param registerRequest - The user registration request
 * @returns Ok with confirmation string
 */
const sendRegisterRequest = ({email, password}: AuthRequest): Promise<AxiosResponse<string>> => {
  const url = `${apiBaseUrl}/${authApi}/register`;
  const config: AxiosRequestConfig & { withoutAccessToken: boolean } = {
    withoutAccessToken: true
  };

  return axiosClient.post<string>(url, { email, password }, config);
};

/**
 * Sends an HTTP request in order to refresh the access token
 * @param refreshToken - The refresh token
 * @returns Axios response containing the new tokens
 */
const sendRefreshTokenRequest = (refreshToken: string): Promise<AxiosResponse<AuthResponse>> => {
  const url = `${apiBaseUrl}/${authApi}/refresh-token`;
  const config: AxiosRequestConfig & { withoutAccessToken: boolean } = {
    withoutAccessToken: true
  };

  return axiosClient.post<AuthResponse>(url, { refresh_token: refreshToken }, config);
};