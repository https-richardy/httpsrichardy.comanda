import axios from 'axios';

import { env } from '@/configs/env';

export const federationClient = axios.create({
  baseURL: env.VITE_FEDERATION_API
});
