import { Container } from '@mantine/core';

interface PageContainerProps {
  children: React.ReactNode;
}

export const PageContainer = (props: PageContainerProps) => {
  return <Container size='lg'>{props.children}</Container>;
};
