using static System.Net.Mime.MediaTypeNames;

namespace SlideshowEffectInterfaces
{
    public interface ISlideshowEffect
    {
        string Name { get; }
        void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight);
    }
}