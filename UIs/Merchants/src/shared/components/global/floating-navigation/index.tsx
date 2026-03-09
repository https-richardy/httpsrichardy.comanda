import { Button } from '../../ui/button';
import type{ ElementType } from 'react';

interface NavigationItem {
  name: string;
  href: string;
  icon: ElementType;
  active?: boolean;
}

interface FloatingNavigationProps {
  items: NavigationItem[];
}
export const FloatingNavigation = ({ items }: FloatingNavigationProps) => {
  
  return (
    <ul className="flex gap-2 bg-background p-1 rounded-full shadow-sm">
      {items.map((item) => (
        <li key={item.name}>
          <a
            href={
              !item.active ? `${item.href}` : '#'
            }
            target={!item.active ? '_blank' : undefined}
            rel={!item.active ? 'noopener noreferrer' : undefined}
            className={`
              group flex items-center h-full cursor-pointer p-1 rounded-full bg-background 
              transition-all duration-500 ease-in-out border border-transparent
              ${
                item.active
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
              {item.icon && <item.icon className="size-4" />}
            </Button>

            <div
              className={`
                overflow-hidden transition-all duration-500 ease-in-out
                flex flex-col justify-center
                ${
                  item.active
                    ? 'max-w-[200px] opacity-100'
                    : 'max-w-0 opacity-0 group-hover:max-w-[200px] group-hover:opacity-100'
                } // Se inativo, anima de 0 a 200px
              `}
            >
              <h3 className="font-semibold text-sm whitespace-nowrap pr-3 pl-1">
                {item.name}
              </h3>
            </div>
          </a>
        </li>
      ))}
    </ul>
  );
};
