import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '@/shared/components/ui/form';

import { Input } from '@/shared/components/ui/input';
import {LockIcon, MailIcon } from 'lucide-react';
import type { UseFormReturn } from 'react-hook-form';
import type { LoginFormSchema } from './schema';

interface LoginFormCredentialsProps {
  form: UseFormReturn<LoginFormSchema>;
}

export function CredentialsFields({
  form
}: Readonly<LoginFormCredentialsProps>) {
  return (
    <>
      <FormField
        control={form.control}
        name="username"
        render={({ field }) => (
          <FormItem>
            <FormLabel>E-mail</FormLabel>
            <FormControl>
              <div className="relative w-full">
                <MailIcon className="absolute left-3 top-1/2 size-4 -translate-y-1/2" />
                <Input
                  type="email"
                  id="current-username"
                  placeholder="exemplo@email.com"
                  className="pl-10"
                  {...field}
                />
              </div>
            </FormControl>
            <FormMessage />
          </FormItem>
        )}
      />

      <FormField
        control={form.control}
        name="password"
        render={({ field }) => (
          <FormItem>
            <FormLabel>Senha</FormLabel>
            <FormControl>
              <div className="relative w-full">
                <LockIcon className="absolute left-3 top-1/2 size-4 -translate-y-1/2" />
                <Input
                  type="password"
                  id="current-password"
                  placeholder="**********"
                  className="pl-10 placeholder:tracking-wider"
                  {...field}
                />
              </div>
            </FormControl>
            <FormMessage />
          </FormItem>
        )}
      />
    </>
  );
}
