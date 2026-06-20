import type { ReactNode } from 'react';

import { useFilePickerContext } from '../../hooks';

type TFilePickerEmpty = {
  children?: ReactNode;
  className?: string;
};

export function FilePickerEmpty({ children, className }: TFilePickerEmpty) {
  const [{ files }] = useFilePickerContext();
  return (
    files.length < 1 && (
      <div
        className={`flex flex-col items-center justify-center px-4 py-3 text-center ${className}`}
      >
        {children}
      </div>
    )
  );
}
