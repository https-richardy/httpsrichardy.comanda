import { useDropzone } from '@/shared/hooks/use-dropzone';

import { useBoard } from '../../hook/use-board';

interface UseBoardColumnOptions {
  id: string;
  onDrop: (itemId: string, columnId: string) => void;
  isDropDisabled?: boolean;
}

export function useBoardColumn({
  id,
  onDrop,
  isDropDisabled = false
}: UseBoardColumnOptions) {
  const { getDropHandler, isColumnActive } = useBoard();
  const { isDragging, dropzoneProps } = useDropzone({
    onDrop: getDropHandler({ id, onDrop, isDropDisabled }),
    disabled: isDropDisabled
  });

  const isActiveDropTarget = isColumnActive(isDragging, isDropDisabled);

  return { dropzoneProps, isActiveDropTarget };
}
