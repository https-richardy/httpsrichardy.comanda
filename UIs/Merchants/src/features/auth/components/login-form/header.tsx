export function LoginHeader() {
  return (
    <div className="flex flex-col items-center gap-2">
      <h1 className="text-xl font-bold">OS PEDIDOS TE ESPERAM</h1>

      <div className="text-center text-sm">
        Não possui uma conta?{' '}
        <a href="/" className="underline underline-offset-4">
          Entre para o beta gratuito
        </a>
      </div>
    </div>
  );
}
