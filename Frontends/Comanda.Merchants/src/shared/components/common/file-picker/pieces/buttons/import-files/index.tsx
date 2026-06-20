import { UploadIcon } from 'lucide-react';
import type { ComponentProps } from 'react';

import { Button } from '@/shared/components/ui/button';

import { useFilePickerContext } from '../../../hooks';

type TFilePickerButton = ComponentProps<typeof Button> & {
  label: string;
  className?: string;
};

export function FilePickerButton({
  label,
  className,
  ...props
}: TFilePickerButton) {
  const [, { openFileDialog }] = useFilePickerContext();

  return (
    <Button
      type="button"
      variant="outline"
      className={`mt-4 ${className}`}
      onClick={openFileDialog}
      {...props}
    >
      <UploadIcon className="-ms-1 opacity-60" aria-hidden="true" />
      {label}
    </Button>
  );
}
