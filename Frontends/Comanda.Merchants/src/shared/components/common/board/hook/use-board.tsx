import {
  createContext,
  type DragEvent,
  type ReactNode,
  useCallback,
  useContext,
  useState
} from 'react';

import type { BoardContextValue } from '../types';

const BoardContext = createContext<BoardContextValue | null>(null);

export function BoardProvider({ children }: { children: ReactNode }) {
  const [draggingId, setDraggingId] = useState<string | null>(null);

  return (
    <BoardContext.Provider value={{ draggingId, setDraggingId }}>
      {children}
    </BoardContext.Provider>
  );
}

export function useBoard() {
  const ctx = useContext(BoardContext);

  if (!ctx) {
    throw new Error('useBoard must be used inside a <Board> component.');
  }

  const { draggingId, setDraggingId } = ctx;

  const getItemProps = useCallback(
    (id: string) => ({
      onDragStart: (e: DragEvent<HTMLElement>) => {
        e.dataTransfer.setData('text/plain', id);
        e.dataTransfer.effectAllowed = 'move';
        setDraggingId(id);
      },
      onDragEnd: () => {
        setDraggingId(null);
      }
    }),
    [setDraggingId]
  );

  const getDropHandler = useCallback(
    ({
      id,
      onDrop,
      isDropDisabled = false
    }: {
      id: string;
      onDrop: (itemId: string, columnId: string) => void;
      isDropDisabled?: boolean;
    }) =>
      (e: DragEvent<HTMLElement>) => {
        const itemId = e.dataTransfer.getData('text/plain');
        if (itemId && !isDropDisabled) {
          onDrop(itemId, id);
        }
      },
    []
  );

  const isColumnActive = useCallback(
    (isDragging: boolean, isDropDisabled: boolean) =>
      isDragging && draggingId !== null && !isDropDisabled,
    [draggingId]
  );

  return { draggingId, getItemProps, getDropHandler, isColumnActive };
}
