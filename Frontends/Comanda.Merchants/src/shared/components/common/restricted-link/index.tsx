//import { getCurrentTenant } from "@/libs/axios";
import { NavLink, type NavLinkProps } from 'react-router';

export function RestrictedLink({ children, ...props }: NavLinkProps) {
  const tenant = 'smartconsig'; // Mock tenant value for demonstration

  if (tenant !== 'smartconsig') {
    return null;
  }

  return <NavLink {...props}>{children}</NavLink>;
}
