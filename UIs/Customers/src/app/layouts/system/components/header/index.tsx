
import { HeaderMenu } from '../header-menu';

export const HeaderLayout = () => {
  return (
    <header className="bg-transparent  backdrop-blur-xs sticky top-0 flex  space-y-3 shrink-0 z-10 pb-4 md:pb-0 justify-between items-center py-3 px-4 pr-6">
      <a href="/" className="pt-1.5 w-28 md:w-48">

      </a>
      <HeaderMenu />
    </header>
  );
};
