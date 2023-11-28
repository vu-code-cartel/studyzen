import { useTranslation } from 'react-i18next';
import {
  Button,
  Stack,
  TextInput,
  Text,
  Container,
  Title,
  Anchor,
  PasswordInput,
  Paper,
  Divider,
  useMantineTheme,
} from '@mantine/core';
import { useForm } from '@mantine/form';
import { useLogin } from '../hooks/api/useAccountsApi';
import { useButtonVariant } from '../hooks/useButtonVariant';
import { useNavigate } from 'react-router-dom';
import { LoginRequest } from '../api/requests';
import { useAppStore } from '../hooks/useAppStore';

export const SignInPage = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const buttonVariant = useButtonVariant();
  const colorScheme = useAppStore((state) => state.colorScheme);
  const theme = useMantineTheme();

  const validateEmail = (value: any) => (value ? null : t('Authentication.Field.Email.Error.Required'));
  const validatePassword = (value: any) => (value ? null : t('Authentication.Field.Password.Error.Required'));

  const form = useForm<LoginRequest>({
    initialValues: {
      email: '',
      password: '',
    },
    validate: {
      email: validateEmail,
      password: validatePassword,
    },
  });

  const loginSuccessCallback = () => {
    navigate('/');
  };

  const { mutate: login, isLoading } = useLogin(loginSuccessCallback);

  const onLogin = async (values: LoginRequest) => {
    login(values);
  };

  const handleSignUpClick = () => navigate('/register');

  return (
    <Container size={420} my={40}>
      <Title order={1} ta='center'>
        {t('Authentication.Title.SignIn')}
      </Title>

      <Paper withBorder p='md' mt='lg'>
        <Stack>
          <form onSubmit={form.onSubmit(onLogin)} autoComplete='off'>
            <Stack>
              <TextInput label={t('Authentication.Field.Email.Label')} {...form.getInputProps('email')} />
              <PasswordInput label={t('Authentication.Field.Password.Label')} {...form.getInputProps('password')} />
              <Button type='submit' variant={buttonVariant} loading={isLoading} fullWidth mt='sm'>
                {isLoading ? t('Authentication.Button.SigningIn') : t('Authentication.Button.SignIn')}
              </Button>
            </Stack>
          </form>

          <Divider />

          <Text ta='center'>
            {t('Authentication.Prompt.NewToStudyZen')}{' '}
            <Anchor onClick={handleSignUpClick} style={{ fontWeight: 500 }}>
              {t('Authentication.Action.SignUp')}
            </Anchor>
          </Text>
        </Stack>
      </Paper>
    </Container>
  );
};
