import {
  TextInput,
  Text,
  Button,
  Stack,
  Select,
  Container,
  Title,
  PasswordInput,
  Anchor,
  Paper,
  Divider,
} from '@mantine/core';
import { useForm } from '@mantine/form';
import { useTranslation } from 'react-i18next';
import { useRegister } from '../hooks/api/useAccountsApi';
import { useButtonVariant } from '../hooks/useButtonVariant';
import { useNavigate } from 'react-router-dom';
import { RegisterRequest } from '../api/requests';
import { Role } from '../api/dtos';

export const SignUpPage = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const buttonVariant = useButtonVariant();

  const onRegisterSuccess = () => {
    navigate('/login');
  };

  const handleSignInClick = () => navigate('/login');

  const { mutate: register, isLoading } = useRegister(onRegisterSuccess);

  const form = useForm<RegisterRequest>({
    initialValues: {
      username: '',
      email: '',
      password: '',
      firstName: '',
      lastName: '',
      role: Role.Student,
    },
    validate: {
      username: (value) => (value ? null : t('Authentication.Field.Username.Error.Required')),
      email: (value) => (value ? null : t('Authentication.Field.Email.Error.Required')),
      password: (value) => (value ? null : t('Authentication.Field.Password.Error.Required')),
      firstName: (value) => (value ? null : t('Authentication.Field.FirstName.Error.Required')),
      lastName: (value) => (value ? null : t('Authentication.Field.LastName.Error.Required')),
    },
  });

  const onRegister = (values: RegisterRequest) => {
    register(values);
  };

  return (
    <Container size={420} my={40}>
      <Title order={1} style={{ textAlign: 'center' }}>
        {t('Authentication.Title.SignUp')}
      </Title>

      <Paper withBorder p='md' mt='lg'>
        <Stack>
          <form onSubmit={form.onSubmit(onRegister)} autoComplete='off'>
            <Stack>
              <TextInput label={t('Authentication.Field.Username.Label')} {...form.getInputProps('username')} />
              <TextInput label={t('Authentication.Field.Email.Label')} {...form.getInputProps('email')} />
              <PasswordInput label={t('Authentication.Field.Password.Label')} {...form.getInputProps('password')} />
              <TextInput label={t('Authentication.Field.FirstName.Label')} {...form.getInputProps('firstName')} />
              <TextInput label={t('Authentication.Field.LastName.Label')} {...form.getInputProps('lastName')} />
              <Select
                label={t('Authentication.Field.Role.Label')}
                {...form.getInputProps('role')}
                data={[
                  { value: Role.Student, label: t('Authentication.Role.Student') },
                  { value: Role.Lecturer, label: t('Authentication.Role.Lecturer') },
                ]}
              />
              <Button type='submit' variant={buttonVariant} loading={isLoading} mt='md'>
                {isLoading ? t('Authentication.Button.SigningUp') : t('Authentication.Button.SignUp')}
              </Button>
            </Stack>
          </form>
          <Divider />
          <Text ta='center'>
            {t('Authentication.Prompt.AlreadyHaveAccount')}{' '}
            <Anchor onClick={handleSignInClick} style={{ fontWeight: 500 }}>
              {t('Authentication.Action.SignIn')}
            </Anchor>
          </Text>
        </Stack>
      </Paper>
    </Container>
  );
};
