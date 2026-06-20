import { Copy } from 'lucide-react';
import { toast } from 'sonner';

import { cn } from '@/shared/utils';

import { Button } from '../../ui/button';

export type CopyButtonProps = {
  successText: string;
  errorText: string;
  copy: string;
  className?: string;
};
export const CopyButton = ({
  successText,
  errorText,
  copy,
  className
}: CopyButtonProps) => {
  const handleCopyCpf = (e: React.MouseEvent) => {
    e.stopPropagation();

    if (copy) {
      navigator.clipboard.writeText(copy);
      toast.success(successText);
    } else {
      toast.warning(errorText);
    }
  };

  return (
    <Button
      className={cn('p-0 h-fit w-fit', className)}
      type="button"
      size={'icon-sm'}
      variant={'ghost'}
      onClick={handleCopyCpf}
    >
      {copy.toUpperCase() !== 'NÃO INFORMADO' && <Copy className="size-3" />}
    </Button>
  );
};
