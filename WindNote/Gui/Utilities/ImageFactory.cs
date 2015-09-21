using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WindNote.Gui.Utilities
{
    public static class ImageFactory
    {
        public static BitmapImage CreateBitmapImageFromUri(String imageUri)
        {
            Uri uri = new Uri(imageUri);
            BitmapImage bitmapImage = new BitmapImage(uri);
            return bitmapImage;
        }

        public static ImageBrush CreateImageBrushFromUri(String imageUri)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = ImageFactory.CreateBitmapImageFromUri(imageUri);
            return imageBrush;
        }
    }
}
