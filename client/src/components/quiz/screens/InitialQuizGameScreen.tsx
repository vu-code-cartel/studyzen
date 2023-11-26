import { useTranslation } from 'react-i18next';
import { QuizGameDto, QuizPlayerDto } from '../../../api/dtos';
import { CenteredLoader } from '../../CenteredLoader';
import { JoinQuizGameModal } from '../JoinQuizGameModal';
import { QuizWaitingRoom } from '../QuizWaitingRoom';

interface InitialQuizGameScreenProps {
  game: QuizGameDto | null;
  connectionId: string | null;
  players: QuizPlayerDto[];
  onPlayerJoin: () => void;
}

export const InitialQuizGameScreen = (props: InitialQuizGameScreenProps) => {
  const { t } = useTranslation();

  return (
    <>
      <JoinQuizGameModal
        gamePin={props.game?.pin ?? null}
        connectionId={props.connectionId}
        onPlayerJoin={props.onPlayerJoin}
      />
      {!props.game || !props.connectionId ? (
        <CenteredLoader text={t('QuizGame.Title.Connecting')} />
      ) : (
        <QuizWaitingRoom game={props.game} players={props.players} />
      )}
    </>
  );
};
