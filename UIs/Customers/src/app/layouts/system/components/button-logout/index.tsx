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
import { LogOut } from 'lucide-react';
import { useAuthStore } from '@/features/auth/store';
import { DropdownMenuItem } from '@/shared/components/ui/dropdown-menu';

export const ButtonLogout = () => {
  const logout = useAuthStore((state) => state.logout);

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

          <AlertDialogAction className="bg-red-700" onClick={logout}>
            Encerrar sessão
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
};
