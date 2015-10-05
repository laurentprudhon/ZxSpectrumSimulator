using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ZXSpectrumUI.PlatformAdapters
{
    public class ScreenAdapter
    {
        private ZXSpectrum.Screen zxScreen;

        private WriteableBitmap zxScreenBitmap;
        public ImageSource ImageSource { get { return zxScreenBitmap; } }

        public ScreenAdapter(ZXSpectrum.Screen zxScreen)
        {
            this.zxScreen = zxScreen;

            // Initialize the ZX Spectrum color palette
            IList<Color> zxColors = new List<Color>();
            zxColors.Add(Color.FromRgb(0x00, 0x00, 0x00)); // black      -- (bright 0) --
            zxColors.Add(Color.FromRgb(0x00, 0x00, 0xCD)); // blue
            zxColors.Add(Color.FromRgb(0xCD, 0x00, 0x00)); // red
            zxColors.Add(Color.FromRgb(0xCD, 0x00, 0xCD)); // magenta
            zxColors.Add(Color.FromRgb(0x00, 0xCD, 0x00)); // green
            zxColors.Add(Color.FromRgb(0x00, 0xCD, 0xCD)); // cyan
            zxColors.Add(Color.FromRgb(0xCD, 0xCD, 0x00)); // yellow
            zxColors.Add(Color.FromRgb(0xCD, 0xCD, 0xCD)); // white
            zxColors.Add(Color.FromRgb(0x00, 0x00, 0x00)); // black      -- (bright 1) --
            zxColors.Add(Color.FromRgb(0x00, 0x00, 0xFF)); // blue
            zxColors.Add(Color.FromRgb(0xFF, 0x00, 0x00)); // red
            zxColors.Add(Color.FromRgb(0xFF, 0x00, 0xFF)); // magenta
            zxColors.Add(Color.FromRgb(0x00, 0xFF, 0x00)); // green
            zxColors.Add(Color.FromRgb(0x00, 0xFF, 0xFF)); // cyan
            zxColors.Add(Color.FromRgb(0xFF, 0xFF, 0x00)); // yellow
            zxColors.Add(Color.FromRgb(0xFF, 0xFF, 0xFF)); // white
            BitmapPalette zxPalette = new BitmapPalette(zxColors);

            // Initialize a writeable bitmap to draw the zx screen, and display it through an image
            zxScreenBitmap = new WriteableBitmap(ZXSpectrum.Screen.WIDTH, ZXSpectrum.Screen.HEIGHT, 96, 96, PixelFormats.Indexed4, zxPalette);
        }       

        public void RefreshScreen()
        {  
            // apply pixels to bitmap
            zxScreenBitmap.WritePixels(new Int32Rect(0, 0, ZXSpectrum.Screen.WIDTH, ZXSpectrum.Screen.HEIGHT), zxScreen.Display, ZXSpectrum.Screen.WIDTH / 2, 0);
        }

        // -- Test function --
        /*
        int frameCounter = 0;

        private byte[] GenerateScrollingRainbowTestScreen()
        {
            // Create an array of pixels to contain pixel color values
            byte[] pixels = new byte[ZXSpectrum.Screen.WIDTH * ZXSpectrum.Screen.HEIGHT / 2];

            int position = 0;
            for (int y = 0; y < ZXSpectrum.Screen.HEIGHT; y++)
            {
                int colorIndex = ((y + frameCounter) / 20) % 16;
                for (int x = 0; x < ZXSpectrum.Screen.WIDTH / 2; x++)
                {
                    pixels[position] = (byte)(colorIndex + (colorIndex << 4));
                    position++;
                }
            }
            frameCounter++;

            return pixels;
        }
        */
    }
}
