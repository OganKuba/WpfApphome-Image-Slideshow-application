using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace WpfApphome
{
    public interface ISlideshowEffect
    {
        string Name { get; }
        void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight);
    }
    public class HorizontalEffect : ISlideshowEffect
    {
        public string Name
        {
            get { return "Horizontal Effect"; }
        }

        public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
        {
            
            if (!(VisualTreeHelper.GetParent(imageIn) is Grid &&
                  VisualTreeHelper.GetParent(imageOut) is Grid))
            {
                throw new ArgumentException("Images must be placed in a Canvas for this effect.");
            }

           
            var inAnimation = new DoubleAnimation
            {
                From = windowWidth,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };

            var outAnimation = new DoubleAnimation
            {
                From = 0,
                To = -windowWidth,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };

            // Apply the animations
            imageIn.BeginAnimation(Canvas.LeftProperty, inAnimation);
            imageOut.BeginAnimation(Canvas.LeftProperty, outAnimation);
        }
    }

public class VerticalEffect : ISlideshowEffect
    {
        public string Name
        {
            get { return "Vertical Effect"; }
        }

        public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
        {
         
            if (!(VisualTreeHelper.GetParent(imageIn) is Canvas &&
                  VisualTreeHelper.GetParent(imageOut) is Canvas))
            {
                throw new ArgumentException("Images must be placed in a Grid for this effect.");
            }

     
            var inAnimation = new DoubleAnimation
            {
                From = windowHeight,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };

    
            var outAnimation = new DoubleAnimation
            {
                From = 0,
                To = -windowHeight,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };

            
            imageIn.BeginAnimation(Canvas.TopProperty, inAnimation);
            imageOut.BeginAnimation(Canvas.TopProperty, outAnimation);
        }
    }

    public class OpacityEffect : ISlideshowEffect
    {
        public string Name
        {
            get { return "Opacity Effect"; }
        }

        public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
        {
           
            var inAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };


            var outAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };

          
            imageIn.BeginAnimation(Image.OpacityProperty, inAnimation);
            imageOut.BeginAnimation(Image.OpacityProperty, outAnimation);
        }
    }




}
