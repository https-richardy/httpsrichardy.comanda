import { useRef, useState } from 'react';

import { cn } from '@/shared/utils';
import { Badge } from '@/shared/components/ui/badge';
import { Button } from '@/shared/components/ui/button';
import { Input } from '@/shared/components/ui/input';
import { Textarea } from '@/shared/components/ui/textarea';
import { Card, CardContent, CardFooter } from '@/shared/components/ui/card';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
  DialogDescription,
} from '@/shared/components/ui/dialog';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/shared/components/ui/alert-dialog';
import { Label } from '@/shared/components/ui/label';
import { ScrollArea } from '@/shared/components/ui/scroll-area';
import { Pencil, Trash2, Plus, Search, ImageIcon, Upload, Package } from 'lucide-react';

import type { Product } from '../../domain/entities';
import { MOCK_PRODUCTS } from '../../data/mock';

interface ProductFormData {
  title: string;
  description: string;
  price: string;
  image: string;
}

interface ProductFormDialogProps {
  mode: 'create' | 'edit';
  open: boolean;
  onClose: () => void;
  onSave: (data: ProductFormData) => void;
  initial?: ProductFormData;
}

function ProductFormDialog({
  mode,
  open,
  onClose,
  onSave,
  initial,
}: ProductFormDialogProps) {
  const [title, setTitle] = useState(initial?.title ?? '');
  const [description, setDescription] = useState(initial?.description ?? '');
  const [price, setPrice] = useState(initial?.price ?? '');
  const [image, setImage] = useState(initial?.image ?? '');
  const fileInputRef = useRef<HTMLInputElement>(null);

  // Sync form fields when dialog opens/reopens with new initial values
  const handleOpenChange = (o: boolean) => {
    if (!o) {
      onClose();
    }
  };

  // Reset to initial when modal opens
  const handleOpen = () => {
    setTitle(initial?.title ?? '');
    setDescription(initial?.description ?? '');
    setPrice(initial?.price ?? '');
    setImage(initial?.image ?? '');
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    const reader = new FileReader();
    reader.onload = (ev) => {
      if (typeof ev.target?.result === 'string') {
        setImage(ev.target.result);
      }
    };
    reader.readAsDataURL(file);
  };

  const handleSave = () => {
    onSave({ title, description, price, image });
    onClose();
  };

  const isValid = title.trim().length > 0 && parseFloat(price) > 0;

  return (
    <Dialog open={open} onOpenChange={handleOpenChange}>
      <DialogContent
        className="sm:max-w-[500px]"
        onOpenAutoFocus={() => handleOpen()}
      >
        <DialogHeader>
          <DialogTitle>{mode === 'create' ? 'Novo Produto' : 'Editar Produto'}</DialogTitle>
          <DialogDescription>
            {mode === 'create'
              ? 'Preencha as informações do novo produto.'
              : 'Altere as informações do produto.'}
          </DialogDescription>
        </DialogHeader>

        <ScrollArea className="max-h-[60vh]">
          <div className="space-y-4 p-1 pr-3">
            {/* Image upload */}
            <div className="space-y-1.5">
              <Label>Imagem</Label>
              <input
                ref={fileInputRef}
                type="file"
                accept="image/*"
                className="hidden"
                onChange={handleFileChange}
              />
              <div
                className={cn(
                  'rounded-lg border-2 border-dashed border-border hover:border-primary/50 cursor-pointer transition-colors',
                  'aspect-video flex flex-col items-center justify-center gap-2 text-muted-foreground overflow-hidden',
                )}
                onClick={() => fileInputRef.current?.click()}
              >
                {image ? (
                  <img
                    src={image}
                    alt="Preview"
                    className="w-full h-full object-cover rounded-lg"
                  />
                ) : (
                  <>
                    <Upload className="size-8 opacity-40" />
                    <span className="text-sm">Clique para selecionar uma imagem</span>
                  </>
                )}
              </div>
            </div>

            {/* Title */}
            <div className="space-y-1.5">
              <Label htmlFor="product-title">Nome</Label>
              <Input
                id="product-title"
                placeholder="Ex: X-Burguer"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
              />
            </div>

            {/* Description */}
            <div className="space-y-1.5">
              <Label htmlFor="product-description">Descrição</Label>
              <Textarea
                id="product-description"
                placeholder="Descreva o produto..."
                rows={3}
                value={description}
                onChange={(e) => setDescription(e.target.value)}
              />
            </div>

            {/* Price */}
            <div className="space-y-1.5">
              <Label htmlFor="product-price">Preço (R$)</Label>
              <Input
                id="product-price"
                type="number"
                min={0}
                step={0.01}
                placeholder="0.00"
                value={price}
                onChange={(e) => setPrice(e.target.value)}
              />
            </div>
          </div>
        </ScrollArea>

        <DialogFooter>
          <Button variant="outline" onClick={onClose}>
            Cancelar
          </Button>
          <Button onClick={handleSave} disabled={!isValid}>
            {mode === 'create' ? 'Criar Produto' : 'Salvar Alterações'}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

export function ProductsPage() {
  const [products, setProducts] = useState<Product[]>(MOCK_PRODUCTS);
  const [search, setSearch] = useState('');

  // Create dialog
  const [createOpen, setCreateOpen] = useState(false);

  // Edit dialog
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);

  // Delete confirmation
  const [deletingProduct, setDeletingProduct] = useState<Product | null>(null);

  const filtered = products.filter((p) => {
    const q = search.toLowerCase();
    return p.title.toLowerCase().includes(q) || p.description.toLowerCase().includes(q);
  });

  const handleCreate = (data: ProductFormData) => {
    const newProduct: Product = {
      id: crypto.randomUUID(),
      title: data.title.trim(),
      description: data.description.trim(),
      price: parseFloat(data.price),
      image: data.image,
    };
    setProducts((prev) => [newProduct, ...prev]);
  };

  const handleEdit = (data: ProductFormData) => {
    if (!editingProduct) return;
    setProducts((prev) =>
      prev.map((p) =>
        p.id === editingProduct.id
          ? {
              ...p,
              title: data.title.trim(),
              description: data.description.trim(),
              price: parseFloat(data.price),
              image: data.image,
            }
          : p,
      ),
    );
  };

  const handleDelete = () => {
    if (!deletingProduct) return;
    setProducts((prev) => prev.filter((p) => p.id !== deletingProduct.id));
    setDeletingProduct(null);
  };

  const getInitials = (title: string) =>
    title
      .split(' ')
      .slice(0, 2)
      .map((w) => w[0])
      .join('')
      .toUpperCase();

  return (
    <main className="p-4 lg:p-6 space-y-6">
      {/* Header */}
      <div className="flex items-start justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Produtos</h1>
          <p className="text-sm text-muted-foreground mt-0.5">
            Gerencie o cardápio do seu estabelecimento
          </p>
        </div>
        <Button onClick={() => setCreateOpen(true)}>
          <Plus className="size-4" />
          Novo Produto
        </Button>
      </div>

      {/* Search */}
      <div className="relative max-w-sm">
        <Search className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground pointer-events-none" />
        <Input
          className="pl-9"
          placeholder="Buscar produtos..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>

      {/* Count badge */}
      <div className="flex items-center gap-2">
        <Badge variant="secondary">
          {filtered.length} {filtered.length === 1 ? 'produto' : 'produtos'}
        </Badge>
      </div>

      {/* Grid */}
      {filtered.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-20 text-muted-foreground gap-3">
          <Package className="size-10 opacity-30" />
          <p className="text-sm">Nenhum produto encontrado.</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
          {filtered.map((product) => (
            <Card key={product.id} className="overflow-hidden flex flex-col py-0 gap-0">
              {/* Image */}
              <div className="aspect-square bg-muted flex items-center justify-center overflow-hidden">
                {product.image ? (
                  <img
                    src={product.image}
                    alt={product.title}
                    className="w-full h-full object-cover"
                  />
                ) : (
                  <div className="flex flex-col items-center gap-2 text-muted-foreground/50">
                    <ImageIcon className="size-8" />
                    <span className="text-2xl font-bold tracking-tight">
                      {getInitials(product.title)}
                    </span>
                  </div>
                )}
              </div>

              <CardContent className="p-3 flex-1 space-y-1.5">
                <p className="font-semibold text-sm leading-snug line-clamp-1">{product.title}</p>
                <p className="text-xs text-muted-foreground line-clamp-2 leading-relaxed">
                  {product.description}
                </p>
                <p className="text-sm font-bold text-emerald-600">
                  R$ {product.price.toFixed(2)}
                </p>
              </CardContent>

              <CardFooter className="p-3 pt-0 flex gap-2">
                <Button
                  variant="outline"
                  size="sm"
                  className="flex-1 gap-1"
                  onClick={() => setEditingProduct(product)}
                >
                  <Pencil className="size-3.5" />
                  Editar
                </Button>
                <Button
                  variant="outline"
                  size="sm"
                  className="text-destructive hover:text-destructive hover:bg-destructive/10"
                  onClick={() => setDeletingProduct(product)}
                >
                  <Trash2 className="size-3.5" />
                </Button>
              </CardFooter>
            </Card>
          ))}
        </div>
      )}

      {/* Create dialog */}
      <ProductFormDialog
        mode="create"
        open={createOpen}
        onClose={() => setCreateOpen(false)}
        onSave={handleCreate}
      />

      {/* Edit dialog */}
      {editingProduct && (
        <ProductFormDialog
          mode="edit"
          open={!!editingProduct}
          onClose={() => setEditingProduct(null)}
          onSave={handleEdit}
          initial={{
            title: editingProduct.title,
            description: editingProduct.description,
            price: String(editingProduct.price),
            image: editingProduct.image,
          }}
        />
      )}

      {/* Delete confirmation */}
      <AlertDialog
        open={!!deletingProduct}
        onOpenChange={(open) => !open && setDeletingProduct(null)}
      >
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Tem certeza?</AlertDialogTitle>
            <AlertDialogDescription>
              O produto <strong>{deletingProduct?.title}</strong> será removido permanentemente.
              Esta ação não pode ser desfeita.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancelar</AlertDialogCancel>
            <AlertDialogAction
              className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
              onClick={handleDelete}
            >
              Remover
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </main>
  );
}
