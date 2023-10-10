import ReactCardFlip from 'react-card-flip';
import { Flashcard } from './Flashcard';
import { useEffect, useState } from 'react';
import { Color } from '../api/dtos';

interface FlippableFlashcardProps {
  front: string;
  back: string;
  color: Color;
}

export const FlippableFlashcard = (props: FlippableFlashcardProps) => {
  const [isFlipped, setIsFlipped] = useState<boolean>(false);

  const flip = () => setIsFlipped((prev) => !prev);

  useEffect(() => {
    setIsFlipped(false);
  }, [props]);

  return (
    <ReactCardFlip isFlipped={isFlipped} flipDirection='vertical' flipSpeedBackToFront={0.3} flipSpeedFrontToBack={0.3}>
      <Flashcard onClick={flip} text={props.front} color={props.color} />
      <Flashcard onClick={flip} text={props.back} color={props.color} />
    </ReactCardFlip>
  );
};
