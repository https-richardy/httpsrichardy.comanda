//import { getCurrentTenant } from "@/libs/axios";
import { NavLink, type NavLinkProps } from 'react-router';

export function RestrictedLink({ children, ...props }: NavLinkProps) {
  return <NavLink {...props}>{children}</NavLink>;
}
