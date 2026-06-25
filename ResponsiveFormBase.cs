using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public class ResponsiveFormBase : Form
    {
        private readonly Dictionary<Control, ControlLayoutSnapshot>
            _layoutSnapshots =
            new Dictionary<Control, ControlLayoutSnapshot>();

        private Size _baseClientSize = Size.Empty;

        private bool _layoutCaptured;

        private bool _isScaling;

        // ENABLE / DISABLE CUSTOM TOP BAR

        public bool EnableWindowChrome { get; set; } = true;

        // ============================================
        // GLOBAL WINDOW STATE
        // ============================================

        public static FormWindowState GlobalWindowState =
            FormWindowState.Normal;

        // ============================================
        // CUSTOM TITLE BAR CONTROLS
        // ============================================

        private Panel topPanel;

        private Button btnMinimize;

        private Button btnMaximize;

        private Button btnClose;

        // ============================================
        // DRAG WINDOW IMPORTS
        // ============================================

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(
            IntPtr hWnd,
            int Msg,
            int wParam,
            int lParam);

        // ============================================
        // FORM LOAD
        // ============================================

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                CaptureLayoutSnapshot();

                ConfigureTopLevelWindow();

                // ONLY MAIN FORMS GET CUSTOM BAR

                if (EnableWindowChrome && TopLevel)
                {
                    CreateCustomTitleBar();
                }
            }

            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (!DesignMode &&
                _layoutCaptured &&
                TopLevel &&
                WindowState != FormWindowState.Minimized)
            {
                ApplyResponsiveLayout();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (!DesignMode &&
                _layoutCaptured &&
                WindowState != FormWindowState.Minimized)
            {
                ApplyResponsiveLayout();
            }
        }

        // ============================================
        // WINDOW SETTINGS
        // ============================================

        private void ConfigureTopLevelWindow()
        {
            if (!TopLevel ||
                DesignMode ||
                !EnableWindowChrome)
            {
                return;
            }

            // REMOVE WHITE WINDOWS TOP BAR

            FormBorderStyle = FormBorderStyle.None;

            // OPEN CENTER

            StartPosition = FormStartPosition.CenterScreen;

            // MINIMUM SIZE

            MinimumSize = new Size(900, 600);

            // SAME STATE FOR ALL DASHBOARDS

            WindowState = GlobalWindowState;

            // BACKGROUND

            BackColor = Color.Black;
        }

        // ============================================
        // CREATE CUSTOM TOP BAR
        // ============================================

        private void CreateCustomTitleBar()
        {
            // PREVENT DOUBLE BAR

            if (topPanel != null)
            {
                return;
            }

            // TOP PANEL

            topPanel = new Panel();

            topPanel.Height = 40;

            topPanel.Dock = DockStyle.Top;

            topPanel.BackColor = Color.Black;

            topPanel.MouseDown += topPanel_MouseDown;

            Controls.Add(topPanel);

            topPanel.BringToFront();

            // ====================================
            // MINIMIZE BUTTON
            // ====================================

            btnMinimize = new Button();

            btnMinimize.Text = "—";

            btnMinimize.ForeColor = Color.White;

            btnMinimize.BackColor = Color.Black;

            btnMinimize.FlatStyle = FlatStyle.Flat;

            btnMinimize.FlatAppearance.BorderSize = 0;

            btnMinimize.Width = 45;

            btnMinimize.Height = 40;

            btnMinimize.Dock = DockStyle.Right;

            btnMinimize.Cursor = Cursors.Hand;

            btnMinimize.Click += btnMinimize_Click;

            topPanel.Controls.Add(btnMinimize);

            // ====================================
            // MAXIMIZE BUTTON
            // ====================================

            btnMaximize = new Button();

            btnMaximize.Text = "□";

            btnMaximize.ForeColor = Color.White;

            btnMaximize.BackColor = Color.Black;

            btnMaximize.FlatStyle = FlatStyle.Flat;

            btnMaximize.FlatAppearance.BorderSize = 0;

            btnMaximize.Width = 45;

            btnMaximize.Height = 40;

            btnMaximize.Dock = DockStyle.Right;

            btnMaximize.Cursor = Cursors.Hand;

            btnMaximize.Click += btnMaximize_Click;

            topPanel.Controls.Add(btnMaximize);

            // ====================================
            // CLOSE BUTTON
            // ====================================

            btnClose = new Button();

            btnClose.Text = "X";

            btnClose.ForeColor = Color.White;

            btnClose.BackColor = Color.Black;

            btnClose.FlatStyle = FlatStyle.Flat;

            btnClose.FlatAppearance.BorderSize = 0;

            btnClose.Width = 45;

            btnClose.Height = 40;

            btnClose.Dock = DockStyle.Right;

            btnClose.Cursor = Cursors.Hand;

            btnClose.Click += btnClose_Click;

            topPanel.Controls.Add(btnClose);
        }

        // ============================================
        // BUTTON EVENTS
        // ============================================

        private void btnMinimize_Click(
            object sender,
            EventArgs e)
        {
            WindowState =
                FormWindowState.Minimized;
        }

        private void btnMaximize_Click(
            object sender,
            EventArgs e)
        {
            if (WindowState ==
                FormWindowState.Maximized)
            {
                WindowState =
                    FormWindowState.Normal;
            }
            else
            {
                WindowState =
                    FormWindowState.Maximized;
            }

            // SAVE GLOBAL STATE

            GlobalWindowState = WindowState;
        }

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            Application.Exit();
        }

        // ============================================
        // DRAG WINDOW
        // ============================================

        private void topPanel_MouseDown(
            object sender,
            MouseEventArgs e)
        {
            ReleaseCapture();

            SendMessage(
                Handle,
                0x112,
                0xf012,
                0);
        }

        // ============================================
        // SAVE ORIGINAL CONTROL SIZES
        // ============================================

        private void CaptureLayoutSnapshot()
        {
            _baseClientSize = ClientSize;

            _layoutSnapshots.Clear();

            foreach (Control control in GetAllControls(this))
            {
                if (control == this)
                {
                    continue;
                }

                _layoutSnapshots[control] =
                    new ControlLayoutSnapshot
                    {
                        Bounds = control.Bounds,

                        FontSize = control.Font.Size,

                        Dock = control.Dock
                    };
            }

            _layoutCaptured = true;
        }

        // ============================================
        // RESPONSIVE RESIZE
        // ============================================

        private void ApplyResponsiveLayout()
        {
            if (!_layoutCaptured ||
                _baseClientSize.Width <= 0 ||
                _baseClientSize.Height <= 0)
            {
                return;
            }

            if (_isScaling)
            {
                return;
            }

            _isScaling = true;

            try
            {
                float scaleX =
                    (float)ClientSize.Width /
                    _baseClientSize.Width;

                float scaleY =
                    (float)ClientSize.Height /
                    _baseClientSize.Height;

                float fontScale =
                    Math.Min(scaleX, scaleY);

                foreach (KeyValuePair<Control,
                    ControlLayoutSnapshot> entry
                    in _layoutSnapshots)
                {
                    Control control = entry.Key;

                    ControlLayoutSnapshot snapshot =
                        entry.Value;

                    if (control == null ||
                        control.IsDisposed)
                    {
                        continue;
                    }

                    // SKIP DOCKED CONTROLS

                    if (snapshot.Dock != DockStyle.None)
                    {
                        continue;
                    }

                    Rectangle scaledBounds =
                        new Rectangle(
                            (int)(snapshot.Bounds.X * scaleX),

                            (int)(snapshot.Bounds.Y * scaleY),

                            Math.Max(
                                1,
                                (int)(snapshot.Bounds.Width * scaleX)),

                            Math.Max(
                                1,
                                (int)(snapshot.Bounds.Height * scaleY)));

                    control.Bounds = scaledBounds;

                    float scaledFontSize =
                        snapshot.FontSize * fontScale;

                    if (scaledFontSize >= 7f &&
                        scaledFontSize <= 48f)
                    {
                        control.Font =
                            new Font(
                                control.Font.FontFamily,
                                scaledFontSize,
                                control.Font.Style);
                    }
                }
            }
            finally
            {
                _isScaling = false;
            }
        }

        // ============================================
        // GET ALL CONTROLS
        // ============================================

        private static IEnumerable<Control>
            GetAllControls(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                yield return child;

                foreach (Control nested
                    in GetAllControls(child))
                {
                    yield return nested;
                }
            }
        }

        // ============================================
        // SNAPSHOT CLASS
        // ============================================

        private sealed class ControlLayoutSnapshot
        {
            public Rectangle Bounds { get; set; }

            public float FontSize { get; set; }

            public DockStyle Dock { get; set; }
        }

        // ============================================
        // DESIGNER
        // ============================================

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 
            // ResponsiveFormBase
            // 

            this.ClientSize =
                new System.Drawing.Size(278, 244);

            this.Name =
                "ResponsiveFormBase";

            this.Load +=
                new System.EventHandler(
                    this.ResponsiveFormBase_Load);

            this.ResumeLayout(false);
        }

        private void ResponsiveFormBase_Load(
            object sender,
            EventArgs e)
        {

        }
    }
}