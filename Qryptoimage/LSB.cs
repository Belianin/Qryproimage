using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Qryptoimage
{
    public static class LSB
    {
        public static string Decode(Bitmap bitmap)
        {
            var bytes = new List<byte>();
            byte value = 0;
            var counter = 0;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    
                    byte n = color.R % 2 == 0 ? (byte) 0 : (byte) 1;
                    value |= (byte)(n << 7-counter);
                    counter++;
                    
                    if (counter == 8)
                    {
                        bytes.Add(value);
                        value = 0;
                        counter = 0;
                    }
                }
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }

        public static void Encode(Bitmap bitmap, string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var charCounter = 0;
            var byteCounter = 0;
            
            for (int i = 0; i < bitmap.Width; i++) // lable
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);

                    if (byteCounter == 8)
                    {
                        byteCounter = 0;
                        charCounter++;
                    }
                    else
                    {
                        byteCounter++;
                    }
                    
                    if (charCounter >= bytes.Length)
                        return;

                    var bb = bytes[charCounter];
                    var b = (bb & (1 << byteCounter-1));
                    var red = (int) color.R;
                    if (color.R % 2 == 0 && b == 1)
                    {
                        red = color.R + 1;
                    }
                    else if (color.R % 2 == 1 && b == 0)
                    {
                        red = color.R - 1;
                    }
                    
                    bitmap.SetPixel(i, j, Color.FromArgb(color.A, red, color.G, color.B));
                }
            }
        }
    }
}