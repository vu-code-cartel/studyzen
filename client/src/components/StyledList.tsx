import { Card, Divider, Paper } from '@mantine/core';

interface StyledListProps {
  items: React.ReactNode[];
}

export const StyledList = (props: StyledListProps) => {
  if (props.items.length < 1) {
    return <></>;
  }

  return (
    <Card withBorder p={0}>
      {props.items.map((item, idx) => (
        <div key={idx}>
          {idx != 0 && <Divider />}
          <Paper p='md' style={{ border: 0, background: 'transparent' }}>
            {item}
          </Paper>
        </div>
      ))}
    </Card>
  );
};
