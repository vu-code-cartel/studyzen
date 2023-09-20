import { NavLink } from '@mantine/core';
import { Link } from 'react-router-dom';

interface AppNavLinkProps {
  label: string;
  to: string;
  isActive?: boolean;
  onClick: () => void;
}

export const AppNavLink = (props: AppNavLinkProps) => {
  return (
    <NavLink
      label={props.label}
      active={props.isActive}
      px='md'
      component={Link}
      to={props.to}
      onClick={props.onClick}
    />
  );
};
