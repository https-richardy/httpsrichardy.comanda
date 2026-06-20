import { ToggleTheme } from '@/app/theme/toggle-theme';

import { NotificationsBell } from '@/features/notification';

import { UserMenu } from '../user-menu';

export function HeaderMenu() {
  return (
    <div className="flex items-center gap-2 pr-1">
      <ToggleTheme />
      <NotificationsBell />
      <UserMenu />
    </div>
  );
}
