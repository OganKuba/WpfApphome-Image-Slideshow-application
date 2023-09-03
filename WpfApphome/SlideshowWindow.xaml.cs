using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Linq;

namespace WpfApphome
{
    public partial class SlideshowWindow : Window
    {
        private ISlideshowEffect _effect;
        private List<string> _imagePaths;
        private Image _currentImage, _nextImage;
        private int _currentImageIndex = 0;
        private bool _isPaused = false;
        private DispatcherTimer _timer;
        private List<Image> _images;
        public SlideshowWindow(ISlideshowEffect effect, List<string> imagePaths)
        {
            InitializeComponent();

            _effect = effect;
            _imagePaths = imagePaths;
            var bitmap = new BitmapImage(new Uri(_imagePaths[0]));
            _currentImage = new Image
            {
                Source = bitmap,
                Stretch = System.Windows.Media.Stretch.UniformToFill,
              
            };
            SlideshowGrid.Children.Add(_currentImage);


            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            _timer.Tick += Timer_Tick;


            PlaySlideshow();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_isPaused)
            {
                NextSlide();
            }
        }

        private void NextSlide()
        {
        
            _currentImageIndex = (_currentImageIndex + 1) % _imagePaths.Count;
            var bitmap = new BitmapImage(new Uri(_imagePaths[_currentImageIndex]));

            _nextImage = new Image
            {
                Source = bitmap,
                Stretch = System.Windows.Media.Stretch.UniformToFill,
            };

            SlideshowGrid.Children.Add(_nextImage);


            _effect.PlaySlideshow(_nextImage, _currentImage, this.Width, this.Height);


            var animationTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            animationTimer.Tick += (s, e) =>
            {
                SlideshowGrid.Children.Remove(_currentImage);
                _currentImage = _nextImage;

                ((DispatcherTimer)s).Stop();
            };
            animationTimer.Start();
        }

        private void PlaySlideshow()
        {
            _timer.Start();
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            _isPaused = !_isPaused;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            this.Close();
        }
    }
}
