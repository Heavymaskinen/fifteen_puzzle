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
        public float CenterX => Width/2+(Col * Width);
        public float CenterY => Height/2+(Row * Height);

        public int Row { get;  set; }
        public int Col { get; set; }
        public string Value { get; set; }

        public UIImage DrawTile(UIImage image, UIColor textColor, int fontSize)
        {
            Frame = new CGRect(0, 0, Width, Height);
            Center = new CGPoint(CenterX, CenterY);
            UserInteractionEnabled = true;
            Image = image;
            if ( Value == "0")
            {
                return image;
            }

            nfloat width = image.Size.Width;
            nfloat height = image.Size.Height;

            using (CGBitmapContext ctx = new CGBitmapContext(IntPtr.Zero, (nint)width, (nint)height, 8, 4 * (nint)width, CGColorSpace.CreateDeviceRGB(), CGBitmapFlags.PremultipliedFirst))
            {
                ctx.DrawImage(new CGRect(0, 0, (double)width, (double)height), image.CGImage);
                ctx.SelectFont("HelveticaNeue-Bold", fontSize, CGTextEncoding.MacRoman);

                // Measure the text's width - This involves drawing an invisible string to calculate the X position difference
                float start, end, textWidth;

                // Get the texts current position
                start = (float)ctx.TextPosition.X;

                // Set the drawing mode to invisible
                ctx.SetTextDrawingMode(CGTextDrawingMode.Invisible);

                // Draw the text at the current position
                ctx.ShowText(Value);

                // Get the end position
                end = (float)ctx.TextPosition.X;

                // Subtract start from end to get the text's width
                textWidth = end - start;

                nfloat fRed, fGreen, fBlue, fAlpha;

                // Set the fill color to black. This is the text color.
                textColor.GetRGBA(out fRed, out fGreen, out fBlue, out fAlpha);
                ctx.SetFillColor(fRed, fGreen, fBlue, fAlpha);

                // Set the drawing mode back to something that will actually draw Fill for example
                ctx.SetTextDrawingMode(CGTextDrawingMode.Fill);

                // Draw the text at given coords.
                var xPos = Value.Length > 1 ? textWidth / 2 : 50;
                ctx.ShowTextAtPoint(xPos, 50, Value);

                return UIImage.FromImage(ctx.ToImage());
            }
        }
    }
}
