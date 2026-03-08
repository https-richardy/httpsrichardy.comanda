import React from 'react';
import { cn } from '@/shared/utils';

interface InputWithSuffixProps
  extends React.InputHTMLAttributes<HTMLInputElement> {
  suffix: string;
}

export const InputWithSuffix = React.forwardRef<
  HTMLInputElement,
  InputWithSuffixProps
>(({ className, suffix, ...props }, ref) => {
  return (
    <div
      className={cn(
        'flex h-10 w-full items-center rounded-md border border-input bg-background ring-offset-background',
        'focus-within:ring-2 focus-within:ring-ring focus-within:ring-offset-2',
        className
      )}
    >
      <input
        {...props}
        ref={ref}
        className="w-full bg-transparent px-3 py-2 text-sm placeholder:text-muted-foreground focus:outline-none disabled:cursor-not-allowed disabled:opacity-50"
      />

      <div className="flex h-full items-center border-l border-input bg-muted/50 px-3 text-sm text-muted-foreground rounded-r-md">
        {suffix}
      </div>
    </div>
  );
});

InputWithSuffix.displayName = 'InputWithSuffix';
