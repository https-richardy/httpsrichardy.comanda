import { z } from 'zod';

export const loginFormSchema = z.object({
  username: z.string().email('O formato do e-mail informado é inválido'),
  password: z.string().min(4, 'A senha deve ter no mínimo 3 caracteres.')
});

export type LoginFormSchema = z.infer<typeof loginFormSchema>;
