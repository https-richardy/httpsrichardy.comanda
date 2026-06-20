import { Mail, UsersIcon } from 'lucide-react';

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
          className="relative flex items-center h-full w-fit p-0 hover:bg-transparent"
        >
          <Avatar className="size-8">
            <AvatarFallback className="bg-border dark:bg-border">
              {getInitials(user.name)}
            </AvatarFallback>
          </Avatar>
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
