export function SuspenseLoading() {
  return (
    <div className="inset-0 z-[999] grid h-screen w-screen place-items-center bg-background bg-opacity-60 backdrop-blur-sm transition-opacity duration-300">
      <div className="relative mx-auto w-full">
        <div className="flex items-center justify-center h-screen gap-2">
          <span className="h-4 w-1 bg-destructive rounded-full animate-ping [animation-duration:1s]" />
          <span className="h-4 w-1 bg-destructive rounded-full animate-ping [animation-duration:1s] [animation-delay:0.15s]" />
          <span className="h-4 w-1 bg-destructive rounded-full animate-ping [animation-duration:1s] [animation-delay:0.3s]" />
          <span className="h-4 w-1 bg-destructive rounded-full animate-ping [animation-duration:1s] [animation-delay:0.45s]" />
        </div>
      </div>
    </div>
  );
}
