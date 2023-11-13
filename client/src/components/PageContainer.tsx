import { Container, ContainerProps } from '@mantine/core';

interface PageContainerProps {
  children: React.ReactNode;
}

export const PageContainer = ({ children, ...rest }: PageContainerProps & ContainerProps) => {
  return (
    <Container size='lg' h='100%' {...rest}>
      {children}
    </Container>
  );
};
