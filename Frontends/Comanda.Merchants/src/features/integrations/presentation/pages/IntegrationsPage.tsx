import { useGetIntegrations } from '../hooks/use-queries';

export function IntegrationsPage() {
  const { data, isLoading } = useGetIntegrations();

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">Integrações</h1>

      {isLoading && <p>Carregando integrações...</p>}

      {!isLoading && (
        <div>
          {data && data.length > 0 ? (
            <ul>
              {data.map((it) => (
                <li key={it.id} className="py-2 border-b">
                  <div className="font-medium">{it.name}</div>
                  <div className="text-sm text-muted-foreground">{it.enabled ? 'Ativa' : 'Desativada'}</div>
                </li>
              ))}
            </ul>
          ) : (
            <div className="text-muted-foreground">Nenhuma integração encontrada.</div>
          )}
        </div>
      )}
    </main>
  );
}

export default IntegrationsPage;
