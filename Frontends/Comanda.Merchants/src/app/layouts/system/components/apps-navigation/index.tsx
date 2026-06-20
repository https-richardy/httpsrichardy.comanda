import { useTheme } from 'next-themes';

import { useAuthStore } from '@/features/auth';

import { Button } from '../../../../../shared/components/ui/button';

import { apps } from './apps-list';

export const AppsNavigation = () => {
  const { token } = useAuthStore();
  const { resolvedTheme } = useTheme();

  return (
    <ul className="flex gap-2 bg-background p-1 rounded-full shadow-sm">
      {apps.map((app) => (
        <li key={app.name}>
          <a
            href={
              !app.active
                ? `${app.href}/authorize/sso?token=${token}${resolvedTheme && `&theme=${resolvedTheme}`}`
                : '#'
            }
            rel={!app.active ? 'noopener noreferrer' : undefined}
            className={`
              group flex items-center h-full cursor-pointer p-1 rounded-full bg-background 
              transition-all duration-500 ease-in-out border border-transparent
              ${
                app.active
                  ? 'bg-sidebar text-red-500 hover:bg-sidebar'
                  : 'hover:bg-sidebar hover:text-red-500'
              }
            `}
          >
            <Button
              variant="ghost"
              size="icon"
              className="size-8 rounded-full shrink-0"
            >
              {app.icon && <app.icon className="size-4" />}
            </Button>

            <div
              className={`
                overflow-hidden transition-all duration-500 ease-in-out
                flex flex-col justify-center
                ${
                  app.active
                    ? 'max-w-[200px] opacity-100'
                    : 'max-w-0 opacity-0 group-hover:max-w-[200px] group-hover:opacity-100'
                } // Se inativo, anima de 0 a 200px
              `}
            >
              <h3 className="font-semibold text-sm whitespace-nowrap pr-3 pl-1">
                {app.name}
              </h3>
            </div>
          </a>
        </li>
      ))}
    </ul>
  );
};
