using System;
using CoreGraphics;
using UIKit;

namespace SlidingTilesApp.Logic
{
    public class GameTile : UIImageView
    {
        public GameTile(int row, int col, string value)
        {
            Row = row;
            Col = col;
            Value = value;
        }

        public float Width { get; set; }
        public float Height { get; set; }
        public float CenterX => Width / 2 + (Col * Width);
        public float CenterY => Height / 2 + (Row * Height);

        public int Row { get; set; }
        public int Col { get; set; }
        public string Value { get; set; }

        public CGPoint GetCoords()
        {
            return new CGPoint(CenterX, CenterY);
        }

        public UIImage DrawTile(UIImage image, UIColor textColor, int fontSize)
        {
            Frame = new CGRect(0, 0, Width, Height);
            Center = new CGPoint(CenterX, CenterY);
            UserInteractionEnabled = true;
            Image = image;

            nfloat width = image.Size.Width;
            nfloat height = image.Size.Height;

            using (CGBitmapContext ctx = new CGBitmapContext(IntPtr.Zero, (nint)width, (nint)height, 8, 4 * (nint)width, CGColorSpace.CreateDeviceRGB(), CGBitmapFlags.PremultipliedFirst))
            {
                if (IsEmpty())
                {
                    return UIImage.FromImage(ctx.ToImage());
                }

                ctx.DrawImage(new CGRect(0, 0, (double)width, (double)height), image.CGImage);
                ctx.SelectFont("HelveticaNeue-Bold", fontSize, CGTextEncoding.MacRoman);
                ctx.SetFillColor(textColor.CGColor);

                // Draw the text at given coords.
                float textWidth = CalculateTextWidth(ctx);
                var xPos = Value.Length > 1 ? textWidth / 2 : 50;
                ctx.ShowTextAtPoint(xPos, 50, Value);

                return UIImage.FromImage(ctx.ToImage());
            }
        }

        private bool IsEmpty()
        {
            return Value == "0";
        }

        private float CalculateTextWidth(CGBitmapContext ctx)
        {
            // Measure the text's width - This involves drawing an invisible string to calculate the X position difference
            float start, end, textWidth;

            start = (float)ctx.TextPosition.X;
            ctx.SetTextDrawingMode(CGTextDrawingMode.Invisible);
            ctx.ShowText(Value);
            end = (float)ctx.TextPosition.X;
            textWidth = end - start;
            ctx.SetTextDrawingMode(CGTextDrawingMode.Fill);

            return textWidth;
        }
    }
}
