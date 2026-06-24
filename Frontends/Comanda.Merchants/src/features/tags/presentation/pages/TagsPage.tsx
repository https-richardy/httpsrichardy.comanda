import { useState } from 'react';
import { QRCodeSVG } from 'qrcode.react';
import { Tag as TagIcon, Plus, Info, X, Printer } from 'lucide-react';

import { cn } from '@/shared/utils';
import { Button } from '@/shared/components/ui/button';
import { Card, CardContent } from '@/shared/components/ui/card';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogDescription,
  DialogFooter,
} from '@/shared/components/ui/dialog';
import { Input } from '@/shared/components/ui/input';
import { Label } from '@/shared/components/ui/label';

import type { Tag } from '../../domain/entities';
import { MOCK_TAGS } from '../../data/mock';

export function TagsPage() {
  const [tags, setTags] = useState<Tag[]>(MOCK_TAGS);
  const [selectedTag, setSelectedTag] = useState<Tag | null>(null);
  const [createOpen, setCreateOpen] = useState(false);
  const [newNumber, setNewNumber] = useState('');
  const [numberError, setNumberError] = useState('');

  const handleCreateOpen = () => {
    setNewNumber('');
    setNumberError('');
    setCreateOpen(true);
  };

  const handleCreate = () => {
    const num = parseInt(newNumber, 10);
    if (isNaN(num) || num < 1 || num > 999) {
      setNumberError('Informe um número entre 1 e 999.');
      return;
    }
    if (tags.some((t) => t.number === num)) {
      setNumberError('Já existe uma comanda com esse número.');
      return;
    }
    const newTag: Tag = { id: crypto.randomUUID(), number: num };
    setTags((prev) => [...prev, newTag].sort((a, b) => a.number - b.number));
    setCreateOpen(false);
  };

  const handleDelete = (tagId: string, e: React.MouseEvent) => {
    e.stopPropagation();
    setTags((prev) => prev.filter((t) => t.id !== tagId));
  };

  const handleNumberChange = (val: string) => {
    setNewNumber(val);
    setNumberError('');
  };

  return (
    <main className="p-4 lg:p-6 space-y-6">
      {/* Header */}
      <div className="flex items-start justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Comandas</h1>
          <p className="text-sm text-muted-foreground mt-0.5">
            Gerencie as comandas físicas do seu estabelecimento
          </p>
        </div>
        <Button onClick={handleCreateOpen}>
          <Plus className="size-4" />
          Nova Comanda
        </Button>
      </div>

      {/* Info banner */}
      <div className="flex items-start gap-3 rounded-lg bg-muted/60 border px-4 py-3 text-sm text-muted-foreground">
        <Info className="size-4 mt-0.5 shrink-0 text-blue-500" />
        <p>
          As comandas identificam pedidos de mesa. Cada comanda possui um número único e um QR Code
          que o cliente pode escanear.
        </p>
      </div>

      {/* Grid */}
      {tags.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-20 text-muted-foreground gap-3">
          <TagIcon className="size-10 opacity-30" />
          <p className="text-sm">Nenhuma comanda cadastrada.</p>
        </div>
      ) : (
        <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 gap-3">
          {tags.map((tag) => (
            <Card
              key={tag.id}
              className={cn(
                'relative cursor-pointer transition-all duration-150 hover:shadow-md hover:-translate-y-0.5 py-0 group',
              )}
              onClick={() => setSelectedTag(tag)}
            >
              {/* Delete button */}
              <button
                className={cn(
                  'absolute top-1 right-1 z-10 rounded-full p-1 transition-opacity duration-150',
                  'bg-destructive/10 text-destructive hover:bg-destructive hover:text-destructive-foreground',
                  'opacity-0 group-hover:opacity-100',
                )}
                onClick={(e) => handleDelete(tag.id, e)}
                title="Remover comanda"
              >
                <X className="size-3" />
              </button>

              <CardContent className="p-4 flex flex-col items-center gap-2">
                <span className="font-mono text-3xl font-black leading-none">{tag.number}</span>
                <QRCodeSVG
                  value={`COMANDA:${tag.number}`}
                  size={80}
                  className="rounded-sm"
                />
              </CardContent>
            </Card>
          ))}
        </div>
      )}

      {/* QR detail dialog */}
      <Dialog open={!!selectedTag} onOpenChange={(open) => !open && setSelectedTag(null)}>
        <DialogContent className="sm:max-w-[340px]">
          <DialogHeader>
            <DialogTitle className="font-mono text-2xl">
              Comanda #{selectedTag?.number}
            </DialogTitle>
            <DialogDescription>
              QR Code da comanda para impressão ou exibição ao cliente.
            </DialogDescription>
          </DialogHeader>

          <div className="flex justify-center py-4">
            {selectedTag && (
              <QRCodeSVG
                value={`COMANDA:${selectedTag.number}`}
                size={200}
                className="rounded-lg"
              />
            )}
          </div>

          <DialogFooter className="flex gap-2">
            <Button variant="outline" className="flex-1" onClick={() => window.print()}>
              <Printer className="size-4" />
              Imprimir
            </Button>
            <Button className="flex-1" onClick={() => setSelectedTag(null)}>
              Fechar
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Create dialog */}
      <Dialog open={createOpen} onOpenChange={(open) => !open && setCreateOpen(false)}>
        <DialogContent className="sm:max-w-[340px]">
          <DialogHeader>
            <DialogTitle>Nova Comanda</DialogTitle>
            <DialogDescription>
              Informe o número da nova comanda (1–999). Cada número deve ser único.
            </DialogDescription>
          </DialogHeader>

          <div className="space-y-1.5 py-2">
            <Label htmlFor="tag-number">Número da comanda</Label>
            <Input
              id="tag-number"
              type="number"
              min={1}
              max={999}
              placeholder="Ex: 42"
              value={newNumber}
              onChange={(e) => handleNumberChange(e.target.value)}
              onKeyDown={(e) => e.key === 'Enter' && handleCreate()}
            />
            {numberError && <p className="text-xs text-destructive">{numberError}</p>}
          </div>

          <DialogFooter>
            <Button variant="outline" onClick={() => setCreateOpen(false)}>
              Cancelar
            </Button>
            <Button onClick={handleCreate} disabled={!newNumber.trim()}>
              Criar Comanda
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </main>
  );
}
