import { cn } from '@/shared/utils';

interface AppLogoProps {
  className?: string;
}

export function AppLogo({ className }: AppLogoProps) {
  return (
    <img
      src={""}
      width={420}
      height={400}
      className={cn('w-full', className)}
      alt="Logo da App"
    />
  );
}
