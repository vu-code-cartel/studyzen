import { Group } from '@mantine/core';

interface PageHeaderProps {
  children: React.ReactNode;
}

export const PageHeader = (props: PageHeaderProps) => {
  return (
    <Group h={36} justify='space-between' mb='sm'>
      {props.children}
    </Group>
  );
};
