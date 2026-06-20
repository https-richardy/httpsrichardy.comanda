import { LogOut } from 'lucide-react';

import { useLogout } from '@/features/auth';

import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger
} from '@/shared/components/ui/alert-dialog';
import { DropdownMenuItem } from '@/shared/components/ui/dropdown-menu';

export const ButtonLogout = () => {
  const { mutateAsync: logout } = useLogout();
  return (
    <AlertDialog>
      <AlertDialogTrigger asChild>
        <DropdownMenuItem
          onSelect={(e) => e.preventDefault()}
          className="cursor-pointer text-destructive focus:text-destructive"
        >
          <LogOut className="size-4 mr-2" />
          Encerrar sessão
        </DropdownMenuItem>
      </AlertDialogTrigger>

      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Deseja encerrar a sessão atual?</AlertDialogTitle>
          <AlertDialogDescription>
            Após esta ação, você será deslogado do sistema e redirecionado para
            se autenticar novamente.
          </AlertDialogDescription>
        </AlertDialogHeader>

        <AlertDialogFooter>
          <AlertDialogCancel>Cancelar</AlertDialogCancel>

          <AlertDialogAction className="bg-red-700" onClick={() => logout()}>
            Encerrar sessão
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
};
