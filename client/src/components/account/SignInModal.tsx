import { useTranslation } from 'react-i18next';
import { Button, Modal, Stack, TextInput, Text } from '@mantine/core';
import { useForm } from '@mantine/form';
import { useLogin } from '../../hooks/api/useAccountsApi';
import { useButtonVariant } from '../../hooks/useButtonVariant';

interface SignInModalProps {
    isOpen: boolean;
    close: () => void;
    onLoginSuccess: () => void;
    onSignUp: () => void;
}

export const SigInModal = ({ isOpen, close, onLoginSuccess, onSignUp }: SignInModalProps) => {
    const { t } = useTranslation();
    const { mutate: login, isLoading } = useLogin(onLoginSuccess);
    const buttonVariant = useButtonVariant();
    const form = useForm({
        initialValues: {
            email: '',
            password: '',
        },
        validate: {
            email: (value) => (value ? null : t('Authentication.Field.Email.Error.Required')),
            password: (value) => (value ? null : t('Authentication.Field.Password.Error.Required')),
        },
    });

    const onLogin = (values: any) => {
        login(values);
    };

    const handleSignUpClick = () => {
        close();
        onSignUp();
    };

    const handleClose = () => {
        form.reset();  // Reset the form
        close();
    };

    return (
        <Modal opened={isOpen} onClose={handleClose} title={t('Authentication.Title.SignIn')}>
            <form onSubmit={form.onSubmit(onLogin)} autoComplete='off' style={{ position: 'relative' }}>
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
                    >
                        {isLoading ? t('Authentication.Button.SigningIn') : t('Authentication.Button.SignIn')}
                    </Button>
                </Stack>
            </form>
            <Text>
                New to StudyZen?{' '}
                <Text
                    component="a"
                    onClick={handleSignUpClick}
                    style={{
                        cursor: 'pointer',
                        fontWeight: 'bold',
                        color: '#007bff',
                        textDecoration: 'underline'
                    }}
                >
                    Sign up
                </Text>
            </Text>
        </Modal>
    );
};