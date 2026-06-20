import { Mail, User, UsersIcon } from 'lucide-react';

import { useAuthStore } from '@/features/auth';

import { Avatar, AvatarFallback } from '@/shared/components/ui/avatar';
import { Button } from '@/shared/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger
} from '@/shared/components/ui/dropdown-menu';
import { getInitials, mapGroupName } from '@/shared/utils';

import { ButtonLogout } from '../button-logout';

export const UserMenu = () => {
  const user = useAuthStore((state) => state.user);

  if (!user) return null;

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button
          type="button"
          variant="ghost"
          className="relative flex items-center gap-2 h-full w-fit px-1.5 bg-muted-foreground/10"
        >
          <Avatar className="size-8">
            <AvatarFallback className="bg-border dark:bg-border">
              {getInitials(user.name)}
            </AvatarFallback>
          </Avatar>
          <div className="flex items-start gap-1 justify-center flex-col">
            <div className="flex w-full items-center gap-1">
              <User className="size-3" />
              <p className="text-xs font-medium leading-none">{user.name}</p>
            </div>
            {/* Tenant removed for Comanda */}
          </div>
        </Button>
      </DropdownMenuTrigger>

      <DropdownMenuContent className="w-60 mt-2" align="end" forceMount>
        <DropdownMenuLabel className="font-normal">
          <div className="flex flex-col space-y-1">
            <div className="flex w-full items-center gap-1">
              <Mail className="size-4 text-muted-foreground" />
              <p className="text-sm  text-muted-foreground truncate ">
                {user.email}
              </p>
            </div>

            {user.roles.length > 0 && (
              <div className="flex items-center gap-2">
                <UsersIcon className="size-3 text-muted-foreground" />
                <span className="text-sm text-muted-foreground truncate">
                  {user.roles.map(mapGroupName).join(', ')}
                </span>
              </div>
            )}
          </div>
        </DropdownMenuLabel>
        <DropdownMenuSeparator />
        <ButtonLogout />
      </DropdownMenuContent>
    </DropdownMenu>
  );
};
