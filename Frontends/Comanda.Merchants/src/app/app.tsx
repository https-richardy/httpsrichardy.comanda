import { UseNotification } from '@/features/notification';

import { Toaster } from '../shared/components/ui/sonner';

import { AppProviders } from './providers';
import { AppRouters } from './routers';

export default function App() {
  return (
    <AppProviders>
      <AppRouters />
      <Toaster />
      <UseNotification />
    </AppProviders>
  );
}
