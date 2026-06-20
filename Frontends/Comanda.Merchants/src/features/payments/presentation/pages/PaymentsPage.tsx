import { useGetPayments } from '../hooks/use-queries';

export function PaymentsPage() {
  const { data, isLoading } = useGetPayments();

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">Pagamento</h1>

      {isLoading && <p>Carregando pagamentos...</p>}

      {!isLoading && (
        <div>
          {data && data.length > 0 ? (
            <ul>
              {data.map((p) => (
                <li key={p.id} className="py-2 border-b">
                  <div className="font-medium">{p.method}</div>
                  <div className="text-sm text-muted-foreground">{p.status} — R$ {p.amount}</div>
                </li>
              ))}
            </ul>
          ) : (
            <div className="text-muted-foreground">Nenhum pagamento encontrado.</div>
          )}
        </div>
      )}
    </main>
  );
}

export default PaymentsPage;
