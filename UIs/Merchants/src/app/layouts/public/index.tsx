import { Outlet } from 'react-router';

export function PublicLayout() {
  return (
    <div className="min-h-screen flex items-center justify-center ">
      <main className="w-full max-w-md p-6">
        <Outlet />
      </main>
    </div>
  );
}
