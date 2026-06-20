import React, { useState } from 'react';
import { Badge } from '@/shared/components/ui/badge';
import { Button } from '@/shared/components/ui/button';
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle
} from '@/shared/components/ui/card';
import { Input } from '@/shared/components/ui/input';
import { Label } from '@/shared/components/ui/label';

import { useCurrentEstablishment } from '@/features/products/presentation/hooks/use-queries';
import { useCredentials } from '../hooks/use-queries';
import { useCreateCredential, useUpdateCredential } from '../hooks/use-mutations';
import type { IntegrationProvider, Credential } from '../../domain/entities';

const providerLabel: Record<IntegrationProvider, string> = {
  PaymentGateway: 'Gateway de Pagamento',
  Whatsapp: 'WhatsApp'
};

function CredentialCard({
  credential,
  establishmentId
}: {
  credential: Credential;
  establishmentId: string;
}) {
  const [editing, setEditing] = useState(false);
  const [secretKey, setSecretKey] = useState('');
  const { mutate: updateCredential, isPending } = useUpdateCredential();

  const handleSave = () => {
    updateCredential(
      {
        establishmentId,
        secretKey,
        provider: credential.provider
      },
      { onSuccess: () => setEditing(false) }
    );
  };

  return (
    <Card>
      <CardHeader className="pb-2">
        <div className="flex items-center justify-between">
          <CardTitle className="text-base">
            {providerLabel[credential.provider]}
          </CardTitle>
          <Badge variant="default">Configurado</Badge>
        </div>
        <CardDescription>
          Chave: {'•'.repeat(12)}
        </CardDescription>
      </CardHeader>
      <CardContent>
        {editing ? (
          <div className="space-y-2">
            <Label htmlFor={`key-${credential.id}`}>Nova chave secreta</Label>
            <Input
              id={`key-${credential.id}`}
              type="password"
              value={secretKey}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSecretKey(e.target.value)}
              placeholder="Nova chave secreta"
            />
            <div className="flex gap-2">
              <Button
                size="sm"
                onClick={handleSave}
                disabled={isPending || !secretKey}
              >
                {isPending ? 'Salvando...' : 'Salvar'}
              </Button>
              <Button
                size="sm"
                variant="outline"
                onClick={() => setEditing(false)}
              >
                Cancelar
              </Button>
            </div>
          </div>
        ) : (
          <Button size="sm" variant="outline" onClick={() => setEditing(true)}>
            Atualizar chave
          </Button>
        )}
      </CardContent>
    </Card>
  );
}

function AddCredentialForm({
  establishmentId,
  existingProviders
}: {
  establishmentId: string;
  existingProviders: IntegrationProvider[];
}) {
  const [provider, setProvider] = useState<IntegrationProvider>('PaymentGateway');
  const [secretKey, setSecretKey] = useState('');
  const [open, setOpen] = useState(false);
  const { mutate: createCredential, isPending } = useCreateCredential();

  const available = (
    ['PaymentGateway', 'Whatsapp'] as IntegrationProvider[]
  ).filter((p) => !existingProviders.includes(p));

  if (available.length === 0) return null;

  const handleAdd = () => {
    createCredential(
      { establishmentId, secretKey, provider },
      {
        onSuccess: () => {
          setOpen(false);
          setSecretKey('');
        }
      }
    );
  };

  if (!open) {
    return (
      <Button variant="outline" onClick={() => setOpen(true)}>
        + Adicionar integração
      </Button>
    );
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-base">Nova integração</CardTitle>
      </CardHeader>
      <CardContent className="space-y-3">
        <div className="space-y-1">
          <Label>Provedor</Label>
          <select
            className="w-full border rounded px-3 py-2 text-sm bg-background"
            value={provider}
            onChange={(e) => setProvider(e.target.value as IntegrationProvider)}
          >
            {available.map((p) => (
              <option key={p} value={p}>
                {providerLabel[p]}
              </option>
            ))}
          </select>
        </div>
        <div className="space-y-1">
          <Label>Chave secreta</Label>
          <Input
            type="password"
            value={secretKey}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSecretKey(e.target.value)}
            placeholder="Chave secreta do provedor"
          />
        </div>
        <div className="flex gap-2">
          <Button
            size="sm"
            onClick={handleAdd}
            disabled={isPending || !secretKey}
          >
            {isPending ? 'Adicionando...' : 'Adicionar'}
          </Button>
          <Button
            size="sm"
            variant="outline"
            onClick={() => setOpen(false)}
          >
            Cancelar
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}

export function IntegrationsPage() {
  const { data: establishment, isLoading: loadingEstablishment } =
    useCurrentEstablishment();
  const { data: credentials, isLoading: loadingCredentials } = useCredentials(
    establishment?.id
  );

  const isLoading = loadingEstablishment || loadingCredentials;

  return (
    <main className="p-6 space-y-6">
      <h1 className="text-2xl font-bold">Integrações</h1>

      {isLoading && (
        <p className="text-muted-foreground">Carregando integrações...</p>
      )}

      {!loadingEstablishment && !establishment && (
        <p className="text-muted-foreground">
          Nenhum estabelecimento encontrado para sua conta.
        </p>
      )}

      {!isLoading && establishment && (
        <div className="space-y-4 max-w-xl">
          {credentials?.length === 0 && (
            <p className="text-sm text-muted-foreground">
              Nenhuma integração configurada ainda.
            </p>
          )}

          {credentials?.map((credential) => (
            <CredentialCard
              key={credential.id}
              credential={credential}
              establishmentId={establishment.id}
            />
          ))}

          <AddCredentialForm
            establishmentId={establishment.id}
            existingProviders={
              credentials?.map((c) => c.provider) ?? []
            }
          />
        </div>
      )}
    </main>
  );
}
