import { Center, Loader, Stack, Text } from '@mantine/core';
import { useTranslation } from 'react-i18next';

interface CenteredLoaderProps {
  text?: string;
}

export const CenteredLoader = (props: CenteredLoaderProps) => {
  const { t } = useTranslation();

  return (
    <Center h='100%' pb='20vh'>
      <Stack align='center'>
        <Text fw={600}>{props.text ?? t('Generic.Loading')}</Text>
        <Loader size='lg' type='dots' />
      </Stack>
    </Center>
  );
};
