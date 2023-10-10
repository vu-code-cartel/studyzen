import { Card, SimpleGrid, Text } from '@mantine/core';
import { getFlashcardSetRoute } from '../common/app-routes';
import { Link } from 'react-router-dom';
import { FlashcardSetDto } from '../api/dtos';

interface FlashcardSetListProps {
  flashcardSets: FlashcardSetDto[];
}

export const FlashcardSetList = (props: FlashcardSetListProps) => {
  return (
    <SimpleGrid cols={{ base: 1, sm: 3, md: 3 }}>
      {props.flashcardSets.map((flashcardSet) => (
        <Card
          key={flashcardSet.id}
          withBorder
          component={Link}
          to={getFlashcardSetRoute(flashcardSet.id, flashcardSet.name)}
        >
          <Text fw={500}>{flashcardSet.name}</Text>
        </Card>
      ))}
    </SimpleGrid>
  );
};
