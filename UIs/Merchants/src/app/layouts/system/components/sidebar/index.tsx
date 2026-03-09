import {
  Sidebar,
  SidebarContent,
  SidebarRail
} from '@/shared/components/ui/sidebar';

import { NavItems } from './nav-items';
import { Navlinks } from './nav-links';

export function SystemSidebar({
  ...props
}: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar collapsible="icon" variant="floating" {...props}>
      <SidebarContent>
        <NavItems items={Navlinks} />
      </SidebarContent>
      <SidebarRail />
    </Sidebar>
  );
}
