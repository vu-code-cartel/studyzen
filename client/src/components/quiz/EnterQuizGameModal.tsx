import { Button, Modal, Stack, TextInput } from '@mantine/core';
import { useForm } from '@mantine/form';
import { useTranslation } from 'react-i18next';
import { JoinQuizGameDto } from '../../api/requests';
import { useGetQuizGame } from '../../hooks/api/quizGamesApi';
import { useNavigate } from 'react-router-dom';
import { getQuizGameRoomRoute } from '../../common/app-routes';
import { useButtonVariant } from '../../hooks/useButtonVariant';

interface EnterQuizGameModalProps {
  isOpen: boolean;
  close: () => void;
}

export const EnterQuizGameModal = (props: EnterQuizGameModalProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const buttonVariant = useButtonVariant();
  const form = useForm<JoinQuizGameDto>({
    initialValues: {
      gamePin: '',
      username: '',
      connectionId: '',
    },
    validate: {
      gamePin: (value) => (value ? undefined : t('QuizGame.Field.GamePin.Error.Required')),
    },
  });
  const { refetch: fetchQuizGame } = useGetQuizGame(form.values.gamePin, false);

  const onJoinGame = async () => {
    const result = await fetchQuizGame();

    if (result.data?.pin) {
      navigate(getQuizGameRoomRoute(result.data.pin));
    }
  };

  return (
    <Modal opened={props.isOpen} onClose={props.close} title={t('QuizGame.Title.Join')}>
      <form onSubmit={form.onSubmit(onJoinGame)} autoComplete='off'>
        <Stack>
          <TextInput placeholder={t('QuizGame.Field.GamePin.Label')} {...form.getInputProps('gamePin')} />
          <Button variant={buttonVariant} type='submit'>
            {t('QuizGame.Button.Enter')}
          </Button>
        </Stack>
      </form>
    </Modal>
  );
};
