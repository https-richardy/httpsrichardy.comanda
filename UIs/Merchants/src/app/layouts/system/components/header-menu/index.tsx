import { ToggleTheme } from '@/app/theme/toggle-theme';
import { UserMenu } from '../user-menu';

export function HeaderMenu() {
  return (
    <div className="flex items-center space-x-3">
      <ToggleTheme />
      <UserMenu />
    </div>
  );
}
