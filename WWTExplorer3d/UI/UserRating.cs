using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TerraViewer.Properties;

namespace TerraViewer
{
    [DefaultEvent("ValueChanged")]

    public partial class UserRating : UserControl
    {
        public UserRating()
        {
            InitializeComponent();
        }

        public event EventHandler ValueChanged;
        public enum Interactivity { ReadOnly, ReadWrite };

        Interactivity mode = Interactivity.ReadOnly;

        public Interactivity Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public enum StarSizes { Big, Small };
        StarSizes starSize = StarSizes.Big;

        public StarSizes StarSize
        {
            get { return starSize; }
            set
            {
                starSize = value;
                SetStarSize();
                Refresh();
            }
        }

        double stars = 2.5;

        public double Stars
        {
            get { return stars; }
            set
            {
                stars = value;
                Refresh();
            }
        }

        private void UserRating_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (starSize == StarSizes.Big)
            {
                var rect = new Rectangle(0, 0, (int)(stars * 25.4), 24);
                e.Graphics.DrawImageUnscaled(Resources.StarRatingBackground, 0, 0);
                e.Graphics.DrawImage(Resources.StarRatingForeground, rect, rect, GraphicsUnit.Pixel);
            }
            else
            {
                var rect = new Rectangle(0, 0, (int)(stars * 14), 24);
                e.Graphics.DrawImageUnscaled(Resources.StarRatingBackgroundSmall, 0, 0);
                e.Graphics.DrawImage(Resources.StarRatingForegroundSmall, rect, rect, GraphicsUnit.Pixel);
            }
        }

        private void UserRating_Load(object sender, EventArgs e)
        {
            SetStarSize();
        }

        private void SetStarSize()
        {
            if (starSize == StarSizes.Big)
            {
                Height = 24;
                Width = 128;
            }
            else
            {
                Width = 72;
                Height = 16;
            }
            Invalidate();
        }
        bool down;
        private void UserRating_MouseDown(object sender, MouseEventArgs e)
        {
            down = true;
            UpsateStarCount(e.X);
        }

        private void UpsateStarCount(int xPos)
        {
            if (mode == Interactivity.ReadWrite)
            {
                var blockSize = starSize == StarSizes.Big ? 25.4 : 14;

                var count = Math.Min(5, Math.Max(0, ((int)(xPos / blockSize) + 1)));

                stars = count;
                if (ValueChanged != null)
                {
                    ValueChanged.Invoke(this, new EventArgs());
                }
                Refresh();
            }
        }

        private void UserRating_MouseUp(object sender, MouseEventArgs e)
        {
            down = false;

        }

        private void UserRating_MouseMove(object sender, MouseEventArgs e)
        {
            if (down)
            {
                UpsateStarCount(e.X);
            }   
        }
    }
}
