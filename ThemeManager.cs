using System.Drawing;
using System.Drawing.Drawing2D;

namespace MunicipalServicesApp
{
    public static class ThemeManager
    {
        //COLOR PALETTE
        public static readonly Color EmeraldDark = Color.FromArgb(27, 94, 32);   
        public static readonly Color EmeraldMid = Color.FromArgb(46, 125, 50);   
        public static readonly Color EmeraldLight = Color.FromArgb(56, 142, 60); 
        public static readonly Color BackgroundLight = Color.FromArgb(239, 242, 239);
        public static readonly Color CardWhite = Color.White;
        public static readonly Color TextDark = Color.FromArgb(34, 51, 34);
        public static readonly Color MutedGrey = Color.FromArgb(120, 130, 120);
        public static readonly Color Shadow = Color.FromArgb(210, 220, 210);

        //GRADIENT HEADER
        public static void DrawHeaderGradient(Graphics g, Rectangle rect)
        {
            using var brush = new LinearGradientBrush(rect, EmeraldMid, EmeraldDark, LinearGradientMode.Vertical);
            g.FillRectangle(brush, rect);
        }

        //CARD SHADOW EFFECT
        public static void DrawCardShadow(Graphics g, Rectangle rect)
        {
            var shadowRect = new Rectangle(rect.X + 3, rect.Y + 3, rect.Width, rect.Height);
            using var shadow = new SolidBrush(Color.FromArgb(40, Shadow));
            g.FillRectangle(shadow, shadowRect);
        }
    }
}
