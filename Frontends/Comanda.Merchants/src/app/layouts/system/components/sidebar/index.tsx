import { AppLogo } from '@/shared/components/common/logo';
import {
  Sidebar,
  SidebarContent,
  SidebarHeader,
  SidebarRail
} from '@/shared/components/ui/sidebar';

import { NavItems } from './nav-items';
import { Navlinks } from './nav-links';

export function SystemSidebar({
  ...props
}: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar collapsible="icon" variant="floating" {...props}>
      <SidebarHeader>
        <div className="w-full overflow-hidden">
          <AppLogo className="pl-1.5 w-[150px]" />
        </div>
      </SidebarHeader>
      <SidebarContent>
        <NavItems items={Navlinks} />
      </SidebarContent>
      <SidebarRail />
    </Sidebar>
  );
}
