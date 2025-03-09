import { shouldUseMockData } from '@/config';
import LOCAL_STORAGE from '@/constants/localStorage';

/**
 * Retrieves the expiry timestamp of a JWT token.
 * If mock data is enabled, returns an expiry time of 1 hour from now.
 * @param token - The JWT access token.
 * @returns Expiry timestamp in milliseconds.
 */
export const getTokenExpiry = (token: string): number => {
  if (shouldUseMockData) {
    return Date.now() + 3600000; // 1 hour from now
  }

  const payload = parseJwt(token);
  return payload?.exp ? payload.exp * 1000 : Date.now();
};

/**
 * Saves access and refresh tokens to localStorage with expiry.
 * If the access token is invalid, it removes existing tokens.
 * @param tokens - Object containing `access_token` and `refresh_token`.
 */
export const saveTokens = ({ access_token, refresh_token }: { access_token: string; refresh_token: string }) => {
  if (!access_token || access_token.length > 4000) {
    removeTokens();
    return;
  }

  const expiry = getTokenExpiry(access_token);
  if (!expiry) {
    removeTokens();
    return;
  }

  localStorage.setItem(LOCAL_STORAGE.ACCESS_TOKEN, access_token);
  localStorage.setItem(LOCAL_STORAGE.ACCESS_TOKEN_VALID_UNTIL, expiry.toString());

  if (refresh_token) {
    localStorage.setItem(LOCAL_STORAGE.REFRESH_TOKEN, refresh_token);
  } else {
    localStorage.removeItem(LOCAL_STORAGE.REFRESH_TOKEN);
  }
};

/**
 * Removes stored access and refresh tokens from localStorage.
 */
export const removeTokens = () => {
  [
    LOCAL_STORAGE.ACCESS_TOKEN,
    LOCAL_STORAGE.ACCESS_TOKEN_VALID_UNTIL,
    LOCAL_STORAGE.REFRESH_TOKEN
  ].forEach(key => localStorage.removeItem(key));
};

/**
 * Retrieves a stored token from localStorage.
 * @param key - The LOCAL_STORAGE key.
 * @returns The token string or null if not found.
 */
export const getStoredToken = (key: string): string | null => localStorage.getItem(key);

/**
 * Checks if the access token is still valid.
 * @param expiryTimestamp - The expiration timestamp.
 * @returns True if the token is still valid, otherwise false.
 */
export const isTokenValid = (expiryTimestamp: number | null): boolean => {
  if (!expiryTimestamp) return false;
  return expiryTimestamp - 5000 >= Date.now(); // 5s buffer for slow networks
};


/**
 * Parses a JWT token safely and returns its payload.
 * @param token - The JWT token.
 * @returns Decoded payload object or `null` if invalid.
 */
const parseJwt = (token: string): Record<string, any> | null => {
  if (!token) return null;

  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(function (c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    );
  
    return JSON.parse(jsonPayload);
  } catch (error) {
    console.error('Invalid JWT token:', error);
    return null;
  }
};
