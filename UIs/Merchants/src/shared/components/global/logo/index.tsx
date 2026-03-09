import { cn } from '@/shared/utils';
import logo from "../../../../assets/logo.png";

interface AppLogoProps {
  className?: string;
}

export function AppLogo({ className }: AppLogoProps) {
  return (
    <img
      src={logo}
      width={420}
      height={400}
      className={cn('w-full', className)}
      alt="Logo da App"
    />
  );
}
