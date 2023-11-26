import { Modal, TextInput, Button, Stack, Select } from '@mantine/core';
import { useForm } from '@mantine/form';
import { useTranslation } from 'react-i18next';
import { useRegister } from '../../hooks/api/useAccountsApi';
import { useButtonVariant } from '../../hooks/useButtonVariant';


interface RegisterModalProps {
    isOpen: boolean;
    close: () => void;
    onRegisterSuccess: () => void;
}

export const RegisterModal = ({ isOpen, close, onRegisterSuccess }: RegisterModalProps) => {
    const { t } = useTranslation();
    const { mutate: register, isLoading } = useRegister(onRegisterSuccess);
    const buttonVariant = useButtonVariant();

    const form = useForm({
        initialValues: {
            username: '',
            email: '',
            password: '',
            firstName: '',
            lastName: '',
            role: 'student',
        },
    });

    const onRegister = (values: any) => {
        register(values);
    };

    const handleClose = () => {
        close();
        form.reset();
    };

    return (
        <Modal opened={isOpen} onClose={handleClose} title={t('Authentication.Title.SignUp')}>
            <form onSubmit={form.onSubmit(onRegister)} autoComplete='off'>
                <Stack>
                    <TextInput
                        placeholder={t('Authentication.Field.Username.Label')}
                        {...form.getInputProps('username')}
                    />
                    <TextInput
                        placeholder={t('Authentication.Field.Email.Label')}
                        {...form.getInputProps('email')}
                    />
                    <TextInput
                        placeholder={t('Authentication.Field.Password.Label')}
                        type='password'
                        {...form.getInputProps('password')}
                    />
                    <TextInput
                        placeholder={t('Authentication.Field.FirstName.Label')}
                        {...form.getInputProps('firstName')}
                    />
                    <TextInput
                        placeholder={t('Authentication.Field.LastName.Label')}
                        {...form.getInputProps('lastName')}
                    />
                    <Select
                        label={t('Authentication.Field.Role.Label')}
                        {...form.getInputProps('role')}
                        data={[
                            { value: 'student', label: t('Authentication.Role.Student') },
                            { value: 'lecturer', label: t('Authentication.Role.Lecturer') },
                        ]}
                    />
                    <Button
                        type='submit'
                        variant={buttonVariant}
                        disabled={isLoading}
                    >
                        {isLoading ? t('Authentication.Button.RegisteringIn') : t('Authentication.Button.Register')}
                    </Button>
                </Stack>
            </form>
        </Modal>
    );
};