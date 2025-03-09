import { AxiosError, AxiosResponse } from 'axios';

import { AxiosBaseQuery, AxiosQuery, MockData } from './baseQuery.types';
import { axiosClient } from './client';

import { apiBaseUrl, shouldUseMockData } from '@/config';

/**
 * Axios-based API query function with error handling and mock support.
 * @param baseUrl - API base URL (defaults to `apiBaseUrl`).
 */
export const axiosBaseQuery =
  ({ baseUrl }: AxiosBaseQuery = { baseUrl: apiBaseUrl }) =>
  async <T>({ url, method, data, params, mockData, headers = {} }: AxiosQuery<T>) => {
    if (shouldUseMockData && mockData) {
      return handleMockData<T>(mockData);
    }

    try {
      const response: AxiosResponse<T> = await axiosClient({
        url: `${baseUrl}${url}`,
        method,
        data,
        params,
        headers
      });

      return { data: response.data };
    } catch (axiosError) {
      return handleAxiosError(axiosError);
    }
  };

/**
 * Handles API errors from Axios.
 * @param axiosError - The error thrown by Axios.
 * @returns An error response object.
 */
const handleAxiosError = (axiosError: unknown) => {
  const err = axiosError as AxiosError;
  return {
    error: {
      status: err.response?.status || 500,
      data: err.response?.data || err.message || 'Unknown error'
    }
  };
};

/**
 * Handles mock data response.
 * @param mockData - The mock data object.
 * @returns A simulated API response.
 */
const handleMockData = async <T>(mockData: MockData<T>) => {
  const status = mockData.status ?? 200;
  if (status >= 300) {
    return { error: { status } };
  }

  return { data: (await mockData.json()) as T };
};
