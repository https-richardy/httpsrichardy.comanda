import axios from 'axios';

import { env } from '@/configs/env';

export const httpClient = axios.create({
  baseURL: env.VITE_GATEWAY_API,
  paramsSerializer: {
    indexes: null
  }
});

export const setupHttpClientInterceptors = (
  getToken: () => string | null,
  onUnauthorized: () => void
) => {
  httpClient.interceptors.request.use(
    (config) => {
      const token = getToken();

      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }

      return config;
    },
    (error) => Promise.reject(error)
  );

  httpClient.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response?.status === 401) {
        onUnauthorized();
      }

      return Promise.reject(error);
    }
  );
};
