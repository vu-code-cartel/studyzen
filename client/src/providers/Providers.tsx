import ChakraProvider from './ChakraProvider';
import RouterProvider from './RouterProvider';

const Providers = () => {
  return (
    <ChakraProvider>
      <RouterProvider />
    </ChakraProvider>
  );
};

export default Providers;
