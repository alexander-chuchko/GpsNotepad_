using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GpsNotepad.Controls
{
    public class CustomClock: SKCanvasView
    {
        public CustomClock()
        {
            this.PaintSurface += OnCanvasViewPaintSurface;
            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                this.InvalidateSurface();
                return true;
            });
        }

        SKPaint whiteStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        SKPaint paintLine = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.White.ToSKColor(),
            StrokeCap = SKStrokeCap.Round,
            StrokeWidth = 2f
        };

        SKPaint paintText = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = Color.White.ToSKColor()
        };


    SKPaint blackFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black
        };




        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface sKSurface = e.Surface;
            SKCanvas sKCanvas = sKSurface.Canvas;
            sKCanvas.Clear(SKColors.CornflowerBlue);

            int widht = e.Info.Width;
            int height = e.Info.Height;

            //Set transforms
            sKCanvas.Translate(widht / 2, height / 2);
            sKCanvas.Scale(widht / 200f);

            //Get DateTime
            DateTime dateTime = DateTime.Now;

            //Clock background
            sKCanvas.DrawCircle(0, 0, 100, blackFillPaint);

            sKCanvas.DrawCircle(0, 0, 7, paintLine);

            sKCanvas.DrawCircle(0, 0, 90, paintLine);

            sKCanvas.DrawLine(0, 90, 0, 83, paintLine);
            sKCanvas.DrawLine(0, -90, 0, -83, paintLine);
            sKCanvas.DrawLine(-90, 0, -83, 0, paintLine);
            sKCanvas.DrawLine(90, 0, 83, 0, paintLine);

            sKCanvas.DrawText("12", -7, -70, paintText);
            sKCanvas.DrawText("6", -4, 77, paintText);
            sKCanvas.DrawText("9", -77, 4, paintText);
            sKCanvas.DrawText("3", 72, 4, paintText);

            ///Hour hand
            sKCanvas.Save();
            sKCanvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            whiteStrokePaint.StrokeWidth = 6;
            sKCanvas.DrawLine(0, 0, 0, -50, whiteStrokePaint);
            sKCanvas.Restore();

            //Minute hand
            sKCanvas.Save();
            sKCanvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            whiteStrokePaint.StrokeWidth = 4;
            sKCanvas.DrawLine(0, 0, 0, -65, whiteStrokePaint);
            sKCanvas.Restore();

            //Second hand
            sKCanvas.Save();
            float seconds = dateTime.Second + dateTime.Millisecond / 1000f;
            sKCanvas.RotateDegrees(6 *seconds);
            whiteStrokePaint.StrokeWidth = 2;
            sKCanvas.DrawLine(0, 0, 0, -75, whiteStrokePaint);
            sKCanvas.Restore();

        }
    }
}
