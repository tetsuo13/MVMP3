using NLog;
using NLog.Targets;
using System;
using System.Windows.Forms;

//
//Credit: https://github.com/justalemon/Lemon.NLog.WinForms
//
namespace Lemon.NLog.WinForms
{
    /// <summary>
    /// Logs text into a Text Box control in an existing form.
    /// </summary>
    [Target("TextBox")]
    public class TextBoxTarget : TargetWithContext
    {
        /// <summary>
        /// The TextBox control that will be used for this
        /// </summary>
        public TextBox TargetTextBox { get; }
        /// <summary>
        /// If a new line should be added at the end of the text.
        /// </summary>
        public bool AddNewLine { get; set; } = true;
        /// <summary>
        /// If the text should be appended instead of replaced.
        /// </summary>
        public bool Append { get; set; } = true;

        public TextBoxTarget(TextBox textBox) : base()
        {
            TargetTextBox = textBox;
        }

        protected override void Write(LogEventInfo LogEvent)
        {
            // Start by formatting the text that we need
            string text = Layout.Render(LogEvent);
            // If we need to add a new line, do it
            if (AddNewLine)
            {
                text += Environment.NewLine;
            }

            // If we need to invoke
            if (TargetTextBox.InvokeRequired)
            {
                // If a handle has not been created for this TextBox
                if (!TargetTextBox.IsHandleCreated)
                {
                    // Get the pointer/handle of the TextBox (is silently created if not)
                    TargetTextBox.Invoke(new Action(() => { _ = TargetTextBox.Handle; }));
                }

                // If we need to append, use AppendText
                if (Append)
                {
                    TargetTextBox.Invoke(new Action(() => TargetTextBox.AppendText(text)));
                }
                // Otherwise, replace the text
                else
                {
                    TargetTextBox.Invoke(new Action(() => TargetTextBox.Text = text));
                }
            }
            // Otherwise, do the same things without invoking
            else
            {
                if (!TargetTextBox.IsHandleCreated)
                {
                    _ = TargetTextBox.Handle;
                }

                // If we need to append, use AppendText
                if (Append)
                {
                    TargetTextBox.AppendText(text);
                }
                // Otherwise, replace the text
                else
                {
                    TargetTextBox.Text = text;
                }
            }
        }
    }
}
