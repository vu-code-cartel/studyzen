import { Button, Modal, Stack, TextInput } from '@mantine/core';
import { JoinQuizGameDto } from '../../api/requests';
import { useTranslation } from 'react-i18next';
import { useDisclosure } from '@mantine/hooks';
import { useForm } from '@mantine/form';
import { useQuizJoinGame } from '../../hooks/api/quizGamesApi';
import { useButtonVariant } from '../../hooks/useButtonVariant';

interface JoinQuizGameModalProps {
  gamePin: string | null;
  connectionId: string | null;
  onPlayerJoin: () => void;
}

export const JoinQuizGameModal = (props: JoinQuizGameModalProps) => {
  const [opened, { close }] = useDisclosure(true);
  const { t } = useTranslation();
  const buttonVariant = useButtonVariant();
  const form = useForm<JoinQuizGameDto>({
    initialValues: {
      gamePin: '',
      username: '',
      connectionId: '',
    },
    validate: {
      username: (value) => (value ? undefined : t('QuizGame.Field.Username.Error.Required')),
    },
  });
  const joinGame = useQuizJoinGame();

  const onJoinGameClick = async (values: JoinQuizGameDto) => {
    if (!props.gamePin || !props.connectionId) {
      return;
    }

    const dto: JoinQuizGameDto = {
      gamePin: props.gamePin,
      username: values.username,
      connectionId: props.connectionId,
    };

    try {
      await joinGame.mutateAsync(dto);
      props.onPlayerJoin();
      close();
    } catch {
      // ignored
    }
  };

  return (
    <Modal opened={opened} onClose={close} withCloseButton={false} closeOnClickOutside={false} closeOnEscape={false}>
      <form onSubmit={form.onSubmit(onJoinGameClick)}>
        <Stack>
          <TextInput
            placeholder={t('QuizGame.Field.Username.Label')}
            data-autofocus
            {...form.getInputProps('username')}
          />
          <Button
            variant={buttonVariant}
            type='submit'
            loading={joinGame.isLoading || !props.connectionId || !props.gamePin}
          >
            {t('QuizGame.Button.Join')}
          </Button>
        </Stack>
      </form>
    </Modal>
  );
};
