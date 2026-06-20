import { useEffect, useState } from 'react';
import { Outlet } from 'react-router';
import { ArrowUp } from 'lucide-react';

import { Button } from '@/shared/components/ui/button';
import { SidebarInset, SidebarProvider } from '@/shared/components/ui/sidebar';

import { HeaderLayout } from './components/header';
import { SystemSidebar } from './components/sidebar';

export const SystemLayout = () => {
  const [showBackToTop, setShowBackToTop] = useState<boolean>(false);

  useEffect(() => {
    const handleWindowScroll = () => {
      setShowBackToTop(window.scrollY > 150);
    };

    window.addEventListener('scroll', handleWindowScroll);
    return () => window.removeEventListener('scroll', handleWindowScroll);
  }, []);

  const scrollToTop = () => {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  };

  return (
    <div className="flex size-full flex-col relative">
      <main className="size-full">
        <SidebarProvider
          style={
            {
              '--sidebar-width': 'calc(var(--spacing) * 64)',
              '--sidebar-height': '100%',
              '--header-height': '0px'
            } as React.CSSProperties
          }
          defaultOpen={false}
        >
          <SystemSidebar variant="inset" />
          <SidebarInset>
            <div className="flex flex-1 flex-col">
              <div className="@container/main flex flex-1 flex-col gap-2">
                <HeaderLayout />
                <Outlet />
              </div>
            </div>
          </SidebarInset>
        </SidebarProvider>
      </main>

      {showBackToTop && (
        <div className="fixed bottom-6 left-1/2 -translate-x-1/2 z-50">
          <Button
            onClick={scrollToTop}
            className="rounded-full shadow-xl bg-primary text-primary-foreground hover:bg-primary/90 flex gap-2 w-max px-6 transition-all animate-in slide-in-from-bottom-5 fade-in-0 duration-300"
          >
            <ArrowUp className="w-4 h-4" />
            Voltar ao topo
          </Button>
        </div>
      )}
    </div>
  );
};
