import { z } from 'zod';

const envSchema = z.object({
  VITE_FEDERATION_API: z.string().url(),
  VITE_GATEWAY_API: z.string().url()
});

export const env = envSchema.parse({
  VITE_FEDERATION_API: import.meta.env.VITE_FEDERATION_API,
  VITE_GATEWAY_API: import.meta.env.VITE_GATEWAY_API
});
