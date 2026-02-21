import * as React from "react";

const MOBILE_BREAKPOINT = 768;
const EVENT_NAME = "change";

export function useIsMobile()
{
    const [isMobile, setIsMobile] = React.useState<boolean | undefined>(undefined);

    React.useEffect(() =>
    {
        const mediaQuery = window.matchMedia(`(max-width: ${MOBILE_BREAKPOINT - 1}px)`);
        const handleMediaQueryChange = () =>
        {
            setIsMobile(window.innerWidth < MOBILE_BREAKPOINT);
        };

        mediaQuery.addEventListener(EVENT_NAME, handleMediaQueryChange);
        setIsMobile(window.innerWidth < MOBILE_BREAKPOINT);

        return () =>
        {
            mediaQuery.removeEventListener(EVENT_NAME, handleMediaQueryChange);
        };
    }, []);

  return Boolean(isMobile);
}
