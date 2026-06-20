import { Star } from 'lucide-react';
import type { ComponentProps } from 'react';

import { Button } from '@/shared/components/ui/button';
import { cn } from '@/shared/utils';

import { useFilePickerContext } from '../../hooks';
import { getFileIcon } from '../../utils'; // Importamos getFileIcon em vez de getFilePreview para ter mais controle
import { FilePickerRemoveFileButton } from '../buttons/remove-file';

type FilePickerContentProps = ComponentProps<'div'> & {
  showFavButton?: boolean;
  showRemoveFileButton?: boolean;
};

export function FilePickerContent({
  className,
  showFavButton,
  showRemoveFileButton,
  ...props
}: FilePickerContentProps) {
  const [{ files }, { setPrimaryFile }] = useFilePickerContext();

  return (
    files.length > 0 && (
      <div
        className={cn('grid grid-cols-2 gap-4 md:grid-cols-3', className)}
        {...props}
      >
        {files.map((file) => {
          const isImage = file.type.startsWith('image/');

          return (
            <div
              key={file.id}
              className="relative flex flex-col rounded-md border bg-background overflow-hidden"
            >
              {isImage ? (
                <img
                  src={file.url}
                  alt={file.name}
                  className="aspect-square size-full rounded-t-[inherit] object-cover"
                />
              ) : (
                <div className="flex w-full items-center gap-2 bg-accent p-2 pr-8">
                  <div className="flex flex-1 items-center justify-center">
                    {getFileIcon({ file: (file.file || file) as File })}
                  </div>
                  <p
                    className="w-full truncate px-2 text-center text-[12px] font-medium text-foreground"
                    title={file.name}
                  >
                    {file.name}
                  </p>
                </div>
              )}
              <div className="absolute top-1 right-1 flex gap-1">
                {showFavButton && file.isPrimary && (
                  <div className="bg-black/50 size-6 rounded-full border-2 border-background flex items-center justify-center">
                    <Star className="size-3.5 text-yellow-400 fill-yellow-400" />
                  </div>
                )}
                {!file.isPrimary && showFavButton && (
                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    className="size-6 bg-black/50 rounded-full text-white hover:text-yellow-400"
                    onClick={(e) => {
                      e.preventDefault();
                      setPrimaryFile(file.id);
                    }}
                    aria-label="Definir como principal"
                  >
                    <Star className="size-3.5" />
                  </Button>
                )}

                {showRemoveFileButton && (
                  <FilePickerRemoveFileButton id={file.id} />
                )}
              </div>
            </div>
          );
        })}
      </div>
    )
  );
}
