import { AppLogo } from '@/shared/components/global/logo';

export function LoginHeader() {
  return (
    <div className="flex flex-col items-center gap-2">
      <div className="flex justify-center items-center">
        <AppLogo className="w-64" />
      </div>

      <h1 className="text-xl font-bold">Faça login na sua conta.</h1>

      <div className="text-center text-sm">
        Não possui uma conta?{' '}
        <a href="/" className="underline underline-offset-4">
          Contate o adminstrador
        </a>
      </div>
    </div>
  );
}
