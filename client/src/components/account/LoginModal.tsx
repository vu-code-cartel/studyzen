import { useTranslation } from 'react-i18next';
import { Button, Modal, Stack, TextInput } from '@mantine/core';
import { useForm } from '@mantine/form';
import { useLogin } from '../../hooks/api/useAccountsApi';
import { useButtonVariant } from '../../hooks/useButtonVariant';

interface LoginModalProps {
    isOpen: boolean;
    close: () => void;
}

export const LoginModal = ({ isOpen, close }: LoginModalProps) => {
    const { t } = useTranslation();
    const { login, isLoading } = useLogin();
    const buttonVariant = useButtonVariant();
    const form = useForm({
        initialValues: {
            email: '',
            password: '',
        },
        validate: {
            email: (value) => (value ? null : t('Authentication.Field.Username.Error.Required')),
            password: (value) => (value ? null : t('Authentication.Field.Password.Error.Required')),
        },
    });

    const onLogin = (values: any) => {
        login(values);
    };

    return (
        <Modal opened={isOpen} onClose={close} title={t('Authentication.Title.Login')}>
            <form onSubmit={form.onSubmit(onLogin)} autoComplete='off' style={{ position: 'relative' }}>
                <Stack>
                    <TextInput
                        placeholder={t('Authentication.Field.Username.Label')}
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
                        {isLoading ? t('Authentication.Button.LoggingIn') : t('Authentication.Button.Login')}
                    </Button>
                </Stack>
            </form>
        </Modal>
    );
}; 