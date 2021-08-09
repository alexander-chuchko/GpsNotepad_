using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

namespace GpsNotepad.Controls
{
    public class CustomClock: SKCanvasView
    {
        private SKColor _clockColorBackground;
        private DateTimeOffset _dateTime;

        public CustomClock()
        {
            this.PaintSurface += OnCanvasViewPaintSurface;

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                this.InvalidateSurface();
                return true;
            });
        }

        SKPaint clockBackgroundPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = Color.FromHex("#F1F3FD").ToSKColor()
        };

        //Стрелки
        SKPaint whiteStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#c7cdf5").ToSKColor(),
            StrokeWidth = 2f,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        //The hand of the clock
        SKPaint colorHandStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#596EFB").ToSKColor(),
            StrokeWidth = 5f,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        SKPaint colorMinuteStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#273BC6").ToSKColor(),
            StrokeWidth = 4f,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        //The Circle in center
        SKPaint paintCircleClock = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#596EFB").ToSKColor(),
            StrokeCap = SKStrokeCap.Round,
            StrokeWidth = 5f
        };

        SKPaint paintLine = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#c7cdf5").ToSKColor(),
            StrokeCap = SKStrokeCap.Round,
            StrokeWidth = 2f
        };

        SKPaint paintCircle = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.FromHex("#c7cdf5").ToSKColor(),
            StrokeCap = SKStrokeCap.Round,
            StrokeWidth = 4f
        };

        SKPaint paintText = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = Color.FromHex("#c7cdf5").ToSKColor()
        };


        #region ---   Public properties   ---

        public static readonly BindableProperty ClockTextColorProperty =
                    BindableProperty.Create(nameof(ClockTextColor),
                    typeof(Color),
                    typeof(CustomClock),
                    defaultValue: Color.FromHex("#1E242B"),
                    defaultBindingMode: BindingMode.TwoWay,
                    propertyChanged: ClockTextColorChanged);

        public string ClockTextColor
        {
            get => (string)GetValue(ClockTextColorProperty);
            set => SetValue(ClockTextColorProperty, value);
        }


        public static readonly BindableProperty ClockBackgroundColorProperty =
                            BindableProperty.Create(nameof(ClockBackgroundColor),
                            typeof(Color),
                            typeof(CustomClock),
                            defaultValue: Color.FromHex("#F1F3FD"),
                            defaultBindingMode: BindingMode.TwoWay,
                            propertyChanged: ClockBackgroundColorChanged);

        public string ClockBackgroundColor
        {
            get => (string)GetValue(ClockBackgroundColorProperty);
            set => SetValue(ClockBackgroundColorProperty, value);
        }


        public static readonly BindableProperty MinuteColorProperty =
            BindableProperty.Create(nameof(MinuteColor),
            typeof(Color),
            typeof(CustomClock),
            defaultValue: Color.FromHex("#273BC6"),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: MinuteColorChanged);

        public string MinuteColor
        {
            get => (string)GetValue(MinuteColorProperty);
            set => SetValue(MinuteColorProperty, value);
        }


        public static readonly BindableProperty HourColorProperty =
            BindableProperty.Create(nameof(HourColor),
            typeof(Color),
            typeof(CustomClock),
            defaultValue: Color.FromHex("#596EFB"),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: HourColorChanged);

        public string HourColor
        {
            get => (string)GetValue(HourColorProperty);
            set => SetValue(HourColorProperty, value);
        }


        public static readonly BindableProperty SecondAndOutlineColorProperty =
            BindableProperty.Create(nameof(SecondAndOutlineColor),
                typeof(Color),
                typeof(CustomClock),
                defaultValue: Color.FromHex("#575c79"),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: SecondAndOutlineColorChanged);

        public string SecondAndOutlineColor
        {
            get => (string)GetValue(SecondAndOutlineColorProperty);
            set => SetValue(SecondAndOutlineColorProperty, value);
        }

        public static readonly BindableProperty TimeZoneTimeProperty =
            BindableProperty.Create(nameof(TimeZoneTime),
                            typeof(DateTimeOffset),
                            typeof(CustomClock),
                            defaultValue: default(DateTimeOffset),
                            propertyChanged: TimeZoneTimePropertyChanged);


        public DateTime TimeZoneTime
        {
            get => (DateTime)GetValue(TimeZoneTimeProperty);
            set => SetValue(TimeZoneTimeProperty, value);
        }

        #endregion

        #region --- Private helpers ---

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface sKSurface = e.Surface;
            SKCanvas sKCanvas = sKSurface.Canvas;
            sKCanvas.Clear();

            int widht = e.Info.Width;
            int height = e.Info.Height;

            //Set transforms
            sKCanvas.Translate(widht / 2, height / 2);
            sKCanvas.Scale(widht / 200f);

            //Get DateTime
            DateTimeOffset dateTime = DateTimeOffset.Now.AddHours(_dateTime.Offset.Hours);

            //Clock background
            sKCanvas.DrawCircle(0, 0, 90, clockBackgroundPaint);

            sKCanvas.DrawCircle(0, 0, 6, paintCircleClock);

            sKCanvas.DrawCircle(0, 0, 90, paintLine);

            //Division
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
            whiteStrokePaint.StrokeWidth = 5;
            sKCanvas.DrawLine(0, -6, 0, -50, colorHandStrokePaint);
            sKCanvas.Restore();

            //Minute hand
            sKCanvas.Save();
            sKCanvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            whiteStrokePaint.StrokeWidth = 4;
            sKCanvas.DrawLine(0, -9, 0, -65, colorMinuteStrokePaint);
            sKCanvas.Restore();

            //Second hand
            sKCanvas.Save();
            float seconds = dateTime.Second + dateTime.Millisecond / 1000f;
            sKCanvas.RotateDegrees(6 * seconds);
            whiteStrokePaint.StrokeWidth = 2;
            sKCanvas.DrawLine(0, -9, 0, -75, whiteStrokePaint);
            sKCanvas.Restore();
        }

        private static void TimeZoneTimePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomClock customClock = bindable as CustomClock;
            DateTimeOffset dateTime = (DateTimeOffset)newValue;
            if (customClock != null)
            {
                customClock._dateTime = dateTime;
            }
        }

        private static void ClockTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomClock customClock = bindable as CustomClock;
            Color colorHex = (Color)newValue;
            if (customClock != null)
            {
                customClock.paintText.Color = colorHex.ToSKColor();
            }
        }

        private static void ClockBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomClock customClock = bindable as CustomClock;
            Color colorHex = (Color)newValue;
            if (customClock != null)
            {
                customClock._clockColorBackground = colorHex.ToSKColor();

                customClock.clockBackgroundPaint.Color = customClock._clockColorBackground;
            }
        }
        private static void MinuteColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomClock customClock = bindable as CustomClock;
            Color colorHex = (Color)newValue;
            if (customClock != null)
            {
                customClock.colorMinuteStrokePaint.Color = colorHex.ToSKColor();
            }
        }

        private static void HourColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomClock customClock = bindable as CustomClock;
            Color colorHex = (Color)newValue;
            if (customClock != null)
            {
                customClock.colorHandStrokePaint.Color = colorHex.ToSKColor();
                customClock.paintCircleClock.Color = colorHex.ToSKColor();
            }
        }

        private static void SecondAndOutlineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomClock customClock = bindable as CustomClock;
            Color colorHex = (Color)newValue;
            if (customClock != null)
            {
                customClock.whiteStrokePaint.Color = colorHex.ToSKColor();
                customClock.paintLine.Color = colorHex.ToSKColor();
            }
        }

        #endregion


    }
}
