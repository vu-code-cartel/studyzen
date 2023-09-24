import { Button, Fieldset, Group, Stack, TextInput, Textarea } from '@mantine/core';
import { useTranslation } from 'react-i18next';
import { useButtonVariant } from '../hooks/useButtonVariant';
import { useForm } from '@mantine/form';
import { CreateCourseRequest } from '../api/requests';

interface CourseFormProps {
  onSubmit: (request: CreateCourseRequest) => void;
  isReadonly?: boolean;
  isEditable?: boolean;
  onEditClick?: () => void;
  onCancelClick?: () => void;
  initialValues?: CreateCourseRequest;
}

export const CourseForm = (props: CourseFormProps) => {
  const { t } = useTranslation();
  const buttonVariant = useButtonVariant();
  const form = useForm<CreateCourseRequest>({
    initialValues: props.initialValues ?? {
      name: '',
      description: '',
    },
    validate: {
      name: (value) => (value ? null : t('Course.Validation.NameMustNotBeEmpty')),
    },
  });

  return (
    <form onSubmit={form.onSubmit(props.onSubmit)} autoComplete='off'>
      <Fieldset>
        <Stack>
          <TextInput
            label={t('Course.Field.Name')}
            withAsterisk={!props.isReadonly}
            spellCheck='false'
            autoCorrect='off'
            {...form.getInputProps('name')}
            readOnly={props.isReadonly}
          />
          <Textarea
            label={t('Course.Field.Description')}
            autosize
            minRows={5}
            withAsterisk={!props.isReadonly}
            spellCheck='false'
            autoCorrect='off'
            readOnly={props.isReadonly}
            {...form.getInputProps('description')}
          />
          {props.isEditable && props.onEditClick && props.onCancelClick ? (
            <Group justify='end'>
              {props.isReadonly ? (
                <Button variant={buttonVariant} onClick={props.onEditClick}>
                  Edit
                </Button>
              ) : (
                <>
                  <Button variant='default' onClick={props.onCancelClick}>
                    Cancel
                  </Button>
                  <Button type='submit' variant={buttonVariant} color='teal'>
                    Save changes
                  </Button>
                </>
              )}
            </Group>
          ) : (
            <Button type='submit' color='teal' variant={buttonVariant} disabled={props.isReadonly}>
              {t('Course.Action.CreateCourse')}
            </Button>
          )}
        </Stack>
      </Fieldset>
    </form>
  );
};
