using System.Drawing;
namespace System.Windows.Forms
{
    public static class TextBoxUtil
    {

        public static void AppendText(this RichTextBox text, string value, Color color)
        {
            value = value.Replace("\r", string.Empty);
            text.AppendText(value);
            try
            {
                text.Select(text.TextLength - value.Length, value.Length);
                text.SelectionColor = color;
                text.Select(text.TextLength, 0);
            }
            catch { }
        }
    }
}
