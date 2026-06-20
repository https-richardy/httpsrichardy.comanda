import { AlertTriangle, CheckCircle2, Info, XCircle } from 'lucide-react';
import { Toaster as Sonner } from 'sonner';

type ToasterProps = React.ComponentProps<typeof Sonner>;

export function Toaster({ ...props }: ToasterProps) {
  return (
    <Sonner
      className="toaster group"
      closeButton
      toastOptions={{
        classNames: {
          toast:
            'group toast bg-white text-zinc-900 border border-border shadow-lg rounded-2xl p-3 pl-4 pr-12 flex gap-3 items-center w-full relative overflow-hidden',
          content: 'flex-1',
          title: 'text-[13px] font-medium',
          description: 'text-zinc-500 text-sm mt-0.5',
          icon: 'mr-4! w-auto h-auto bg-transparent',
          actionButton:
            'bg-secondary! text-primary! p-0 font-semibold hover:bg-transparent text-[10px]',
          closeButton:
            'absolute right-3 top-1/2 -translate-y-1/2 border-none bg-transparent hover:bg-zinc-100 text-zinc-400 hover:text-zinc-900 opacity-100 flex items-center justify-center size-8 rounded-full transition-colors'
        }
      }}
      icons={{
        success: (
          <div className="bg-green-100/60 p-1.5 rounded-xl text-green-700 shrink-0">
            <CheckCircle2 className="size-5" />
          </div>
        ),
        info: (
          <div className="bg-zinc-100 p-1.5 rounded-xl text-zinc-700 shrink-0">
            <Info className="size-5" />
          </div>
        ),
        warning: (
          <div className="bg-amber-100/60 p-1.5 rounded-xl text-amber-700 shrink-0">
            <AlertTriangle className="size-5" />
          </div>
        ),
        error: (
          <div className="bg-red-100/60 p-1.5 rounded-xl text-red-700 shrink-0">
            <XCircle className="size-5" />
          </div>
        )
      }}
      {...props}
    />
  );
}
