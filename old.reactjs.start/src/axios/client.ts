import axios from 'axios';

import {
  accessTokenInterceptor
} from './interceptors';

export const axiosClient = axios.create({
  headers: {
    'Content-Type': 'application/json'
  }
});

axiosClient.interceptors.request.use(accessTokenInterceptor);