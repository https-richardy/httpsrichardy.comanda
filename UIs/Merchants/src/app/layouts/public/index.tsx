import { Outlet } from 'react-router';

export function PublicLayout() {
  return (
    <div className="min-h-screen flex items-center justify-center ">
      <main className="w-full max-w-md bg-white ring-1 ring-zinc-200 dark:bg-zinc-700 dark:ring-1 dark:ring-zinc-600 p-6 rounded-lg shadow">
        <Outlet />
      </main>
    </div>
  );
}
