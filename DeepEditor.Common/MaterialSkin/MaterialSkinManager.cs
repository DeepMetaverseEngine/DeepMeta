namespace MaterialSkin
{
    using DeepEditor.Common;
    using DeepEditor.Common.G2D;
    using DeepEditor.Common.G2D.DataGrid;
    using DeepEditor.Common.Properties;
    using MaterialSkin.Controls;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class MaterialSkinManager
    {
        public static bool DEFAULT_DARK_MODE = true;

        private static MaterialSkinManager _instance;
        public static MaterialSkinManager Instance => _instance ?? (_instance = new MaterialSkinManager());

        private readonly HashSet<Form> _formsToManage = new HashSet<Form>();
        private static HashSet<Type> _ignoreTypes = new HashSet<Type>();

        // Constructor
        public MaterialSkinManager()
        {
            _instance = this;

            // Theme = Themes.LIGHT;
            // ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);

            SetDarkMode(DEFAULT_DARK_MODE);

            InitFonts();
        }
        public bool SetDarkMode(bool value)
        {
            if (value)
            {
                SetTheme(MaterialSkinManager.Themes.DARK, new ColorScheme(
                    Primary.Indigo500,
                    Primary.Indigo700,
                    Primary.Indigo100,
                    Accent.Pink200,
                    TextShade.WHITE));
            }
            else
            {
                SetTheme(MaterialSkinManager.Themes.LIGHT, new ColorScheme(
                    Primary.Indigo500,
                    Primary.Indigo700,
                    Primary.Indigo100,
                    Accent.Pink200,
                    TextShade.BLACK));
            }
            return value;
        }

        // Destructor
        ~MaterialSkinManager()
        {
            foreach (IntPtr handle in logicalFonts.Values)
            {
                NativeTextRenderer.DeleteObject(handle);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------------
        #region Dyanmic Themes

        public delegate void SkinManagerEventHandler(object sender);
        public delegate bool UpdateControlBackColorHandler(Control control, Color newBackColor);

        public event SkinManagerEventHandler ColorSchemeChanged;
        public event SkinManagerEventHandler ThemeChanged;
        public event UpdateControlBackColorHandler TryUpdateControlBackColor;

        /// <summary>
        /// Set this property to false to stop enforcing the backcolor on non-materialSkin components
        /// </summary>
        public bool EnforceBackcolorOnAllComponents = true;

        public static void AddIgnoreControlType(Type type)
        {
            _ignoreTypes.Add(type);
        }

        public void AddFormToManage(Form materialForm)
        {
            if (_formsToManage.Contains(materialForm))
            {
                return;
            }
            _formsToManage.Add(materialForm);
            UpdateBackgrounds(materialForm);
            // Set background on newly added controls
            materialForm.ControlAdded += static (sender, e) =>
            {
                Instance.UpdateControlBackColor(e.Control, Instance.BackdropColor);
            };
        }

        public void RemoveFormToManage(Form materialForm)
        {
            _formsToManage.Remove(materialForm);
        }

        private void UpdateBackgrounds()
        {
            var newBackColor = BackdropColor;
            foreach (var materialForm in _formsToManage)
            {
                materialForm.SuspendLayout();
                //materialForm.SuspendDrawing();
                try
                {
                    materialForm.BackColor = newBackColor;
                    UpdateControlBackColor(materialForm, newBackColor);
                }
                finally
                {
                    //materialForm.ResumeDrawing();
                    materialForm.ResumeLayout();
                }
            }
        }
        public void UpdateBackgrounds(Control controlToUpdate)
        {
            var newBackColor = BackdropColor;
            UpdateControlBackColor(controlToUpdate, newBackColor);
        }
        public void UpdateToolStripItem(ToolStripItem item)
        {
            var newBackColor = BackdropColor;
            UpdateToolStripItem(item, newBackColor);
        }

        protected virtual void UpdateControlBackColor(Control controlToUpdate, in Color newBackColor)
        {
            // No control
            if (controlToUpdate == null) return;
            {
                if (_ignoreTypes.Contains(controlToUpdate.GetType()))
                {
                    return;
                }
            }
            //controlToUpdate.SuspendDrawing();
            controlToUpdate.SuspendLayout();
            try
            {


                if (TryUpdateControlBackColor != null && TryUpdateControlBackColor(controlToUpdate, newBackColor))
                {
                    return;
                }

                controlToUpdate.ForeColor = TextHighEmphasisColor;
                controlToUpdate.BackColor = newBackColor;

                // Control's Context menu
                if (controlToUpdate.ContextMenuStrip != null)
                {
                    UpdateToolStrip(controlToUpdate.ContextMenuStrip, in newBackColor);
                }

                if (controlToUpdate is PropertyGrid propGrid)
                {
                    propGrid.LineColor = PropertyGridLineColor;
                    propGrid.DisabledItemForeColor = Color.Gray;

                    propGrid.CategorySplitterColor = DividersColor;
                    propGrid.CategoryForeColor = TextHighEmphasisColor;

                    propGrid.CommandsBackColor = newBackColor;
                    propGrid.CommandsBorderColor = newBackColor;
                    propGrid.CommandsForeColor = TextHighEmphasisColor;

                    propGrid.ViewForeColor = TextHighEmphasisColor;
                    propGrid.ViewBackColor = newBackColor;
                    propGrid.ViewBorderColor = newBackColor;

                    propGrid.HelpBorderColor = DividersColor;
                    propGrid.HelpForeColor = TextHighEmphasisColor;
                    propGrid.HelpBackColor = newBackColor;
                }
                if (controlToUpdate is G2DPropertyGrid propGridG2D)
                {
                }
                if (controlToUpdate is DataGridView dataGrid)
                {
                    UpdateDataGridView(dataGrid, in newBackColor);
                }

                if (controlToUpdate is ToolStrip tools)
                {
                    UpdateToolStrip(tools, newBackColor);
                }


                if (controlToUpdate is TabControl tabControl)
                {
                }
                if (controlToUpdate is ScrollableControl scrollableControl)
                {

                }
                if (controlToUpdate is SplitContainer splitContainer)
                {

                }
                if (controlToUpdate is Panel panel)
                {
                    //panel.BorderStyle = BorderStyle.None;
                }
                if (controlToUpdate is TreeView tree)
                {
                    tree.LineColor = TreeViewLineColor;
                }
                if (controlToUpdate is G2DBaseListView list)
                {

                }
                if (controlToUpdate is G2DBaseButton btn)
                {

                }

                // Material Tabcontrol pages
                if (controlToUpdate is TabPage tabPage)
                {
                    tabPage.BackColor = newBackColor;
                    tabPage.ForeColor = TextHighEmphasisColor;
                    //tabPage.UseVisualStyleBackColor = true;
                }

                // Material Divider
                else if (controlToUpdate is MaterialDivider)
                {
                    controlToUpdate.BackColor = DividersColor;
                }

                // Other Material Skin control
                else if (controlToUpdate.IsMaterialControl())
                {
                    controlToUpdate.BackColor = newBackColor;
                    controlToUpdate.ForeColor = TextHighEmphasisColor;
                }
                // Other Generic control not part of material skin
                else if (EnforceBackcolorOnAllComponents && controlToUpdate.HasProperty("BackColor") && !controlToUpdate.IsMaterialControl() && controlToUpdate.Parent != null)
                {
                    controlToUpdate.BackColor = controlToUpdate.Parent.BackColor;
                    controlToUpdate.ForeColor = TextHighEmphasisColor;
                    try
                    {
                        var f = getFontByType(FontType.Body1);
                        controlToUpdate.Font = f;
                    }
                    catch { }
                }
                {
                    if (controlToUpdate is IG2DBaseComponent g2d)
                    {
                        if (g2d.CustomBackColor.HasValue)
                        {
                            controlToUpdate.BackColor = g2d.CustomBackColor.Value;
                        }
                        if (g2d.CustomForeColor.HasValue)
                        {
                            controlToUpdate.ForeColor = g2d.CustomForeColor.Value;
                        }
                    }
                    if (controlToUpdate is IG2DSkinListener skinListener)
                    {
                        skinListener.OnSkinChanged(this, controlToUpdate, this.Theme);
                    }
                }
                // Recursive call to control's children
                foreach (Control control in controlToUpdate.Controls)
                {
                    if (_ignoreTypes.Contains(control.GetType()))
                    {
                        continue;
                    }
                    UpdateControlBackColor(control, in newBackColor);
                }
            }
            catch
            {
            }
            finally
            {
                controlToUpdate.ResumeLayout();
                //controlToUpdate.ResumeDrawing();
            }
        }

        protected virtual void UpdateToolStrip(ToolStrip toolStrip, in Color newBackColor)
        {
            if (toolStrip == null)
            {
                return;
            }

            toolStrip.BackColor = newBackColor;

            foreach (ToolStripItem item in toolStrip.Items)
            {
                UpdateToolStripItem(item, in newBackColor);

            }
        }

        protected virtual void UpdateToolStripItem(ToolStripItem item, in Color newBackColor)
        {
            if (item == null)
            {
                return;
            }
            item.ForeColor = TextHighEmphasisColor;

            if (item is ToolStripMenuItem menu)
            {
                //item.BackColor = newBackColor;
                if (menu.HasDropDown)
                {
                    //recursive call
                    UpdateToolStrip(menu.DropDown, in newBackColor);
                }
            }
            if (item is ToolStripDropDownItem dropDown)
            {
                //item.BackColor = newBackColor;
                if (dropDown.HasDropDown)
                {
                    //recursive call
                    UpdateToolStrip(dropDown.DropDown, in newBackColor);
                }
            }
            if (item is ToolStripSeparator split)
            {
                split.BackColor = newBackColor;
                split.ForeColor = DividersColor;
            }
            if (item is ToolStripDropDownButton dropButton)
            {
                //item.BackColor = newBackColor;

            }
            if (item is ToolStripTextBox toolText)
            {
                item.BackColor = newBackColor;

            }
            if (item is ToolStripComboBox toolCombo)
            {
                item.BackColor = newBackColor;

            }
            if (item is ToolStripButton toolButton)
            {
                item.BackColor = newBackColor;
            }
            if (item is ToolStripLabel toolLabel)
            {
                toolLabel.BackColor = newBackColor;
            }
            if (item is ToolStripStatusLabel tsLabel)
            {
                tsLabel.BackColor = newBackColor;
            }

            if (item is IG2DBaseComponent g2d)
            {
                if (g2d.CustomBackColor.HasValue)
                {
                    item.BackColor = g2d.CustomBackColor.Value;
                }
                if (g2d.CustomForeColor.HasValue)
                {
                    item.ForeColor = g2d.CustomForeColor.Value;
                }
            }
            if (item is IG2DBaseToolStripItem gitem)
            {
                gitem.Image = UpdateToolStripItemImage(gitem, in newBackColor);
            }
        }

        protected virtual Image UpdateToolStripItemImage(IG2DBaseToolStripItem item, in Color newBackColor)
        {
            var src = item.ImageOrigin;
            if (src != null)
            {
                return src.ToSingleColor(TextHighEmphasisColor);
            }
            return item.Image;
        }
        protected virtual void UpdateDataGridView(DataGridView grid, in Color newBackColor)
        {
            grid.ForeColor = TextHighEmphasisColor;
            grid.BackColor = newBackColor;
        }
        #endregion
        //------------------------------------------------------------------------------------------------------------------------------------------
        #region ColorAndTheme

        public int FORM_PADDING = 14;

        // Themes
        private Themes _theme;

        public Themes Theme
        {
            get { return _theme; }
            //             set
            //             {
            //                 _theme = value;
            //                 UpdateBackgrounds();
            //                 ThemeChanged?.Invoke(this);
            //             }
        }
        private ColorScheme _colorScheme;

        public ColorScheme ColorScheme
        {
            get { return _colorScheme; }
            //             set
            //             {
            //                 _colorScheme = value;
            //                 UpdateBackgrounds();
            //                 ColorSchemeChanged?.Invoke(this);
            //             }
        }

        public void SetTheme(Themes themes, ColorScheme scheme)
        {
            this._theme = themes;
            UpdateBackgrounds();
            ThemeChanged?.Invoke(this);

            this._colorScheme = scheme;
            UpdateBackgrounds();
            ColorSchemeChanged?.Invoke(this);
        }

        public enum Themes : byte
        {
            LIGHT,
            DARK
        }

        // Text
        private static readonly Color TEXT_HIGH_EMPHASIS_LIGHT = Color.FromArgb(255, 255, 255, 255); // Alpha 87%
        private static readonly Brush TEXT_HIGH_EMPHASIS_LIGHT_BRUSH = new SolidBrush(TEXT_HIGH_EMPHASIS_LIGHT);
        private static readonly Color TEXT_HIGH_EMPHASIS_DARK = Color.FromArgb(255, 0, 0, 0); // Alpha 87%
        private static readonly Brush TEXT_HIGH_EMPHASIS_DARK_BRUSH = new SolidBrush(TEXT_HIGH_EMPHASIS_DARK);

        private static readonly Color TEXT_HIGH_EMPHASIS_LIGHT_NOALPHA = Color.FromArgb(255, 255, 255, 255); // Alpha 100%
        private static readonly Brush TEXT_HIGH_EMPHASIS_LIGHT_NOALPHA_BRUSH = new SolidBrush(TEXT_HIGH_EMPHASIS_LIGHT_NOALPHA);
        private static readonly Color TEXT_HIGH_EMPHASIS_DARK_NOALPHA = Color.FromArgb(255, 0, 0, 0); // Alpha 100%
        private static readonly Brush TEXT_HIGH_EMPHASIS_DARK_NOALPHA_BRUSH = new SolidBrush(TEXT_HIGH_EMPHASIS_DARK_NOALPHA);

        private static readonly Color TEXT_MEDIUM_EMPHASIS_LIGHT = Color.FromArgb(153, 255, 255, 255); // Alpha 60%
        private static readonly Brush TEXT_MEDIUM_EMPHASIS_LIGHT_BRUSH = new SolidBrush(TEXT_MEDIUM_EMPHASIS_LIGHT);
        private static readonly Color TEXT_MEDIUM_EMPHASIS_DARK = Color.FromArgb(153, 0, 0, 0); // Alpha 60%
        private static readonly Brush TEXT_MEDIUM_EMPHASIS_DARK_BRUSH = new SolidBrush(TEXT_MEDIUM_EMPHASIS_DARK);

        private static readonly Color TEXT_DISABLED_OR_HINT_LIGHT = Color.FromArgb(97, 255, 255, 255); // Alpha 38%
        private static readonly Brush TEXT_DISABLED_OR_HINT_LIGHT_BRUSH = new SolidBrush(TEXT_DISABLED_OR_HINT_LIGHT);
        private static readonly Color TEXT_DISABLED_OR_HINT_DARK = Color.FromArgb(97, 0, 0, 0); // Alpha 38%
        private static readonly Brush TEXT_DISABLED_OR_HINT_DARK_BRUSH = new SolidBrush(TEXT_DISABLED_OR_HINT_DARK);

        // Dividers and thin lines
        private static readonly Color DIVIDERS_LIGHT = Color.FromArgb(30, 255, 255, 255); // Alpha 30%
        private static readonly Brush DIVIDERS_LIGHT_BRUSH = new SolidBrush(DIVIDERS_LIGHT);
        private static readonly Color DIVIDERS_DARK = Color.FromArgb(30, 0, 0, 0); // Alpha 30%
        private static readonly Brush DIVIDERS_DARK_BRUSH = new SolidBrush(DIVIDERS_DARK);
        private static readonly Color DIVIDERS_ALTERNATIVE_LIGHT = Color.FromArgb(153, 255, 255, 255); // Alpha 60%
        private static readonly Brush DIVIDERS_ALTERNATIVE_LIGHT_BRUSH = new SolidBrush(DIVIDERS_ALTERNATIVE_LIGHT);
        private static readonly Color DIVIDERS_ALTERNATIVE_DARK = Color.FromArgb(153, 0, 0, 0); // Alpha 60%
        private static readonly Brush DIVIDERS_ALTERNATIVE_DARK_BRUSH = new SolidBrush(DIVIDERS_ALTERNATIVE_DARK);

        // Checkbox / Radio / Switches
        private static readonly Color CHECKBOX_OFF_LIGHT = Color.FromArgb(138, 0, 0, 0);
        private static readonly Brush CHECKBOX_OFF_LIGHT_BRUSH = new SolidBrush(CHECKBOX_OFF_LIGHT);
        private static readonly Color CHECKBOX_OFF_DARK = Color.FromArgb(179, 255, 255, 255);
        private static readonly Brush CHECKBOX_OFF_DARK_BRUSH = new SolidBrush(CHECKBOX_OFF_DARK);
        private static readonly Color CHECKBOX_OFF_DISABLED_LIGHT = Color.FromArgb(66, 0, 0, 0);
        private static readonly Brush CHECKBOX_OFF_DISABLED_LIGHT_BRUSH = new SolidBrush(CHECKBOX_OFF_DISABLED_LIGHT);
        private static readonly Color CHECKBOX_OFF_DISABLED_DARK = Color.FromArgb(77, 255, 255, 255);
        private static readonly Brush CHECKBOX_OFF_DISABLED_DARK_BRUSH = new SolidBrush(CHECKBOX_OFF_DISABLED_DARK);

        // Switch specific
        private static readonly Color SWITCH_OFF_THUMB_LIGHT = Color.FromArgb(255, 255, 255, 255);
        private static readonly Color SWITCH_OFF_THUMB_DARK = Color.FromArgb(255, 190, 190, 190);
        private static readonly Color SWITCH_OFF_TRACK_LIGHT = Color.FromArgb(100, 0, 0, 0);
        private static readonly Color SWITCH_OFF_TRACK_DARK = Color.FromArgb(100, 255, 255, 255);
        private static readonly Color SWITCH_OFF_DISABLED_THUMB_LIGHT = Color.FromArgb(255, 230, 230, 230);
        private static readonly Color SWITCH_OFF_DISABLED_THUMB_DARK = Color.FromArgb(255, 150, 150, 150);

        // Generic back colors - for user controls
        private static readonly Color BACKGROUND_LIGHT = Color.FromArgb(255, 255, 255, 255);
        private static readonly Brush BACKGROUND_LIGHT_BRUSH = new SolidBrush(BACKGROUND_LIGHT);
        private static readonly Color BACKGROUND_DARK = Color.FromArgb(255, 80, 80, 80);
        private static readonly Brush BACKGROUND_DARK_BRUSH = new SolidBrush(BACKGROUND_DARK);
        private static readonly Color BACKGROUND_ALTERNATIVE_LIGHT = Color.FromArgb(10, 0, 0, 0);
        private static readonly Brush BACKGROUND_ALTERNATIVE_LIGHT_BRUSH = new SolidBrush(BACKGROUND_ALTERNATIVE_LIGHT);
        private static readonly Color BACKGROUND_ALTERNATIVE_DARK = Color.FromArgb(10, 255, 255, 255);
        private static readonly Brush BACKGROUND_ALTERNATIVE_DARK_BRUSH = new SolidBrush(BACKGROUND_ALTERNATIVE_DARK);
        private static readonly Color BACKGROUND_HOVER_LIGHT = Color.FromArgb(20, 0, 0, 0);
        private static readonly Brush BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(BACKGROUND_HOVER_LIGHT);
        private static readonly Color BACKGROUND_HOVER_DARK = Color.FromArgb(20, 255, 255, 255);
        private static readonly Brush BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(BACKGROUND_HOVER_DARK);
        private static readonly Color BACKGROUND_HOVER_RED = Color.FromArgb(255, 255, 0, 0);
        private static readonly Brush BACKGROUND_HOVER_RED_BRUSH = new SolidBrush(BACKGROUND_HOVER_RED);
        private static readonly Color BACKGROUND_DOWN_RED = Color.FromArgb(255, 255, 84, 54);
        private static readonly Brush BACKGROUND_DOWN_RED_BRUSH = new SolidBrush(BACKGROUND_DOWN_RED);
        private static readonly Color BACKGROUND_FOCUS_LIGHT = Color.FromArgb(30, 0, 0, 0);
        private static readonly Brush BACKGROUND_FOCUS_LIGHT_BRUSH = new SolidBrush(BACKGROUND_FOCUS_LIGHT);
        private static readonly Color BACKGROUND_FOCUS_DARK = Color.FromArgb(80, 140, 140, 255);
        private static readonly Brush BACKGROUND_FOCUS_DARK_BRUSH = new SolidBrush(BACKGROUND_FOCUS_DARK);
        private static readonly Color BACKGROUND_DISABLED_LIGHT = Color.FromArgb(25, 0, 0, 0);
        private static readonly Brush BACKGROUND_DISABLED_LIGHT_BRUSH = new SolidBrush(BACKGROUND_DISABLED_LIGHT);
        private static readonly Color BACKGROUND_DISABLED_DARK = Color.FromArgb(25, 255, 255, 255);
        private static readonly Brush BACKGROUND_DISABLED_DARK_BRUSH = new SolidBrush(BACKGROUND_DISABLED_DARK);

        //Expansion Panel colors
        private static readonly Color EXPANSIONPANEL_FOCUS_LIGHT = Color.FromArgb(255, 242, 242, 242);
        private static readonly Brush EXPANSIONPANEL_FOCUS_LIGHT_BRUSH = new SolidBrush(EXPANSIONPANEL_FOCUS_LIGHT);
        private static readonly Color EXPANSIONPANEL_FOCUS_DARK = Color.FromArgb(255, 50, 50, 50);
        private static readonly Brush EXPANSIONPANEL_FOCUS_DARK_BRUSH = new SolidBrush(EXPANSIONPANEL_FOCUS_DARK);

        // Backdrop colors - for containers, like forms or panels
        private static readonly Color BACKDROP_LIGHT = Color.FromArgb(255, 242, 242, 242);
        private static readonly Brush BACKDROP_LIGHT_BRUSH = new SolidBrush(BACKGROUND_LIGHT);
        private static readonly Color BACKDROP_DARK = Color.FromArgb(255, 50, 50, 50);
        private static readonly Brush BACKDROP_DARK_BRUSH = new SolidBrush(BACKGROUND_DARK);

        //Other colors
        private static readonly Color CARD_BLACK = Color.FromArgb(255, 42, 42, 42);
        private static readonly Color CARD_WHITE = Color.White;

        // Getters - Using these makes handling the dark theme switching easier
        // Text
        public Color TextHighEmphasisColor => Theme == Themes.LIGHT ? TEXT_HIGH_EMPHASIS_DARK : TEXT_HIGH_EMPHASIS_LIGHT;
        public Brush TextHighEmphasisBrush => Theme == Themes.LIGHT ? TEXT_HIGH_EMPHASIS_DARK_BRUSH : TEXT_HIGH_EMPHASIS_LIGHT_BRUSH;
        public Color TextHighEmphasisNoAlphaColor => Theme == Themes.LIGHT ? TEXT_HIGH_EMPHASIS_DARK_NOALPHA : TEXT_HIGH_EMPHASIS_LIGHT_NOALPHA;
        public Brush TextHighEmphasisNoAlphaBrush => Theme == Themes.LIGHT ? TEXT_HIGH_EMPHASIS_DARK_NOALPHA_BRUSH : TEXT_HIGH_EMPHASIS_LIGHT_NOALPHA_BRUSH;
        public Color TextMediumEmphasisColor => Theme == Themes.LIGHT ? TEXT_MEDIUM_EMPHASIS_DARK : TEXT_MEDIUM_EMPHASIS_LIGHT;
        public Brush TextMediumEmphasisBrush => Theme == Themes.LIGHT ? TEXT_MEDIUM_EMPHASIS_DARK_BRUSH : TEXT_MEDIUM_EMPHASIS_LIGHT_BRUSH;
        public Color TextDisabledOrHintColor => Theme == Themes.LIGHT ? TEXT_DISABLED_OR_HINT_DARK : TEXT_DISABLED_OR_HINT_LIGHT;
        public Brush TextDisabledOrHintBrush => Theme == Themes.LIGHT ? TEXT_DISABLED_OR_HINT_DARK_BRUSH : TEXT_DISABLED_OR_HINT_LIGHT_BRUSH;

        // Divider
        public Color DividersColor => Theme == Themes.LIGHT ? DIVIDERS_DARK : DIVIDERS_LIGHT;
        public Brush DividersBrush => Theme == Themes.LIGHT ? DIVIDERS_DARK_BRUSH : DIVIDERS_LIGHT_BRUSH;
        public Color DividersAlternativeColor => Theme == Themes.LIGHT ? DIVIDERS_ALTERNATIVE_DARK : DIVIDERS_ALTERNATIVE_LIGHT;
        public Brush DividersAlternativeBrush => Theme == Themes.LIGHT ? DIVIDERS_ALTERNATIVE_DARK_BRUSH : DIVIDERS_ALTERNATIVE_LIGHT_BRUSH;

        // Checkbox / Radio / Switch
        public Color CheckboxOffColor => Theme == Themes.LIGHT ? CHECKBOX_OFF_LIGHT : CHECKBOX_OFF_DARK;
        public Brush CheckboxOffBrush => Theme == Themes.LIGHT ? CHECKBOX_OFF_LIGHT_BRUSH : CHECKBOX_OFF_DARK_BRUSH;
        public Color CheckBoxOffDisabledColor => Theme == Themes.LIGHT ? CHECKBOX_OFF_DISABLED_LIGHT : CHECKBOX_OFF_DISABLED_DARK;
        public Brush CheckBoxOffDisabledBrush => Theme == Themes.LIGHT ? CHECKBOX_OFF_DISABLED_LIGHT_BRUSH : CHECKBOX_OFF_DISABLED_DARK_BRUSH;

        // Switch
        public Color SwitchOffColor => Theme == Themes.LIGHT ? CHECKBOX_OFF_DARK : CHECKBOX_OFF_LIGHT; // yes, I re-use the checkbox color, sue me
        public Color SwitchOffThumbColor => Theme == Themes.LIGHT ? SWITCH_OFF_THUMB_LIGHT : SWITCH_OFF_THUMB_DARK;
        public Color SwitchOffTrackColor => Theme == Themes.LIGHT ? SWITCH_OFF_TRACK_LIGHT : SWITCH_OFF_TRACK_DARK;
        public Color SwitchOffDisabledThumbColor => Theme == Themes.LIGHT ? SWITCH_OFF_DISABLED_THUMB_LIGHT : SWITCH_OFF_DISABLED_THUMB_DARK;

        // Control Back colors
        public Color BackgroundColor => Theme == Themes.LIGHT ? BACKGROUND_LIGHT : BACKGROUND_DARK;
        public Brush BackgroundBrush => Theme == Themes.LIGHT ? BACKGROUND_LIGHT_BRUSH : BACKGROUND_DARK_BRUSH;
        public Color BackgroundAlternativeColor => Theme == Themes.LIGHT ? BACKGROUND_ALTERNATIVE_LIGHT : BACKGROUND_ALTERNATIVE_DARK;
        public Brush BackgroundAlternativeBrush => Theme == Themes.LIGHT ? BACKGROUND_ALTERNATIVE_LIGHT_BRUSH : BACKGROUND_ALTERNATIVE_DARK_BRUSH;
        public Color BackgroundDisabledColor => Theme == Themes.LIGHT ? BACKGROUND_DISABLED_LIGHT : BACKGROUND_DISABLED_DARK;
        public Brush BackgroundDisabledBrush => Theme == Themes.LIGHT ? BACKGROUND_DISABLED_LIGHT_BRUSH : BACKGROUND_DISABLED_DARK_BRUSH;
        public Color BackgroundHoverColor => Theme == Themes.LIGHT ? BACKGROUND_HOVER_LIGHT : BACKGROUND_HOVER_DARK;
        public Brush BackgroundHoverBrush => Theme == Themes.LIGHT ? BACKGROUND_HOVER_LIGHT_BRUSH : BACKGROUND_HOVER_DARK_BRUSH;
        public Color BackgroundHoverRedColor => Theme == Themes.LIGHT ? BACKGROUND_HOVER_RED : BACKGROUND_HOVER_RED;
        public Brush BackgroundHoverRedBrush => Theme == Themes.LIGHT ? BACKGROUND_HOVER_RED_BRUSH : BACKGROUND_HOVER_RED_BRUSH;
        public Brush BackgroundDownRedBrush => Theme == Themes.LIGHT ? BACKGROUND_DOWN_RED_BRUSH : BACKGROUND_DOWN_RED_BRUSH;
        public Color BackgroundFocusColor => Theme == Themes.LIGHT ? BACKGROUND_FOCUS_LIGHT : BACKGROUND_FOCUS_DARK;
        public Brush BackgroundFocusBrush => Theme == Themes.LIGHT ? BACKGROUND_FOCUS_LIGHT_BRUSH : BACKGROUND_FOCUS_DARK_BRUSH;


        // Other color
        public Color CardsColor => Theme == Themes.LIGHT ? CARD_WHITE : CARD_BLACK;

        // Expansion Panel color/brush
        public Brush ExpansionPanelFocusBrush => Theme == Themes.LIGHT ? EXPANSIONPANEL_FOCUS_LIGHT_BRUSH : EXPANSIONPANEL_FOCUS_DARK_BRUSH;

        // SnackBar
        public Color SnackBarTextHighEmphasisColor => Theme != Themes.LIGHT ? TEXT_HIGH_EMPHASIS_DARK : TEXT_HIGH_EMPHASIS_LIGHT;
        public Color SnackBarBackgroundColor => Theme != Themes.LIGHT ? BACKGROUND_LIGHT : BACKGROUND_DARK;
        public Color SnackBarTextButtonNoAccentTextColor => Theme != Themes.LIGHT ? ColorScheme.PrimaryColor : ColorScheme.LightPrimaryColor;

        // Backdrop color
        public Color BackdropColor => Theme == Themes.LIGHT ? BACKDROP_LIGHT : BACKDROP_DARK;
        public Brush BackdropBrush => Theme == Themes.LIGHT ? BACKDROP_LIGHT_BRUSH : BACKDROP_DARK_BRUSH;

        //
        public Color PropertyGridLineColor => Theme == Themes.LIGHT ? Color.LightGray : Color.FromArgb(0xff, 80, 80, 80);
        public Color TextDisabledColor => Theme == Themes.LIGHT ? Color.Gray : Color.Gray;
        public Color TreeViewLineColor => Theme == Themes.LIGHT ? Color.DarkGray : Color.Gray;

        #endregion
        //------------------------------------------------------------------------------------------------------------------------------------------
        #region Fonts

        // Font Handling
        public enum FontType
        {
            H1,
            H2,
            H3,
            H4,
            H5,
            H6,
            Subtitle1,
            Subtitle2,
            SubtleEmphasis,
            Body1,
            Body2,
            Button,
            Caption,
            Overline
        }

        private const string FONT_NAME = "Microsoft YaHei UI";
        private const string FONT_NAME_LIGHT = "Microsoft YaHei UI";
        private const string FONT_NAME_MEDIUM = "Microsoft YaHei UI";
        private const string FONT_NAME_REGULAR = "Microsoft YaHei UI";
        private const string FONT_NAME_BOLD = "Microsoft YaHei UI";

        private void InitFonts()
        {
            // Create and cache Roboto fonts
            // Thanks https://www.codeproject.com/Articles/42041/How-to-Use-a-Font-Without-Installing-it
            // And https://www.codeproject.com/Articles/107376/Embedding-Font-To-Resources

            // Add font to system table in memory and save the font family
            //             addFont(Resources.Roboto_Thin);
            //             addFont(Resources.Roboto_Light);
            //             addFont(Resources.Roboto_Regular);
            //             addFont(Resources.Roboto_Medium);
            //             addFont(Resources.Roboto_Bold);
            //             addFont(Resources.Roboto_Black);
            addFont(Resources.msyh);
            addFont(Resources.msyh);
            addFont(Resources.msyhl);
            addFont(Resources.msyhl);
            addFont(Resources.msyhbd);
            addFont(Resources.msyhbd);

            RobotoFontFamilies = new Dictionary<string, FontFamily>();
            foreach (FontFamily ff in privateFontCollection.Families.ToArray())
            {
                RobotoFontFamilies.Add(ff.Name, ff);
            }

            // create and save font handles for GDI
            logicalFonts = new Dictionary<string, IntPtr>(18);
            logicalFonts.Add("H1", createLogicalFont(FONT_NAME_LIGHT, 96, NativeTextRenderer.logFontWeight.FW_LIGHT));
            logicalFonts.Add("H2", createLogicalFont(FONT_NAME_LIGHT, 60, NativeTextRenderer.logFontWeight.FW_LIGHT));
            logicalFonts.Add("H3", createLogicalFont(FONT_NAME, 48, NativeTextRenderer.logFontWeight.FW_REGULAR));
            logicalFonts.Add("H4", createLogicalFont(FONT_NAME, 34, NativeTextRenderer.logFontWeight.FW_REGULAR));
            logicalFonts.Add("H5", createLogicalFont(FONT_NAME, 24, NativeTextRenderer.logFontWeight.FW_REGULAR));
            logicalFonts.Add("H6", createLogicalFont(FONT_NAME_MEDIUM, 20, NativeTextRenderer.logFontWeight.FW_MEDIUM));
            logicalFonts.Add("Subtitle1", createLogicalFont(FONT_NAME, 16, NativeTextRenderer.logFontWeight.FW_REGULAR));
            logicalFonts.Add("Subtitle2", createLogicalFont(FONT_NAME_MEDIUM, 14, NativeTextRenderer.logFontWeight.FW_MEDIUM));
            logicalFonts.Add("SubtleEmphasis", createLogicalFont(FONT_NAME, 12, NativeTextRenderer.logFontWeight.FW_NORMAL, 1));
            logicalFonts.Add("Body1", createLogicalFont(FONT_NAME, 16, NativeTextRenderer.logFontWeight.FW_REGULAR));
            logicalFonts.Add("Body2", createLogicalFont(FONT_NAME, 14, NativeTextRenderer.logFontWeight.FW_REGULAR));
            logicalFonts.Add("Button", createLogicalFont(FONT_NAME_MEDIUM, 14, NativeTextRenderer.logFontWeight.FW_MEDIUM));
            logicalFonts.Add("Caption", createLogicalFont(FONT_NAME, 12, NativeTextRenderer.logFontWeight.FW_REGULAR));
            logicalFonts.Add("Overline", createLogicalFont(FONT_NAME, 10, NativeTextRenderer.logFontWeight.FW_REGULAR));
            // Logical fonts for textbox animation
            logicalFonts.Add("textBox16", createLogicalFont(FONT_NAME, 16, NativeTextRenderer.logFontWeight.FW_REGULAR));
            logicalFonts.Add("textBox15", createLogicalFont(FONT_NAME, 15, NativeTextRenderer.logFontWeight.FW_REGULAR));
            logicalFonts.Add("textBox14", createLogicalFont(FONT_NAME, 14, NativeTextRenderer.logFontWeight.FW_REGULAR));
            logicalFonts.Add("textBox13", createLogicalFont(FONT_NAME_MEDIUM, 13, NativeTextRenderer.logFontWeight.FW_MEDIUM));
            logicalFonts.Add("textBox12", createLogicalFont(FONT_NAME_MEDIUM, 12, NativeTextRenderer.logFontWeight.FW_MEDIUM));


            //             logicalFonts.Add("H1", createLogicalFont("Roboto Light", 96, NativeTextRenderer.logFontWeight.FW_LIGHT));
            //             logicalFonts.Add("H2", createLogicalFont("Roboto Light", 60, NativeTextRenderer.logFontWeight.FW_LIGHT));
            //             logicalFonts.Add("H3", createLogicalFont("Roboto", 48, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             logicalFonts.Add("H4", createLogicalFont("Roboto", 34, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             logicalFonts.Add("H5", createLogicalFont("Roboto", 24, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             logicalFonts.Add("H6", createLogicalFont("Roboto Medium", 20, NativeTextRenderer.logFontWeight.FW_MEDIUM));
            //             logicalFonts.Add("Subtitle1", createLogicalFont("Roboto", 16, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             logicalFonts.Add("Subtitle2", createLogicalFont("Roboto Medium", 14, NativeTextRenderer.logFontWeight.FW_MEDIUM));
            //             logicalFonts.Add("SubtleEmphasis", createLogicalFont("Roboto", 12, NativeTextRenderer.logFontWeight.FW_NORMAL, 1));
            //             logicalFonts.Add("Body1", createLogicalFont("Roboto", 16, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             logicalFonts.Add("Body2", createLogicalFont("Roboto", 14, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             logicalFonts.Add("Button", createLogicalFont("Roboto Medium", 14, NativeTextRenderer.logFontWeight.FW_MEDIUM));
            //             logicalFonts.Add("Caption", createLogicalFont("Roboto", 12, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             logicalFonts.Add("Overline", createLogicalFont("Roboto", 10, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             // Logical fonts for textbox animation
            //             logicalFonts.Add("textBox16", createLogicalFont("Roboto", 16, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             logicalFonts.Add("textBox15", createLogicalFont("Roboto", 15, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             logicalFonts.Add("textBox14", createLogicalFont("Roboto", 14, NativeTextRenderer.logFontWeight.FW_REGULAR));
            //             logicalFonts.Add("textBox13", createLogicalFont("Roboto Medium", 13, NativeTextRenderer.logFontWeight.FW_MEDIUM));
            //             logicalFonts.Add("textBox12", createLogicalFont("Roboto Medium", 12, NativeTextRenderer.logFontWeight.FW_MEDIUM));
        }
        public Font getFontByType(FontType type)
        {
            switch (type)
            {
                case FontType.H1:
                    return new Font(RobotoFontFamilies[FONT_NAME_LIGHT], 96f, FontStyle.Regular, GraphicsUnit.Pixel);

                case FontType.H2:
                    return new Font(RobotoFontFamilies[FONT_NAME_LIGHT], 60f, FontStyle.Regular, GraphicsUnit.Pixel);

                case FontType.H3:
                    return new Font(RobotoFontFamilies[FONT_NAME], 48f, FontStyle.Bold, GraphicsUnit.Pixel);

                case FontType.H4:
                    return new Font(RobotoFontFamilies[FONT_NAME], 34f, FontStyle.Bold, GraphicsUnit.Pixel);

                case FontType.H5:
                    return new Font(RobotoFontFamilies[FONT_NAME], 24f, FontStyle.Bold, GraphicsUnit.Pixel);

                case FontType.H6:
                    return new Font(RobotoFontFamilies[FONT_NAME_MEDIUM], 20f, FontStyle.Bold, GraphicsUnit.Pixel);

                case FontType.Subtitle1:
                    return new Font(RobotoFontFamilies[FONT_NAME], 16f, FontStyle.Regular, GraphicsUnit.Pixel);

                case FontType.Subtitle2:
                    return new Font(RobotoFontFamilies[FONT_NAME_MEDIUM], 14f, FontStyle.Bold, GraphicsUnit.Pixel);

                case FontType.SubtleEmphasis:
                    return new Font(RobotoFontFamilies[FONT_NAME], 12f, FontStyle.Italic, GraphicsUnit.Pixel);

                case FontType.Body1:
                    return new Font(RobotoFontFamilies[FONT_NAME], 14f, FontStyle.Regular, GraphicsUnit.Pixel);

                case FontType.Body2:
                    return new Font(RobotoFontFamilies[FONT_NAME], 12f, FontStyle.Regular, GraphicsUnit.Pixel);

                case FontType.Button:
                    return new Font(RobotoFontFamilies[FONT_NAME], 14f, FontStyle.Bold, GraphicsUnit.Pixel);

                case FontType.Caption:
                    return new Font(RobotoFontFamilies[FONT_NAME], 12f, FontStyle.Regular, GraphicsUnit.Pixel);

                case FontType.Overline:
                    return new Font(RobotoFontFamilies[FONT_NAME], 10f, FontStyle.Regular, GraphicsUnit.Pixel);
            }
            return new Font(RobotoFontFamilies[FONT_NAME], 14f, FontStyle.Regular, GraphicsUnit.Pixel);
        }
        //         public Font getFontByType(fontType type)
        //         {
        //             switch (type)
        //             {
        //                 case fontType.H1:
        //                     return new Font(RobotoFontFamilies["Roboto_Light"], 96f, FontStyle.Regular, GraphicsUnit.Pixel);
        // 
        //                 case fontType.H2:
        //                     return new Font(RobotoFontFamilies["Roboto_Light"], 60f, FontStyle.Regular, GraphicsUnit.Pixel);
        // 
        //                 case fontType.H3:
        //                     return new Font(RobotoFontFamilies["Roboto"], 48f, FontStyle.Bold, GraphicsUnit.Pixel);
        // 
        //                 case fontType.H4:
        //                     return new Font(RobotoFontFamilies["Roboto"], 34f, FontStyle.Bold, GraphicsUnit.Pixel);
        // 
        //                 case fontType.H5:
        //                     return new Font(RobotoFontFamilies["Roboto"], 24f, FontStyle.Bold, GraphicsUnit.Pixel);
        // 
        //                 case fontType.H6:
        //                     return new Font(RobotoFontFamilies["Roboto_Medium"], 20f, FontStyle.Bold, GraphicsUnit.Pixel);
        // 
        //                 case fontType.Subtitle1:
        //                     return new Font(RobotoFontFamilies["Roboto"], 16f, FontStyle.Regular, GraphicsUnit.Pixel);
        // 
        //                 case fontType.Subtitle2:
        //                     return new Font(RobotoFontFamilies["Roboto_Medium"], 14f, FontStyle.Bold, GraphicsUnit.Pixel);
        // 
        //                 case fontType.SubtleEmphasis:
        //                     return new Font(RobotoFontFamilies["Roboto"], 12f, FontStyle.Italic, GraphicsUnit.Pixel);
        // 
        //                 case fontType.Body1:
        //                     return new Font(RobotoFontFamilies["Roboto"], 14f, FontStyle.Regular, GraphicsUnit.Pixel);
        // 
        //                 case fontType.Body2:
        //                     return new Font(RobotoFontFamilies["Roboto"], 12f, FontStyle.Regular, GraphicsUnit.Pixel);
        // 
        //                 case fontType.Button:
        //                     return new Font(RobotoFontFamilies["Roboto"], 14f, FontStyle.Bold, GraphicsUnit.Pixel);
        // 
        //                 case fontType.Caption:
        //                     return new Font(RobotoFontFamilies["Roboto"], 12f, FontStyle.Regular, GraphicsUnit.Pixel);
        // 
        //                 case fontType.Overline:
        //                     return new Font(RobotoFontFamilies["Roboto"], 10f, FontStyle.Regular, GraphicsUnit.Pixel);
        //             }
        //             return new Font(RobotoFontFamilies["Roboto"], 14f, FontStyle.Regular, GraphicsUnit.Pixel);
        //         }

        /// <summary>
        /// Get the font by size - used for textbox label animation, try to not use this for anything else
        /// </summary>
        /// <param name="size">font size, ranges from 12 up to 16</param>
        /// <returns></returns>
        public IntPtr getTextBoxFontBySize(int size)
        {
            string name = "textBox" + Math.Min(16, Math.Max(12, size)).ToString();
            return logicalFonts[name];
        }

        /// <summary>
        /// Gets a Material Skin Logical Roboto Font given a standard material font type
        /// </summary>
        /// <param name="type">material design font type</param>
        /// <returns></returns>
        public IntPtr getLogFontByType(FontType type)
        {
            return logicalFonts[Enum.GetName(typeof(FontType), type)];
        }

        // Font stuff
        private Dictionary<string, IntPtr> logicalFonts;

        private Dictionary<string, FontFamily> RobotoFontFamilies;

        private PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        private void addFont(byte[] fontdata)
        {
            // Add font to system table in memory
            int dataLength = fontdata.Length;

            IntPtr ptrFont = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontdata, 0, ptrFont, dataLength);

            // GDI Font
            NativeTextRenderer.AddFontMemResourceEx(fontdata, dataLength, IntPtr.Zero, out _);

            // GDI+ Font
            privateFontCollection.AddMemoryFont(ptrFont, dataLength);
        }

        private IntPtr createLogicalFont(string fontName, int size, NativeTextRenderer.logFontWeight weight, byte lfItalic = 0)
        {
            // Logical font:
            NativeTextRenderer.LogFont lfont = new NativeTextRenderer.LogFont();
            lfont.lfFaceName = fontName;
            lfont.lfHeight = -size;
            lfont.lfWeight = (int)weight;
            lfont.lfItalic = lfItalic;
            return NativeTextRenderer.CreateFontIndirect(lfont);
        }
        #endregion
        //------------------------------------------------------------------------------------------------------------------------------------------
    }

    //     internal class MaterialScrollBarRenderer : ScrollBarRenderer
    //     {
    // 
    //     }
    internal class MaterialToolStripRender : ToolStripProfessionalRenderer, IMaterialControl
    {
        private const int LEFT_PADDING = 26;
        private const int RIGHT_PADDING = 8;

        //Properties for managing the material design properties
        public int Depth { get; set; }
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

        public MouseState MouseState { get; set; }

        internal MaterialToolStripRender() : base(new MaterialProfessionalColorTable()) { }

#if FALSE
   private Rectangle GetItemRect(ToolStripItem item)
        {
            return new Rectangle(0, item.ContentRectangle.Y, item.ContentRectangle.Width, item.ContentRectangle.Height);
        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var itemRect = GetItemRect(e.Item);
            var textRect = new Rectangle(LEFT_PADDING, itemRect.Y, itemRect.Width - (LEFT_PADDING + RIGHT_PADDING), itemRect.Height);
           
            if (e.Item is IG2DBaseComponent g2D && g2D.CustomForeColor.HasValue)
            {
                var fore = g2D.CustomForeColor.Value;
                using (NativeTextRenderer NativeText = new NativeTextRenderer(g))
                {
                    NativeText.DrawTransparentText(e.Text, SkinManager.getLogFontByType(MaterialSkinManager.FontType.Body2),
                        e.Item.Enabled ? fore : SkinManager.TextDisabledOrHintColor,
                        textRect.Location,
                        textRect.Size,
                        NativeTextRenderer.TextAlignFlags.Left | NativeTextRenderer.TextAlignFlags.Middle);
                }
            }
            else
            {
                using (NativeTextRenderer NativeText = new NativeTextRenderer(g))
                {
                    NativeText.DrawTransparentText(e.Text, SkinManager.getLogFontByType(MaterialSkinManager.FontType.Body2),
                        e.Item.Enabled ? SkinManager.TextHighEmphasisColor : SkinManager.TextDisabledOrHintColor,
                        textRect.Location,
                        textRect.Size,
                        NativeTextRenderer.TextAlignFlags.Left | NativeTextRenderer.TextAlignFlags.Middle);
                }
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var g = e.Graphics;
            var itemRect = GetItemRect(e.Item);
            if (e.Item is IG2DBaseComponent g2D && g2D.CustomBackColor.HasValue)
            {
                var back = g2D.CustomBackColor.Value;
                g.Clear(back);
                g.FillRectangle(e.Item.Selected && e.Item.Enabled ? SkinManager.BackgroundFocusBrush : new SolidBrush(back), itemRect);

            }
            else
            {
                g.Clear(SkinManager.BackgroundColor);
                g.FillRectangle(e.Item.Selected && e.Item.Enabled ? SkinManager.BackgroundFocusBrush : SkinManager.BackgroundBrush, itemRect);

            }

            //Ripple animation
            var toolStrip = e.ToolStrip as MaterialContextMenuStrip;
            if (toolStrip != null)
            {
                var animationManager = toolStrip.AnimationManager;
                var animationSource = toolStrip.AnimationSource;
                if (toolStrip.AnimationManager.IsAnimating() && e.Item.Bounds.Contains(animationSource))
                {
                    for (int i = 0; i < animationManager.GetAnimationCount(); i++)
                    {
                        var animationValue = animationManager.GetProgress(i);
                        var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationValue * 50)), Color.Black));
                        var rippleSize = (int)(animationValue * itemRect.Width * 2.5);
                        g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, itemRect.Y - itemRect.Height, rippleSize, itemRect.Height * 3));
                    }
                }
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            var g = e.Graphics;

            g.FillRectangle(SkinManager.BackgroundBrush, e.Item.Bounds);
            g.DrawLine(
                new Pen(SkinManager.DividersColor),
                new Point(e.Item.Bounds.Left, e.Item.Bounds.Height / 2),
                new Point(e.Item.Bounds.Right, e.Item.Bounds.Height / 2));
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            e.ToolStrip.BackColor = SkinManager.BackgroundColor;
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            var g = e.Graphics;
            const int ARROW_SIZE = 4;

            var arrowMiddle = new Point(e.ArrowRectangle.X + e.ArrowRectangle.Width / 2, e.ArrowRectangle.Y + e.ArrowRectangle.Height / 2);
            var arrowBrush = e.Item.Enabled ? SkinManager.TextHighEmphasisBrush : SkinManager.TextDisabledOrHintBrush;
            using (var arrowPath = new GraphicsPath())
            {
                arrowPath.AddLines(
                    new[] {
                        new Point(arrowMiddle.X - ARROW_SIZE, arrowMiddle.Y - ARROW_SIZE),
                        new Point(arrowMiddle.X, arrowMiddle.Y),
                        new Point(arrowMiddle.X - ARROW_SIZE, arrowMiddle.Y + ARROW_SIZE) });
                arrowPath.CloseFigure();

                g.FillPath(arrowBrush, arrowPath);
            }
        }
#else
        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderDropDownButtonBackground(e);
            if (e.Item is ToolStripDropDownItem item && item.Pressed && item.HasDropDownItems)
            {
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);
                if ((bounds.Width == 0) || (bounds.Height == 0))
                {
                    return;  // can't new up a linear gradient brush with no dimension.
                }
                var b = SkinManager.BackdropBrush;
                e.Graphics.FillRectangle(b, bounds);
            }
        }
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
        }
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            //base.OnRenderSeparator(e);
            var g = e.Graphics;
            var bounds = new Rectangle(Point.Empty, e.Item.Size);
            if (e.Vertical)
            {
                g.DrawLine(
                    new Pen(SkinManager.BackgroundColor),
                    new Point(bounds.X, 0),
                    new Point(bounds.X, bounds.Height));
                bounds.X += 1;
                g.DrawLine(
                    new Pen(SkinManager.DividersColor),
                    new Point(bounds.X, 0),
                    new Point(bounds.X, bounds.Height));
            }
            else
            {
                g.DrawLine(
                    new Pen(SkinManager.BackgroundColor),
                    new Point(0, bounds.Y),
                    new Point(bounds.Width, bounds.Y));
                bounds.Y += 1;
                g.DrawLine(
                    new Pen(SkinManager.DividersColor),
                    new Point(0, bounds.Y),
                    new Point(bounds.Width, bounds.Y));
            }
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = SkinManager.TextHighEmphasisColor;
            base.OnRenderArrow(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBorder(e);
            //             var toolStrip = e.ToolStrip;
            //             var g = e.Graphics;
            //             var bounds = new Rectangle(Point.Empty, toolStrip.Size);
            //             bounds.Width -= 1;
            //             bounds.Height -= 1;
            //             g.DrawRectangle(new Pen(SkinManager.DividersColor), bounds);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            base.OnRenderItemCheck(e);
        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            base.OnRenderItemText(e);
        }

#endif
    }

    public class MaterialProfessionalColorTable : ProfessionalColorTable
    {
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public override Color ToolStripBorder => SkinManager.DividersAlternativeColor;
    }
}
