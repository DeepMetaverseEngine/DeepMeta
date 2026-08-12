using DeepCore;
using DeepCore.Log;
using FreeImageAPI;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static DeepEditor.Common.G3D.GLView;
using static System.Net.Mime.MediaTypeNames;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace DeepEditor.Common.G3D
{
    public enum GLTextureAnchor
    {
        L_T,
        C_T,
        R_T,
        L_C,
        C_C,
        R_C,
        L_B,
        C_B,
        R_B,
    }

    public abstract class GLTexture2D : Disposable
    {
        protected static Logger log = new LazyLogger("GLTexture");
        public int TextureID { get; private set; } = 0;
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;
        abstract public string Text { get; set; }

        private Bitmap _cacheBitmap;
        private bool _autoDisposeBitmap;
        ~GLTexture2D()
        {
            if (_autoDisposeBitmap) _cacheBitmap?.Dispose();
            _cacheBitmap = null;
        }
        protected override void Disposing()
        {
            if (_autoDisposeBitmap) _cacheBitmap?.Dispose();
            _cacheBitmap = null;
            if (TextureID != 0)
            {
                GL.DeleteTexture(TextureID);
                TextureID = 0;
            }
        }
        public GLTexture2D InitWithFile(string file)
        {
            this.InitWithBitmap(new Bitmap(file), true);
            return this;
        }
        public GLTexture2D InitWithStream(Stream fs)
        {
            this.InitWithBitmap(new Bitmap(fs), true);
            return this;
        }
        public GLTexture2D InitWithBinary(byte[] bin)
        {
            this.InitWithBitmap(new Bitmap(new MemoryStream(bin)), true);
            return this;
        }
        public GLTexture2D InitWithText(string text, float fontSize, Color4 fontColor)
        {
            this.InitWithText(text, FontStyle.Regular, fontSize, fontColor, 0, Color4.Black, Color4.Transparent, Color4.Transparent, SizeF.Empty);
            return this;
        }
        public GLTexture2D InitWithText(string text, float fontSize, Color4 fontColor, int borderTime, Color4 borderColor)
        {
            this.InitWithText(text, FontStyle.Regular, fontSize, fontColor, borderTime, borderColor, Color4.Transparent, Color4.Transparent, SizeF.Empty);
            return this;
        }
        public virtual GLTexture2D InitWithText(string text, FontStyle style, float fontSize, Color4 fontColor, int borderTime, Color4 borderColor, Color4 backColor, Color4 backBorderColor, SizeF expectSize)
        {
            try
            {
                Text = text;
                var font = GLFonts.Instance.CreateFont(fontSize, style);
                var bounds = GLFonts.Instance.GetTextBounds(text, font, borderTime, expectSize.Width);
                var src = GLFonts.Instance.GenStringBuffer(
                      (int)Math.Ceiling(bounds.Width),
                      (int)Math.Ceiling(bounds.Height),
                      text, font,
                      fontColor,
                      borderTime,
                      borderColor,
                      backColor,
                      backBorderColor);
                InitWithBitmap(src, true);
            }
            catch (Exception err) { log.Error(err); }
            return this;
        }
        public virtual GLTexture2D InitWithBitmap(Bitmap bitmap, bool autoDispose=false)
        {
            _cacheBitmap = bitmap;
            _autoDisposeBitmap = autoDispose;
            if (bitmap != null)
            {
                this.Width = bitmap.Width;
                this.Height = bitmap.Height;
            }
            return this;
        }

        private void InternalInitTexture()
        {
            if (_cacheBitmap != null)
            {
                var bitmap = _cacheBitmap;
                _cacheBitmap = null;
                GL.Enable(EnableCap.Texture2D);
                try
                {
                    if (TextureID != 0)
                    {
                        GL.DeleteTexture(TextureID);
                        TextureID = 0;
                    }
                    GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Fastest);
                    this.TextureID = GL.GenTexture();
                    if (TextureID != 0)
                    {
                        GL.BindTexture(TextureTarget.Texture2D, TextureID);
                        var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                            ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                            OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);

                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                        GL.BindTexture(TextureTarget.Texture2D, 0);
                    }
                }
                finally
                {
                    GL.Disable(EnableCap.Texture2D);
                }
                if (_autoDisposeBitmap)
                {
                    bitmap.Dispose();
                }
            }
        }

        protected virtual void OnBeginDraw(PaintEventArgs3D sender) { InternalInitTexture(); }
        protected virtual void OnEndDraw() { }

        public bool DrawQuards2D(PaintEventArgs3D sender, float x, float y, GLTextureAnchor anchor = GLTextureAnchor.L_T)
        {
            return DrawQuards2D(sender, x, y, 0, 0, anchor);
        }
        public bool DrawQuards2D(PaintEventArgs3D sender, float x, float y, float w, float h, GLTextureAnchor anchor = GLTextureAnchor.L_T)
        {
            OnBeginDraw(sender);
            if (TextureID == 0) return false;
            if (anchor == GLTextureAnchor.L_C || anchor == GLTextureAnchor.C_C || anchor == GLTextureAnchor.R_C)
            {
                y += (h - Height) / 2;
            }
            if (anchor == GLTextureAnchor.L_B || anchor == GLTextureAnchor.C_B || anchor == GLTextureAnchor.R_B)
            {
                y += (h - Height);
            }
            if (anchor == GLTextureAnchor.C_T || anchor == GLTextureAnchor.C_C || anchor == GLTextureAnchor.C_B)
            {
                x += (w - Width) / 2;
            }
            if (anchor == GLTextureAnchor.R_T || anchor == GLTextureAnchor.R_C || anchor == GLTextureAnchor.R_B)
            {
                x += (w - Width);
            }
            if (w == 0) w = Width;
            if (h == 0) h = Height;
            GL.Enable(EnableCap.Texture2D);
            try
            {
                GL.BindTexture(TextureTarget.Texture2D, TextureID);
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(1f, 1f, 1f, 1f);
                GL.TexCoord2(0, 0);
                GL.Vertex2(x, y);
                GL.TexCoord2(1, 0);
                GL.Vertex2(x + w, y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(x + w, y + h);
                GL.TexCoord2(0, 1);
                GL.Vertex2(x, y + h);
                GL.End();
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            finally
            {
                GL.Disable(EnableCap.Texture2D);
            }
            OnEndDraw();
            return true;
        }
        public bool DrawQuards(PaintEventArgs3D sender, Vector3[] quads)
        {
            OnBeginDraw(sender);
            if (TextureID == 0) return false;
            GL.Enable(EnableCap.Texture2D);
            try
            {
                GL.BindTexture(TextureTarget.Texture2D, TextureID);
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(1f, 1f, 1f, 1f);
                GL.TexCoord2(0, 0);
                GL.Vertex3(quads[0]);
                GL.TexCoord2(1, 0);
                GL.Vertex3(quads[1]);
                GL.TexCoord2(1, 1);
                GL.Vertex3(quads[2]);
                GL.TexCoord2(0, 1);
                GL.Vertex3(quads[3]);
                GL.End();
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            finally
            {
                GL.Disable(EnableCap.Texture2D);
            }
            OnEndDraw();
            return true;
        }

    }
    public class GLImageTexture2D : GLTexture2D
    {
        public override string Text { get; set; } = "";
        public GLImageTexture2D()
        {
        }       
    }
    public class GLBitmapTexture2D : GLImageTexture2D
    {
        private Size renderHudSize;
        private Bitmap renderHudBuffer;
        private Graphics renderHudGraphics;
        public Size BufferSize { get => renderHudBuffer.Size; }
        public override string Text { get; set; } = "";
        public GLBitmapTexture2D(Size size)
        {
            this.renderHudSize = size;
            this.renderHudBuffer = new Bitmap(
                Math.Max(size.Width, 1),
                Math.Max(size.Height, 1),
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            this.renderHudGraphics = Graphics.FromImage(renderHudBuffer);
        }
        public Graphics BeginGraphics(Size size)
        {
            if (renderHudSize != size)
            {
                this.renderHudSize = size;
                this.renderHudGraphics.Dispose();
                this.renderHudBuffer.Dispose();
                this.renderHudBuffer = new Bitmap(
                Math.Max(size.Width, 1),
                Math.Max(size.Height, 1),
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                this.renderHudGraphics = Graphics.FromImage(renderHudBuffer);
            }
            else
            {
                renderHudGraphics.Clear(System.Drawing.Color.FromArgb(0, 0, 0, 0));
            }
            return renderHudGraphics;
        }
        public void Flush()
        {
            base.InitWithBitmap(this.renderHudBuffer, false);
        }
    }
    public class GLTextTexture2D : GLTexture2D
    {
        override public string Text
        {
            get => m_text;
            set { m_isDirty = (m_text != value); m_text = value; }
        }
        public FontStyle Style
        {
            get => m_style;
            set { m_isDirty = (m_style != value); m_style = value; }
        }
        public float FontSize
        {
            get => m_fontSize;
            set { m_isDirty = (m_fontSize != value); m_fontSize = value; }
        }
        public Color4 FontColor
        {
            get => m_fontColor;
            set { m_isDirty = (m_fontColor != value); m_fontColor = value; }
        }
        public int BorderTime
        {
            get => m_borderTime;
            set { m_isDirty = (m_borderTime != value); m_borderTime = value; }
        }
        public Color4 BorderColor
        {
            get => m_borderColor;
            set { m_isDirty = (m_borderColor != value); m_borderColor = value; }
        }
        public Color4 BackColor
        {
            get => m_backColor;
            set { m_isDirty = (m_backColor != value); m_backColor = value; }
        }
        public Color4 BackBorderColor
        {
            get => m_backBorderColor;
            set { m_isDirty = (m_backBorderColor != value); m_backBorderColor = value; }
        }
        public SizeF ExpectSize
        {
            get => m_expectSize;
            set { m_isDirty = (m_expectSize != value); m_expectSize = value; }
        }

        private bool m_isDirty = false;
        private string m_text;
        private FontStyle m_style;
        private float m_fontSize;
        private Color4 m_fontColor;
        private int m_borderTime;
        private Color4 m_borderColor;
        private Color4 m_backColor = Color4.Transparent;
        private Color4 m_backBorderColor = Color4.Transparent;
        private SizeF m_expectSize = SizeF.Empty;

        public GLTextTexture2D() { }
        //         public GLTextTexture2D(FontStyle style, float fontSize, Color4 fontColor, int borderTime, Color4 borderColor)
        //         {
        //             this.m_style = style;
        //             this.m_fontSize = fontSize;
        //             this.m_fontColor = fontColor;
        //             this.m_borderTime = borderTime;
        //             this.m_borderColor = borderColor;
        //         }
        public GLTextTexture2D(FontStyle style, float fontSize, Color4 fontColor)
        {
            this.m_style = style;
            this.m_fontSize = fontSize;
            this.m_fontColor = fontColor;
            this.m_borderTime = 0;
            this.m_borderColor = fontColor;
        }
        public override GLTexture2D InitWithText(string text, FontStyle style, float fontSize, Color4 fontColor, int borderTime, Color4 borderColor, Color4 backColor, Color4 backBorderColor, SizeF expectSize)
        {
            this.m_style = style;
            this.m_fontSize = fontSize;
            this.m_fontColor = fontColor;
            this.m_borderTime = borderTime;
            this.m_borderColor = borderColor;
            this.m_expectSize = expectSize;
            return base.InitWithText(text, style, fontSize, fontColor, borderTime, borderColor, backColor, backBorderColor, expectSize);
        }
        protected override void OnBeginDraw(PaintEventArgs3D sender)
        {
            if (m_isDirty)
            {
                m_isDirty = false;
                if (m_text != null)
                {
                    base.InitWithText(m_text, m_style, m_fontSize, m_fontColor, m_borderTime, m_borderColor, m_backColor, m_backBorderColor, m_expectSize);
                }
                else
                {
                    base.Dispose();
                }
            }
            base.OnBeginDraw(sender);
        }

    }

    public class Texture : IDisposable
    {
        public readonly int Handle;
        public virtual void Dispose()
        {
            if (Handle != 0)
            {
                GL.DeleteTexture(Handle);
            }
        }
        public static Texture LoadFromFile(string path)
        {
            using (var image = new Bitmap(path))
            {
                return LoadFromBitmap(image);
            }
        }
        public static Texture LoadFromBinary(byte[] data)
        {
            using (var image = new Bitmap(new MemoryStream(data)))
            {
                return LoadFromBitmap(image);
            }
        }
        public static Texture LoadFromStream(Stream sf)
        {
            using (var image = new Bitmap(sf))
            {
                return LoadFromBitmap(image);
            }
        }
        public static Texture LoadFromBitmap(Bitmap image)
        {
            // Generate handle
            int handle = GL.GenTexture();

            // Bind the handle
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            // For this example, we're going to use .NET's built-in System.Drawing library to load textures.

            // Load the image
            {
                // Our Bitmap loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
                // This will correct that, making the texture display properly.
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                // First, we get our pixels from the bitmap we loaded.
                // Arguments:
                //   The pixel area we want. Typically, you want to leave it as (0,0) to (width,height), but you can
                //   use other rectangles to get segments of textures, useful for things such as spritesheets.
                //   The locking mode. Basically, how you want to use the pixels. Since we're passing them to OpenGL,
                //   we only need ReadOnly.
                //   Next is the pixel format we want our pixels to be in. In this case, ARGB will suffice.
                //   We have to fully qualify the name because OpenTK also has an enum named PixelFormat.
                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // Now that our pixels are prepared, it's time to generate a texture. We do this with GL.TexImage2D.
                // Arguments:
                //   The type of texture we're generating. There are various different types of textures, but the only one we need right now is Texture2D.
                //   Level of detail. We can use this to start from a smaller mipmap (if we want), but we don't need to do that, so leave it at 0.
                //   Target format of the pixels. This is the format OpenGL will store our image with.
                //   Width of the image
                //   Height of the image.
                //   Border of the image. This must always be 0; it's a legacy parameter that Khronos never got rid of.
                //   The format of the pixels, explained above. Since we loaded the pixels as ARGB earlier, we need to use BGRA.
                //   Data type of the pixels.
                //   And finally, the actual pixels.
                GL.TexImage2D(TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);
            }

            // Now that our texture is loaded, we can set a few settings to affect how the image appears on rendering.

            // First, we set the min and mag filter. These are used for when the texture is scaled down and up, respectively.
            // Here, we use Linear for both. This means that OpenGL will try to blend pixels, meaning that textures scaled too far will look blurred.
            // You could also use (amongst other options) Nearest, which just grabs the nearest pixel, which makes the texture look pixelated if scaled too far.
            // NOTE: The default settings for both of these are LinearMipmap. If you leave these as default but don't generate mipmaps,
            // your image will fail to render at all (usually resulting in pure black instead).
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Now, set the wrapping mode. S is for the X axis, and T is for the Y axis.
            // We set this to Repeat so that textures will repeat when wrapped. Not demonstrated here since the texture coordinates exactly match
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // Next, generate mipmaps.
            // Mipmaps are smaller copies of the texture, scaled down. Each mipmap level is half the size of the previous one
            // Generated mipmaps go all the way down to just one pixel.
            // OpenGL will automatically switch between mipmaps when an object gets sufficiently far away.
            // This prevents moiré effects, as well as saving on texture bandwidth.
            // Here you can see and read about the morié effect https://en.wikipedia.org/wiki/Moir%C3%A9_pattern
            // Here is an example of mips in action https://en.wikipedia.org/wiki/File:Mipmap_Aliasing_Comparison.png
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(handle);
        }

        public Texture(int glHandle)
        {
            Handle = glHandle;
        }

        // Activate texture
        // Multiple textures can be bound, if your shader needs more than just one.
        // If you want to do that, use GL.ActiveTexture to set which slot GL.BindTexture binds to.
        // The OpenGL standard requires that there be at least 16, but there can be more depending on your graphics card.
        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
        public void EndUse(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
