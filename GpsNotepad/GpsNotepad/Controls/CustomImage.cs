using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GpsNotepad.Controls
{
    public class CustomImage : Image
    {
        public CustomImage()
        {
            const int _animationTime = 10;
            var imageSwipe = new SwipeGestureRecognizer();
            var iconTap = new TapGestureRecognizer();
            imageSwipe.Swiped += async (sender, e) =>
            {
                try
                {
                    var btn = (CustomImage)sender;
                    await btn.ScaleTo(5, _animationTime);
                }
                catch (Exception ex)
                {
                    //ex.Message();
                }
            };
        }







        /*
        public CustomImage():base()
        {
            const int _animationTime = 10;
            var imageSwipe = new SwipeGestureRecognizer();
            var iconTap = new TapGestureRecognizer();
            imageSwipe.Swiped += async (sender, e) =>
            {
                try
                {
                    var btn = (CustomImage)sender;
                    await btn.ScaleTo(5, _animationTime);
                }
                catch (Exception ex)
                {
                    //ex.Message();
                }
            };
        }*/
    }
}
