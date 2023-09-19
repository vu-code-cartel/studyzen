import { Button, TextInput, Stack, Input, Container, useMantineTheme, Fieldset } from '@mantine/core';
import { useEditor } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import { useTranslation } from 'react-i18next';
import { PageHeader } from '../components/PageHeader';
import { RichTextEditor, Link } from '@mantine/tiptap';
import Highlight from '@tiptap/extension-highlight';
import Underline from '@tiptap/extension-underline';
import TextAlign from '@tiptap/extension-text-align';
import Superscript from '@tiptap/extension-superscript';
import SubScript from '@tiptap/extension-subscript';
import { AppBreadcrumbs } from '../components/AppBreadcrumbs';
import { AppRoutes } from '../common/app-routes';
import { useForm } from '@mantine/form';
import { usePageCategory } from '../hooks/usePageCategory';
import { useDocumentTitle } from '@mantine/hooks';
import { useButtonVariant } from '../hooks/useButtonVariant';

interface CreateCourseRequest {
  name: string;
  description: string;
}

export const NewCoursePage = () => {
  const { t } = useTranslation();
  const theme = useMantineTheme();
  const buttonVariant = useButtonVariant();

  useDocumentTitle(t('NewCoursePage.DocumentTitle'));
  usePageCategory('courses');

  const form = useForm<CreateCourseRequest>({
    initialValues: {
      name: '',
      description: '',
    },
    validate: {
      name: (value) => (value ? null : t('NewCoursePage.Errors.NameMustNotBeEmpty')),
      description: (value) => (value ? null : t('NewCoursePage.Errors.DescriptionMustNotBeEmpty')),
    },
  });

  const editor = useEditor({
    extensions: [
      StarterKit,
      Underline,
      Link,
      Superscript,
      SubScript,
      Highlight,
      TextAlign.configure({ types: ['heading', 'paragraph'] }),
    ],
    onUpdate(props) {
      form.setFieldValue('description', props.editor.getText() ? props.editor.getHTML() : '');
    },
  });

  const onCourseCreateClick = (request: CreateCourseRequest) => {
    alert(JSON.stringify(request));
  };

  return (
    <Container>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: t('CoursesPage.Title'), to: AppRoutes.Courses },
            { title: t('NewCoursePage.Title'), to: AppRoutes.NewCourse },
          ]}
        />
      </PageHeader>
      <form onSubmit={form.onSubmit(onCourseCreateClick)} autoComplete='off'>
        <Fieldset>
          <Stack>
            <TextInput label={t('NewCoursePage.Fields.Name')} withAsterisk {...form.getInputProps('name')} />

            <Input.Wrapper
              label={t('NewCoursePage.Fields.Description')}
              withAsterisk
              {...form.getInputProps('description')}
            >
              <RichTextEditor
                editor={editor}
                mb={4}
                style={{ borderColor: form.errors.description ? theme.colors.red[7] : undefined }}
              >
                <RichTextEditor.Toolbar sticky>
                  <RichTextEditor.ControlsGroup>
                    <RichTextEditor.Bold />
                    <RichTextEditor.Italic />
                    <RichTextEditor.Underline />
                    <RichTextEditor.Strikethrough />
                    <RichTextEditor.ClearFormatting />
                    <RichTextEditor.Highlight />
                    <RichTextEditor.Code />
                  </RichTextEditor.ControlsGroup>

                  <RichTextEditor.ControlsGroup>
                    <RichTextEditor.H1 />
                    <RichTextEditor.H2 />
                    <RichTextEditor.H3 />
                    <RichTextEditor.H4 />
                  </RichTextEditor.ControlsGroup>

                  <RichTextEditor.ControlsGroup>
                    <RichTextEditor.Blockquote />
                    <RichTextEditor.Hr />
                    <RichTextEditor.BulletList />
                    <RichTextEditor.OrderedList />
                    <RichTextEditor.Subscript />
                    <RichTextEditor.Superscript />
                  </RichTextEditor.ControlsGroup>

                  <RichTextEditor.ControlsGroup>
                    <RichTextEditor.Link />
                    <RichTextEditor.Unlink />
                  </RichTextEditor.ControlsGroup>

                  <RichTextEditor.ControlsGroup>
                    <RichTextEditor.AlignLeft />
                    <RichTextEditor.AlignCenter />
                    <RichTextEditor.AlignJustify />
                    <RichTextEditor.AlignRight />
                  </RichTextEditor.ControlsGroup>
                </RichTextEditor.Toolbar>

                <RichTextEditor.Content />
              </RichTextEditor>
            </Input.Wrapper>

            <Button type='submit' color='teal' variant={buttonVariant}>
              {t('NewCoursePage.Submit')}
            </Button>
          </Stack>
        </Fieldset>
      </form>
    </Container>
  );
};
