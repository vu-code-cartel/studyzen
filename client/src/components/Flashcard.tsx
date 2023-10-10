import { AspectRatio, Card, Center } from '@mantine/core';
import { useAppStore } from '../hooks/useAppStore';
import { Color } from '../api/dtos';
import { useColor } from '../hooks/useColor';

interface FlashcardProps {
  text: string;
  onClick: () => void;
  color: Color;
}

export const Flashcard = (props: FlashcardProps) => {
  const isMobile = useAppStore((state) => state.isMobile);
  const [textColor, backgroundColor, borderColor] = useColor(props.color);

  const CARD_MOBILE_WIDTH = '100%';
  const CARD_DESKTOP_WIDTH = '70%';

  return (
    <Center w='100%'>
      <AspectRatio ratio={16 / 9} w={isMobile ? CARD_MOBILE_WIDTH : CARD_DESKTOP_WIDTH}>
        <Card
          onClick={props.onClick}
          withBorder
          shadow='sm'
          style={{ color: textColor, background: backgroundColor, border: `1px solid ${borderColor}` }}
        >
          <Center h='100%'>{props.text}</Center>
        </Card>
      </AspectRatio>
    </Center>
  );
};
