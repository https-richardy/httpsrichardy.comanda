import { NavLink } from 'react-router';
import { Button } from '../../../../../shared/components/ui/button';
import { apps } from './apps-list';

export const AppsNavigation = () => {
  return (
    <ul className="flex gap-2 bg-background p-1 rounded-full shadow-sm">
      {apps.map((app) => (
        <li key={app.name}>
          <NavLink
            to={app.href}
            className={({ isActive }) =>
              `group flex items-center h-full cursor-pointer p-1 rounded-full bg-background transition-all duration-500 ease-in-out border border-transparent ${
                isActive
                  ? 'bg-sidebar text-red-500 hover:bg-sidebar'
                  : 'hover:bg-sidebar hover:text-red-500'
              }`
            }
          >
            {({ isActive }) => (
              <>
                <Button
                  variant="ghost"
                  size="icon"
                  className="size-8 rounded-full shrink-0"
                >
                  {app.icon && <app.icon className="size-4" />}
                </Button>

                <div
                  className={`overflow-hidden transition-all duration-500 ease-in-out flex flex-col justify-center ${
                    isActive
                      ? 'max-w-[200px] opacity-100'
                      : 'max-w-0 opacity-0 group-hover:max-w-[200px] group-hover:opacity-100'
                  }`}
                >
                  <h3 className="font-semibold text-sm whitespace-nowrap pr-3 pl-1">
                    {app.name}
                  </h3>
                </div>
              </>
            )}
          </NavLink>
        </li>
      ))}
    </ul>
  );
};
