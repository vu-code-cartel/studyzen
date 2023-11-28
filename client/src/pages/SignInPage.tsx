import { useTranslation } from 'react-i18next';
import { Button, Stack, TextInput, Text, Container, Title, Anchor, PasswordInput } from '@mantine/core';
import { useForm } from '@mantine/form';
import { useLogin } from '../hooks/api/useAccountsApi';
import { useButtonVariant } from '../hooks/useButtonVariant';
import { useNavigate } from 'react-router-dom';
import { PageContainer } from '../components/PageContainer';
import { LoginRequest } from '../api/requests';


export const SignInPage = () => {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const buttonVariant = useButtonVariant();

    const validateEmail = (value: any) => value ? null : t('Authentication.Field.Email.Error.Required');
    const validatePassword = (value: any) => value ? null : t('Authentication.Field.Password.Error.Required');

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
        <PageContainer style={{ display: 'flex', flexDirection: 'column', justifyContent: 'flex-start', height: '100vh', paddingTop: '10vh' }}>
            <Container size="md">
                <Title order={1} style={{ textAlign: 'center' }}>
                    {t('Authentication.Title.SignIn')}
                </Title>

                <form onSubmit={form.onSubmit(onLogin)} autoComplete='off' style={{ marginTop: 20 }}>
                    <Stack>
                        <TextInput
                            placeholder={t('Authentication.Field.Email.Label')}
                            {...form.getInputProps('email')}
                        />
                        <PasswordInput
                            placeholder={t('Authentication.Field.Password.Label')}
                            {...form.getInputProps('password')}
                        />
                        <Button
                            type='submit'
                            variant={buttonVariant}
                            disabled={isLoading}
                            fullWidth
                        >
                            {isLoading ? t('Authentication.Button.SigningIn') : t('Authentication.Button.SignIn')}
                        </Button>
                    </Stack>
                </form>

                <Text style={{ textAlign: 'center' }} mt="md">
                    {t('Authentication.Prompt.NewToStudyZen')}{' '}
                    <Anchor onClick={handleSignUpClick} style={{ fontWeight: 500 }}>
                        {t('Authentication.Action.SignUp')}
                    </Anchor>
                </Text>
            </Container>
        </PageContainer>
    );
};