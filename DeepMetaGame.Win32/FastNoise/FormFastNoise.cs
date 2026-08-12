using DeepCore;
using DeepEditor.Common.G2D;
using DeepEditor.Common.G3D;
using DeepEditor.Common.Voxel;
using FastNoise;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.WinForms;
using Org.BouncyCastle.Bcpg;

namespace DeepMetaGame.Win32.FastNoise
{
    public partial class FormFastNoise : G2DBaseForm
    {
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public NoiseGenerator Gen { get; set; } = new NoiseGenerator();
        FastNoiseConfig cfg = new FastNoiseConfig()
        {
            noiseTexMin = -1,
            noiseTexMax = 6,
            viewScale = 5,
        };
        View3D view;
        public FormFastNoise()
        {
            InitializeComponent();
            this.Load += FormFastNoise_Load;
            this.g2dPropertyGrid1.SetSelectedObject(cfg);
            this.g2dPropertyGrid1.PropertyValueChanged += G2dPropertyGrid1_PropertyValueChanged;
        }

        //--------------------------------------------------------------------------------------------

        private void FormFastNoise_Load(object sender, EventArgs e)
        {
            this.view = new View3D(this.glControl1, this.timer1);
        }
        private void G2dPropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            GenNoiseMap();
        }
        private void btn_Gen_Click(object sender, EventArgs e)
        {
            GenNoiseMap();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        //--------------------------------------------------------------------------------------------

        private Size prevSize = Size.Empty;
        public Bitmap GenNoiseMap()
        {
            var bitmap = new Bitmap(cfg.sizeX, cfg.sizeY);
            var colors = cfg.GetColors(Gen.Noise);
            var rand = new Random();
            view.RootObject.ClearChildren();
            var matrix = Gen.GenMap(rand, cfg, (x, y, n, layer) =>
            {
                var c = (int)colors[x, y];
                bitmap.SetPixel(x, y, Color.FromArgb(c, c, c));
                this.view.AddLayerObject(rand, cfg, x, y, n, layer);
            });
            this.pictureBox1.Image = bitmap;
            this.pictureBox1.Invalidate();
            this.view.SetupMap(matrix, cfg, bitmap, resetCamera: bitmap.Size != prevSize);
            prevSize = bitmap.Size;
            return bitmap;
        }

        static Color4 GetLineColor(float height)
        {
            var c = AltitudeLines.GetLineColor(height);
            return GLUtils.Argb2Color4(c.ColorARGB);
        }

        class View3D : GLView
        {
            private VertexArrayObject<Vertex> Mesh = new VertexArrayObject<Vertex>(PrimitiveType.Triangles);
            private GLTexture2D texture = new GLImageTexture2D();
            private Bitmap bitmap;
            public View3D(GLControl control, System.Windows.Forms.Timer timer) : base(control, timer)
            {
                this.OnRender += View3D_OnRender;
                this.OnInit += View3D_OnInit;
                this.OnDestory += View3D_OnDestory; ;
            }
            private void View3D_OnInit(GLView sender)
            {
                this.Mesh.SetShader(Shaders.GetOrAdd("TintShader", n =>
                {
                    var ret = new TintShader();
                    //                     ret.OnShaderBegin += (s, e) =>
                    //                     {
                    //                         ret.LightPosition = Camera.CamPosition + new Vector3(0, 10, 0);
                    //                     };
                    return ret;
                }));
            }
            private void View3D_OnDestory(GLView sender)
            {
                Mesh?.Dispose();
            }
            private void View3D_OnRender(GLView sender, PaintEventArgs3D e)
            {
                if (this.bitmap != null)
                {
                    DrawingVoxelObject.DrawBounds(Color4.White, 0, 0, bitmap.Width, bitmap.Height, 1);
                }
                //this.Mesh.Draw();
                this.Mesh.Draw(e);
            }
            public void AddLayerObject(Random rand, FastNoiseConfig cfg, int x, int y, float n, INoiseLayer layer)
            {
                if (layer == null) return;
                if (rand.Next(0, 100) > 10) return;
                var obj = new GLViewTextObject3D();
                obj.Text = $"{layer.Name}";
                obj.Position = new Vector3(x, CMath.Clamp(n, cfg.clampMin, cfg.clampMax) * cfg.viewScale, y);
                RootObject.AddChild(obj);
            }
            public void SetupMap(float[,] matrix, FastNoiseConfig cfg, Bitmap bitmap, bool resetCamera)
            {
                this.bitmap = bitmap;
                this.texture.InitWithBitmap(bitmap);
                this.Mesh.Flush();
                var top = 0f;
                for (int x = 0; x < bitmap.Width - 1; x++)
                {
                    for (int y = 0; y < bitmap.Height - 1; y++)
                    {
                        var c1 = GetLineColor(matrix[x, y]);
                        var c2 = GetLineColor(matrix[x + 1, y]);
                        var c3 = GetLineColor(matrix[x, y + 1]);
                        var c4 = GetLineColor(matrix[x + 1, y + 1]);
                        var v1 = new Vector3(x, CMath.Clamp(matrix[x, y], cfg.clampMin, cfg.clampMax) * cfg.viewScale, y);
                        var v2 = new Vector3(x + 1, CMath.Clamp(matrix[x + 1, y], cfg.clampMin, cfg.clampMax) * cfg.viewScale, y);
                        var v3 = new Vector3(x, CMath.Clamp(matrix[x, y + 1], cfg.clampMin, cfg.clampMax) * cfg.viewScale, y + 1);
                        var v4 = new Vector3(x + 1, CMath.Clamp(matrix[x + 1, y + 1], cfg.clampMin, cfg.clampMax) * cfg.viewScale, y + 1);
                        top = Math.Max(top, v1.Y);

                        this.Mesh.Add(new Vertex() { Position = v1, Color = c1, Normal = new Vector3(0, 1, 0) });
                        this.Mesh.Add(new Vertex() { Position = v2, Color = c2, Normal = new Vector3(0, 1, 0) });
                        this.Mesh.Add(new Vertex() { Position = v3, Color = c3, Normal = new Vector3(0, 1, 0) });

                        this.Mesh.Add(new Vertex() { Position = v2, Color = c2, Normal = new Vector3(0, 1, 0) });
                        this.Mesh.Add(new Vertex() { Position = v4, Color = c4, Normal = new Vector3(0, 1, 0) });
                        this.Mesh.Add(new Vertex() { Position = v3, Color = c3, Normal = new Vector3(0, 1, 0) });
                    }
                }
                if (resetCamera)
                {
                    this.Camera.SetTerrain(bitmap.Width, bitmap.Height);
                }
            }

        }

    }

}
