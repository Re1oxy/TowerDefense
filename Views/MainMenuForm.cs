using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TowerDefense.Views
{
    /// <summary>
    /// Start screen show when application launches. It contains buttons to star
    /// </summary>
    public class MainMenuForm : Form
    {
        private Button _btnPlay;
        private Button _btnQuit;
        private System.Windows.Forms.Timer _animTimer;
        private float _animOffset = 0f;

        public MainMenuForm()
        {
            InitializeForm();
            InitializeControls();
            InitializeAnimation();
        }

        private void InitializeForm()
        {
            Text = "Tower Defense";
            ClientSize = new Size(Utils.Constants.WindowWidth, Utils.Constants.WindowHeight);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(30, 30, 30);
            DoubleBuffered = true;
        }

        private void InitializeControls()
        {
            // Play Button
            _btnPlay = CreateButton("PLAY", new Point(ClientSize.Width / 2 - 120, 340));
            _btnPlay.BackColor = Color.FromArgb(60, 140, 60);
            _btnPlay.Click += (s, e) =>
            {
                var gameForm = new GameForm();
                gameForm.FormClosed += (gs, ge) => Show();
                Hide();
                gameForm.Show();
            };

            // Quit Button
            _btnQuit = CreateButton("QUIT", new Point(ClientSize.Width / 2 - 120, 420));
            _btnQuit.BackColor = Color.FromArgb(140, 50, 50);
            _btnQuit.Click += (s, e) => Application.Exit();

            Controls.Add(_btnPlay);
            Controls.Add(_btnQuit);
        }

        // Create Button
        private Button CreateButton(string text, Point location)
        {
            return new Button
            {
                Text = text,
                Size = new Size(240, 55),
                Location = location,
                Font = new Font(Utils.Constants.FontName, 14, FontStyle.Bold),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        private void InitializeAnimation()
        {
            _animTimer = new System.Windows.Forms.Timer() { Interval = 16 };
            _animTimer.Tick += (s, e) =>
            {
                _animOffset += 0.02f;
                Invalidate();
            };

            _animTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawBackground(g);
            DrawTitle(g);
            DrawSubtitle(g);
            DrawHing(g);
        }

        private void DrawBackground(Graphics g)
        {
            // Animated grass grid
            using var darkBrush = new SolidBrush(Color.FromArgb(25, 40, 25));
            using var lightBrush = new SolidBrush(Color.FromArgb(35, 55, 35));

            int cell = 48;
            for (int x = 0; x < ClientSize.Width; x += cell)
                for (int y = 0; y < ClientSize.Height; y += cell)
                {
                    bool checker = ((x / cell) + (y / cell)) % 2 == 0;
                    g.FillRectangle(checker ? darkBrush : lightBrush,
                        x, y, cell, cell);
                }

            // Dark overlay
            using var vignette = new LinearGradientBrush(
                new Rectangle (0,0, ClientSize.Width, ClientSize.Height),
                Color.FromArgb(160, 0, 0, 0),
                Color.FromArgb(60, 0, 0, 0),
                LinearGradientMode.Vertical);
            g.FillRectangle(vignette, 0, 0, ClientSize.Width, ClientSize.Height);
        }

        private void DrawTitle(Graphics g)
        {
            string title = "TOWER DEFENSE";
            using var font = new Font(Utils.Constants.FontName, 52, FontStyle.Bold);

            // Shadow
            using var shadowBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0));
            g.DrawString(title, font, shadow,
                ClientSize.Width / 2f - 248 -2, 120 - 2);

            // Gradient text effect
            using var glowBrush = new SolidBrush(Color.FromArgb(40, 100, 220, 50));
            g.DrawString(title, font, glowBrush,
                ClientSize.Width / 2f - 248 -2, 120 - 2);

            using var mainBrush = new SolidBrush(Color.FromArgb(180, 230, 160));
            g.DrawString(title, font, mainBrush,
                ClientSize.Width / 2f - 248,
                120);
        }

        private void DrawSubtitle(Graphics g)
        {
            string sub = "Defend your castle against 10 waves of enemies!";
            using var font = new Font(Utils.Constants.FontName, 13, FontStyle.Italic);
            using var brush = new SolidBrush(Color.FromArgb(180, 200, 220, 200));
            var size = g.MeasureString(sub, font);
            g.DrawString(sub, font, brush,
                (ClientSize.Width - size.Width) / 2f,
                240);
        }

        // Draw hint text at the bottom ( With emotions using Win + . )
        private void DrawHint(Graphics g)
        {
            string[] lines =
            {
                "🏹  Archer  –  50g   |   fast, low damage",
                "🧙  Mage    –  100g  |   slow, high damage",
                "⚙️  Catapult – 150g  |   massive AoE damage",
            };

            using var font = new Font(Utils.Constants.FontName, 10);
            using var brush = new SolidBrush(Color.FromArgb(140, 200, 200, 200));
            int y = ClientSize.Height - 100;
            foreach (var line in lines)
            {
                var size = g.MeasureString(line, font);
                g.DrawString(line, font, brush,
                    (ClientSize.Width - size.Width) / 2f, y);
                y += 22;
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _animTimer?.Stop();
            base.OnFormClosed(e);
        }
    }
}
