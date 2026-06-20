import { Bell } from 'lucide-react';

export function NotificationsBell() {
  return (
    <button
      type="button"
      aria-label="Notificações"
      className="p-2 rounded hover:bg-muted-foreground/5"
    >
      <Bell className="size-4" />
    </button>
  );
}

export function UseNotification() {
  // Placeholder for notification integration (sonner, sockets, etc.)
  return null;
}

export default UseNotification;
