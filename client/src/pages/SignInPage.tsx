import { useTranslation } from 'react-i18next';
import { Button, Stack, TextInput, Text, Container, Title, Anchor } from '@mantine/core';
import { useForm } from '@mantine/form';
import { useLogin } from '../hooks/api/useAccountsApi';
import { useButtonVariant } from '../hooks/useButtonVariant';
import { useNavigate } from 'react-router-dom';
import { PageContainer } from '../components/PageContainer';
import { LoginRequest } from '../api/requests';
import { useAppStore } from '../hooks/useAppStore';


export const SignInPage = () => {
    const setIsLoggedIn = useAppStore((state) => state.setIsLoggedIn);
    const { t } = useTranslation();
    const navigate = useNavigate();
    const buttonVariant = useButtonVariant();

    // Validation logic
    const validateEmail = (value: any) => value ? null : t('Authentication.Field.Email.Error.Required');
    const validatePassword = (value: any) => value ? null : t('Authentication.Field.Password.Error.Required');

    const form = useForm({
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
        setIsLoggedIn(true);
        navigate('/');
    };

    const { mutate: login, isLoading } = useLogin(loginSuccessCallback);

    const onLogin = async (values: LoginRequest) => {
        login(values);
    };

    const handleSignUpClick = () => navigate('/register');

    return (
        <PageContainer>
            <Container size="xs" mt={30}>
                <Title order={1} style={{ textAlign: 'center' }}>
                    {t('Authentication.Title.SignIn')}
                </Title>

                <form onSubmit={form.onSubmit(onLogin)} autoComplete='off' style={{ marginTop: 20 }}>
                    <Stack>
                        <TextInput
                            placeholder={t('Authentication.Field.Email.Label')}
                            {...form.getInputProps('email')}
                        />
                        <TextInput
                            placeholder={t('Authentication.Field.Password.Label')}
                            type='password'
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