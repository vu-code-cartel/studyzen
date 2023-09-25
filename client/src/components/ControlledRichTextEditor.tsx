import { RichTextEditor, Link } from '@mantine/tiptap';
import { Editor, useEditor } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import Highlight from '@tiptap/extension-highlight';
import Underline from '@tiptap/extension-underline';
import TextAlign from '@tiptap/extension-text-align';
import Superscript from '@tiptap/extension-superscript';
import SubScript from '@tiptap/extension-subscript';
import { Color } from '@tiptap/extension-color';
import TextStyle from '@tiptap/extension-text-style';
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight';
import { Input, InputWrapperProps, useMantineColorScheme, useMantineTheme } from '@mantine/core';
import { lowlight } from '../main';
import { IconCloudUpload, IconCodeDots, IconEdit } from '@tabler/icons-react';
import { useTranslation } from 'react-i18next';
import { useEffect } from 'react';
import { useAppStore } from '../hooks/useAppStore';

interface ControlledRichTextEditorProps {
  label?: string;
  content?: string;
  setValue: (value: string) => void;
  isRequired?: boolean;
  inputProps?: InputWrapperProps;
  isReadonly?: boolean;
  showEditContentControl?: boolean;
  onEditContentClick?: (editor: Editor | null) => void;
  onSaveChangesClick?: (editor: Editor | null) => void;
}

export const ControlledRichTextEditor = (props: ControlledRichTextEditorProps) => {
  const theme = useMantineTheme();
  const isMobile = useAppStore((state) => state.isMobile);
  const { colorScheme } = useMantineColorScheme();
  const editor = useEditor({
    extensions: [
      StarterKit.configure({ codeBlock: false }),
      Underline,
      Link,
      Superscript,
      SubScript,
      Highlight,
      TextAlign.configure({ types: ['heading', 'paragraph'] }),
      TextStyle,
      Color,
      CodeBlockLowlight.configure({ lowlight }),
    ],
    onUpdate(editorProps) {
      props.setValue(editorProps.editor.getText() ? editorProps.editor.getHTML() : '');
    },
    content: props.content,
    editable: !props.isReadonly,
  });

  useEffect(() => {
    editor?.setEditable(!props.isReadonly, true);
  }, [editor, props.isReadonly]);

  return (
    <Input.Wrapper label={props.label} withAsterisk={props.isRequired} {...props.inputProps} w='100%'>
      <RichTextEditor
        editor={editor}
        mb={props.label ? 4 : 0}
        style={{ borderColor: props.inputProps?.error ? theme.colors.red[7] : undefined }}
        spellCheck='false'
        autoCorrect='off'
      >
        {(!props.isReadonly || (props.isReadonly && props.showEditContentControl)) && (
          // Sticky offset same as header size
          <RichTextEditor.Toolbar
            sticky
            stickyOffset={isMobile ? 48 : 0}
            bg={
              colorScheme == 'dark' ? theme.colors.dark[6] : colorScheme == 'light' ? theme.colors.gray[0] : undefined
            }
          >
            {props.showEditContentControl && props.onEditContentClick && (
              <RichTextEditor.ControlsGroup>
                <EditContentControl onClick={() => props.onEditContentClick?.(editor)} />
                {!props.isReadonly && props.onSaveChangesClick && (
                  <SaveChangesControl onClick={() => props.onSaveChangesClick?.(editor)} />
                )}
              </RichTextEditor.ControlsGroup>
            )}
            {!props.isReadonly && (
              <>
                <RichTextEditor.ControlsGroup>
                  <RichTextEditor.Bold />
                  <RichTextEditor.Italic />
                  <RichTextEditor.Underline />
                  <RichTextEditor.Strikethrough />
                  <RichTextEditor.ClearFormatting />
                  <RichTextEditor.Highlight />
                  <RichTextEditor.ColorPicker
                    colors={[
                      '#25262b',
                      '#868e96',
                      '#fa5252',
                      '#e64980',
                      '#be4bdb',
                      '#7950f2',
                      '#4c6ef5',
                      '#228be6',
                      '#15aabf',
                      '#12b886',
                      '#40c057',
                      '#82c91e',
                      '#fab005',
                      '#fd7e14',
                    ]}
                  />
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
                  <RichTextEditor.Code />
                  <RichTextEditor.CodeBlock icon={IconCodeDots} />
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
              </>
            )}
          </RichTextEditor.Toolbar>
        )}

        <RichTextEditor.Content />
      </RichTextEditor>
    </Input.Wrapper>
  );
};

interface ControlProps {
  onClick: () => void;
}

const EditContentControl = (props: ControlProps) => {
  const { t } = useTranslation();

  return (
    <RichTextEditor.Control
      onClick={props.onClick}
      aria-label={t('RichTextEditor.Control.EditContent')}
      title={t('RichTextEditor.Control.EditContent')}
    >
      <IconEdit stroke={1.5} size='1rem' />
    </RichTextEditor.Control>
  );
};

const SaveChangesControl = (props: ControlProps) => {
  const { t } = useTranslation();

  return (
    <RichTextEditor.Control
      onClick={props.onClick}
      aria-label={t('RichTextEditor.Control.SaveChanges')}
      title={t('RichTextEditor.Control.SaveChanges')}
    >
      <IconCloudUpload stroke={1.5} size='1rem' />
    </RichTextEditor.Control>
  );
};
