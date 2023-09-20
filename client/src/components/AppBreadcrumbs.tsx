import { Anchor, Breadcrumbs } from '@mantine/core';
import { Link } from 'react-router-dom';

interface Breadcrumb {
  title: string;
  to: string;
}

interface AppBreadcrumbsProps {
  items: Breadcrumb[];
}

export const AppBreadcrumbs = (props: AppBreadcrumbsProps) => {
  return (
    <Breadcrumbs>
      {props.items.map((item) => (
        <Anchor component={Link} to={item.to} key={item.to} fw={600}>
          {item.title}
        </Anchor>
      ))}
    </Breadcrumbs>
  );
};
