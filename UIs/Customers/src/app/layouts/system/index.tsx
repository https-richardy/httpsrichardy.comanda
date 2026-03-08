import { SidebarInset, SidebarProvider } from '@/shared/components/ui/sidebar';
import { HeaderLayout } from './components/header';
import { Outlet } from 'react-router';
import { SystemSidebar } from './components/sidebar';

export const SystemLayout = () => {
  return (
    <div className="flex size-full flex-col relative">
      <HeaderLayout />
      <main>
        <SidebarProvider
          style={
            {
              '--sidebar-width': 'calc(var(--spacing) * 64)',
              '--header-height': 'calc(var(--spacing) * 12)'
            } as React.CSSProperties
          }
        >
          <SystemSidebar variant="inset" />
          <SidebarInset>
            <div className="flex flex-1 flex-col">
              <div className="@container/main flex flex-1 flex-col gap-2">
                <Outlet />
              </div>
            </div>
          </SidebarInset>
        </SidebarProvider>
      </main>
    </div>
  );
};
