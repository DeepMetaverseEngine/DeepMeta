namespace MaterialSkin.Controls
{
    using DeepEditor.Common.G2D;
    using MaterialSkin.Animations;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class MaterialContextMenuStrip : ContextMenuStrip, IMaterialControl
    {
        //Properties for managing the material design properties
        [Browsable(false)]
        public int Depth { get; set; }

        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        [Browsable(false)]
        public MouseState MouseState { get; set; }

        internal AnimationManager AnimationManager;

        internal Point AnimationSource;

        public delegate void ItemClickStart(object sender, ToolStripItemClickedEventArgs e);

        public event ItemClickStart OnItemClickStart;

        public MaterialContextMenuStrip()
        {
            Renderer = new MaterialToolStripRender();

            AnimationManager = new AnimationManager(false)
            {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };
            AnimationManager.OnAnimationProgress += sender => Invalidate();
            AnimationManager.OnAnimationFinished += sender => OnItemClicked(_delayesArgs);

            BackColor = SkinManager.BackdropColor;
        }
        public MaterialContextMenuStrip(IContainer container) : base(container)
        {
            Renderer = new MaterialToolStripRender();

            AnimationManager = new AnimationManager(false)
            {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };
            AnimationManager.OnAnimationProgress += sender => Invalidate();
            AnimationManager.OnAnimationFinished += sender => OnItemClicked(_delayesArgs);

            BackColor = SkinManager.BackdropColor;
        }
        protected override void OnOpening(CancelEventArgs e)
        {
            SkinManager.UpdateBackgrounds(this);
            base.OnOpening(e);
        }

        protected override void OnMouseUp(MouseEventArgs mea)
        {
            base.OnMouseUp(mea);

            AnimationSource = mea.Location;
        }

        private ToolStripItemClickedEventArgs _delayesArgs;

        protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null && !(e.ClickedItem is ToolStripSeparator))
            {
                if (e == _delayesArgs)
                {
                    //The event has been fired manualy because the args are the ones we saved for delay
                    base.OnItemClicked(e);
                }
                else
                {
                    //Interrupt the default on click, saving the args for the delay which is needed to display the animaton
                    _delayesArgs = e;

                    //Fire custom event to trigger actions directly but keep cms open
                    OnItemClickStart?.Invoke(this, e);

                    //Start animation
                    AnimationManager.StartNewAnimation(AnimationDirection.In);
                }
            }
        }

        protected override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
        {
            //base.CreateDefaultItem(text, image, onClick);
            if (text == "-")
            {
                return new ToolStripSeparator();
            }
            else
            {
                var item = new G2DBaseToolStripMenuItem() { Text=text, Image=image};
                item.Click += onClick;
                return item;
            }
        }
    }

    public class MaterialToolStripMenuItem : ToolStripMenuItem
    {
        public MaterialToolStripMenuItem()
        {
            //AutoSize = false;
            Size = new Size(128, 32);
        }

//         protected override ToolStripDropDown CreateDefaultDropDown()
//         {
//             var baseDropDown = base.CreateDefaultDropDown();
//             if (DesignMode) return baseDropDown;
// 
//             var defaultDropDown = new MaterialContextMenuStrip() { OwnerItem = this };
//             defaultDropDown.Items.AddRange(baseDropDown.Items);
//             defaultDropDown.MaximumSize = new Size(512, 512);
//             return defaultDropDown;
//         }
    }

}
