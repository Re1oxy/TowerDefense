using System;
using System.Collections.Generic;
using System.Drawing;
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
    }


}
