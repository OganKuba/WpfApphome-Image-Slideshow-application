//using SlideshowEffectInterfaces;
//using static System.Net.Mime.MediaTypeNames;

//namespace HorizontalEffectPlugin
//{
//    public class HorizontalEffect : ISlideshowEffect
//    {
//        public string Name
//        {
//            get { return "Horizontal Effect"; }
//        }

//        public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
//        {
//            // Ensure images are positioned in a Canvas for the animation to work
//            if (!(VisualTreeHelper.GetParent(imageIn) is Canvas &&
//                  VisualTreeHelper.GetParent(imageOut) is Canvas))
//            {
//                throw new ArgumentException("Images must be placed in a Canvas for this effect.");
//            }

//            // Create and configure the animation for the incoming image
//            var inAnimation = new DoubleAnimation
//            {
//                From = windowWidth,
//                To = 0,
//                Duration = new Duration(TimeSpan.FromSeconds(1))
//            };

//            // Create and configure the animation for the outgoing image
//            var outAnimation = new DoubleAnimation
//            {
//                From = 0,
//                To = -windowWidth,
//                Duration = new Duration(TimeSpan.FromSeconds(1))
//            };

//            // Apply the animations
//            imageIn.BeginAnimation(Canvas.LeftProperty, inAnimation);
//            imageOut.BeginAnimation(Canvas.LeftProperty, outAnimation);
//        }
//    }
//}