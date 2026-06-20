import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';

import { SubmitingButton } from '@/shared/components/common/submiting-button';
import { Form } from '@/shared/components/ui/form';

import { useLogin } from '../../hooks/use-mutations';

import { CredentialsFields } from './credentials';
import { LoginHeader } from './header';
import { type LoginFormSchema, loginFormSchema } from './schema';

export function LoginForm() {
  const { mutateAsync: login, isPending } = useLogin();

  const form = useForm<LoginFormSchema>({
    resolver: zodResolver(loginFormSchema),
    defaultValues: {
      username: '',
      password: ''
    }
  });

  const onSubmit = (data: LoginFormSchema) => {
    login({
      username: data.username,
      password: data.password
    });
  };

  return (
    <Form {...form}>
      <div className="max-w-sm mx-auto relative z-50 flex flex-col gap-6">
        <form onSubmit={form.handleSubmit(onSubmit)}>
          <div className="flex flex-col gap-6">
            <LoginHeader />
            <div className="flex flex-col gap-6">
              <CredentialsFields form={form} />
              <SubmitingButton
                label="Acessar"
                state={isPending}
                className="bg-red-700 text-white font-bold hover:bg-red-800"
              />
            </div>
          </div>
        </form>
      </div>
    </Form>
  );
}
