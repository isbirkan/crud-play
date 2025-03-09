 
import { AxiosRequestConfig, InternalAxiosRequestConfig as _AxiosRequestConfigTypeFix } from 'axios'

export type AxiosBaseQuery = { baseUrl: string };

export type AxiosQuery<T> = {
  url: string;
  method: AxiosRequestConfig['method'];
  data?: AxiosRequestConfig['data'];
  params?: AxiosRequestConfig['params'];
  mockData?: MockData<T>;
  headers?: AxiosRequestConfig['headers'];
};

export type MockData<T> = {
  ok: boolean;
  status: number;
  json: () => Promise<T>;
}