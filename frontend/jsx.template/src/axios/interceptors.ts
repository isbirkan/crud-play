import { InternalAxiosRequestConfig, AxiosHeaders } from 'axios';

import * as authApi from '@/api/authorization/authorization';
import LOCAL_STORAGE from '@/constants/localStorage';
import { getStoredToken, isTokenValid, saveTokens } from '@/helpers/tokenHelper';

/**
 * Axios request interceptor to automatically attach the access token.
 * @param config - Axios request config.
 * @returns Updated request config.
 */
export const accessTokenInterceptor = async (
  config: InternalAxiosRequestConfig
): Promise<InternalAxiosRequestConfig> => {
  const withoutAccessToken = (config as { withoutAccessToken?: boolean }).withoutAccessToken ?? false;
  if (withoutAccessToken) return config;

  try {
    let accessToken = getStoredToken(LOCAL_STORAGE.ACCESS_TOKEN);
    const accessTokenValidTill = Number(getStoredToken(LOCAL_STORAGE.ACCESS_TOKEN_VALID_UNTIL));

    if (!accessToken || !isTokenValid(accessTokenValidTill)) {
      const refreshToken = getStoredToken(LOCAL_STORAGE.REFRESH_TOKEN);
      if (refreshToken) {
        const tokenResponse = await authApi.refreshToken(refreshToken);
        saveTokens(tokenResponse);
        accessToken = tokenResponse.access_token;
      }
    }

    // ✅ Fix: Ensure `config.headers` is correctly updated with `AxiosHeaders`
    if (!config.headers) {
      config.headers = new AxiosHeaders();
    }

    config.headers.set('Authorization', accessToken ? `Bearer ${accessToken}` : '');
  } catch (error) {
    console.error('Error in accessTokenInterceptor:', error);
  }

  return config;
};