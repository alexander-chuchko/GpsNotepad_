using GpsNotepad.Recource;
using GpsNotepad.Services.Theme;
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
        private SKColor _colorBackgroundClock;
        private readonly bool _timeAlive;
        private Color color;
        private string dfdsffsd;
        public CustomClock()
        {
            _timeAlive = false;
            //_colorBackgroundClock = ((Color)Resources["backgroundClock"]).ToSKColor();
            dfdsffsd = Application.Current.Resources["backgroundClock"].ToString();
            //_colorBackgroundClock = Color.FromHex(App.Current.Resources["backgroundClock"].ToString()).ToSKColor();
            
            _timeAlive = true;

            this.PaintSurface += OnCanvasViewPaintSurface;

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                this.InvalidateSurface();
                return _timeAlive;
            });
           
        }

        //Стрелки
        SKPaint whiteStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#c7cdf5").ToSKColor(),//SKColors.White
            StrokeWidth = 2f,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        //The hand of the clock
        SKPaint colorHandStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#596EFB").ToSKColor(),//SKColors.White
            StrokeWidth = 6f,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        SKPaint colorMinuteStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#273BC6").ToSKColor(),//SKColors.White
            StrokeWidth = 4f,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        //The Circle in center
        SKPaint paintCircleClock = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#596EFB").ToSKColor(),//Color.White.ToSKColor(),
            StrokeCap = SKStrokeCap.Round,
            StrokeWidth = 6f
        };




        SKPaint paintLine = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#c7cdf5").ToSKColor(),//Color.White.ToSKColor(),
            StrokeCap = SKStrokeCap.Round,
            StrokeWidth = 2f
        };

        SKPaint paintCircle = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#c7cdf5").ToSKColor(),//Color.White.ToSKColor(),
            StrokeCap = SKStrokeCap.Round,
            StrokeWidth = 4f
        };



        SKPaint paintText = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = Color.FromHex("#c7cdf5").ToSKColor() //Color.White.ToSKColor()
        };



        SKPaint blackFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            //Color = Color.FromHex(App.Current.Resources["PrimaryColor"] as string).ToSKColor()
            Color = Color.FromHex("#F1F3FD").ToSKColor()
        };

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface sKSurface = e.Surface;
            SKCanvas sKCanvas = sKSurface.Canvas;
            //sKCanvas.Clear(Color.FromHex("#FEFEFD").ToSKColor());
            sKCanvas.Clear();

            int widht = e.Info.Width;
            int height = e.Info.Height;

            //Set transforms
            sKCanvas.Translate(widht / 2, height / 2);
            sKCanvas.Scale(widht / 200f);

            //Get DateTime
            DateTime dateTime = DateTime.Now;

            //Clock background
            sKCanvas.DrawCircle(0, 0, 90, blackFillPaint);

            sKCanvas.DrawCircle(0, 0, 7, paintCircleClock);

            sKCanvas.DrawCircle(0, 0, 90, paintLine);

            //Деления
            sKCanvas.DrawLine(0, 90, 0, 83, paintLine);
            sKCanvas.DrawLine(0, -90, 0, -83, paintLine);
            sKCanvas.DrawLine(-90, 0, -83, 0, paintLine);
            sKCanvas.DrawLine(90, 0, 83, 0, paintLine);


            //Digitals
            sKCanvas.DrawText("12", -7, -70, paintText);
            sKCanvas.DrawText("6", -4, 77, paintText);
            sKCanvas.DrawText("9", -77, 4, paintText);
            sKCanvas.DrawText("3", 72, 4, paintText);

            ///Hour hand
            sKCanvas.Save();
            sKCanvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            whiteStrokePaint.StrokeWidth = 6;
            sKCanvas.DrawLine(0, -5, 0, -50, colorHandStrokePaint);
            sKCanvas.Restore();

            //Minute hand
            sKCanvas.Save();
            sKCanvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            whiteStrokePaint.StrokeWidth = 4;
            sKCanvas.DrawLine(0, 0, 0, -65, colorMinuteStrokePaint);
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
