import { Center, Loader } from '@mantine/core';

export const CenteredLoader = () => {
  return (
    <Center h='100vh'>
      <Loader size='lg' mb='20vh' />
    </Center>
  );
};
