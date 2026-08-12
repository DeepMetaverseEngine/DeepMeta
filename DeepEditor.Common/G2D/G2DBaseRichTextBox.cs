using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DeepCore.GUI.Display.Text;
using System.Xml;
using MaterialSkin.Controls;
using MaterialSkin;

namespace DeepEditor.Common.G2D
{
    public class G2DBaseRichTextBox : RichTextBox, IG2DBaseComponent
    {
        public System.Drawing.Color? CustomForeColor { get; set; }
        public System.Drawing.Color? CustomBackColor { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        public G2DBaseRichTextBox()
        {
            this.AutoSize = false;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

        }
        public void AppendText(string text, Color color, Font font = null, bool isNewLine = false)
        {
            this.SelectionStart = this.TextLength;
            this.SelectionLength = 0;
            this.SelectionColor = color;
            if (font != null)
            {
                this.SelectionFont = font;
            }
            this.AppendText(isNewLine ? $"{text}{Environment.NewLine}" : text);
            this.SelectionColor = this.ForeColor;
            this.SelectionFont = this.Font;
        }

        public void SetAttributedString(AttributedString atext, TextAttribute dattr)
        {
            DeepCore.GUI.Display.Color.DecodeRGBA(dattr.fontColor, out byte r, out byte g, out byte b, out byte a);
            var dcolor = Color.FromArgb(a, r, g, b);
            var dfont = new Font(Font.FontFamily, dattr.fontSize);
            this.Clear();
            this.SuspendLayout();
            atext.ForEachAttributesText((start, count, ta) =>
            {
                var color = dcolor;
                var font = dfont;
                var word = atext.Substring(start, count);
                if (ta.fontColor != 0)
                {
                    DeepCore.GUI.Display.Color.DecodeRGBA(ta.fontColor, out byte r, out byte g, out byte b, out byte a);
                    color = Color.FromArgb(a, r, g, b);
                }
                if (ta.fontSize != 0)
                {
                    font = new Font(font.FontFamily, ta.fontSize);
                }
                if (ta.fontStyle != DeepCore.GUI.Data.TextFontStyle.Plain)
                {
                    font = new Font(font.FontFamily, font.Size, (FontStyle)ta.fontStyle);
                }
                AppendText(word, color, font);

            });
            this.ResumeLayout();
        }
        public void SetAttributedString(XmlDocument xml)
        {
            var dsize = this.Font.Size;
            var dcolor = DeepCore.GUI.Display.Color.EncodeRGBA(this.ForeColor.R, this.ForeColor.G, this.ForeColor.B, this.ForeColor.A);
            var dattr = new TextAttribute(dcolor, dsize);
            var atext = new AttributedStringDecoder().CreateFromXML(xml, dattr);
            SetAttributedString(atext, dattr);
        }
        public XmlDocument GetAttributedString()
        {
            return null;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys kd)
        {
//             if (Focused)
//             {
//                 if (Keyboard.IsCtrlDown)
//                 {
//                     Keys keyData = kd ^ Keys.Control;
//                     switch (keyData)
//                     {
//                         case Keys.C: DoCopy(); return true;
//                         case Keys.V: if (!ReadOnly) DoPaste(); return true;
//                         case Keys.X: if (!ReadOnly) DoCut(); return true;
//                     }
//                 }
//                 else
//                 {
//                     switch (kd)
//                     {
//                         case Keys.Delete: if (!ReadOnly) DoDelete(); return true;
//                     }
//                 }
//             }
            return base.ProcessCmdKey(ref msg, kd);
        }

        protected virtual void DoCopy()
        {
            if (this.SelectionLength > 0)
            {
                try
                {
                    Win32.SetClipboard(this.Text.Substring(this.SelectionStart, this.SelectionLength));
                }
                catch { }
            }
        }
        protected virtual void DoPaste()
        {
            var text = this.Text;
            var clip = Clipboard.GetText();
            if (this.SelectionLength > 0)
            {
                text = text.Remove(this.SelectionStart, this.SelectionLength);
            }
            text = text.Insert(this.SelectionStart, clip);
            var new_pos = this.SelectionStart + clip.Length;
            this.Text = text;
            this.SelectionStart = new_pos;
            this.SelectionLength = 0;
        }
        protected virtual void DoDelete()
        {
            var text = this.Text;
            if (text.Length > 0)
            {
                if (this.SelectionLength > 0)
                {
                    text = text.Remove(this.SelectionStart, this.SelectionLength);
                }
                else if (this.SelectionStart < text.Length - 1)
                {
                    text = text.Substring(0, text.Length - 1);
                }
                this.Text = text;
            }
        }
        protected virtual void DoCut()
        {
            DoCopy();
            DoDelete();
        }
    }
}
