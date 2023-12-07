using Avalonia.Media.Imaging;
using System.Threading.Tasks;

namespace AvaloniaApplication3
{
    class Vozidlo
    {
        public required string Nazev { get; set; }
        public required decimal Cena { get; set; }
        public required string ImageUrl { get; init; }

        public Task<Bitmap?> ImageSource 
        { 
            get => ImageHelper.LoadFromWeb(new System.Uri(ImageUrl));
        } 
    }
}
