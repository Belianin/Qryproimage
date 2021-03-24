using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Qryptoimage
{
    public static class LSB
    {
        private class BitmapIndexDecorator : IEnumerable<byte>
        {
            private Bitmap bitmap;

            public BitmapIndexDecorator(Bitmap bitmap)
            {
                this.bitmap = bitmap;
            }

            public Color this[int index]
            {
                get
                {
                    var x = index % bitmap.Width;
                    var y = index / bitmap.Width;

                    return bitmap.GetPixel(x, y);
                }
                set
                {
                    var x = index % bitmap.Width;
                    var y = index / bitmap.Width;
                    
                    bitmap.SetPixel(x, y, value);
                }
            }

            public IEnumerator<byte> GetEnumerator()
            {
                var counter = 0;
                while (counter < (bitmap.Width * bitmap.Height) - 8)
                {
                    byte value = 0;
                    for (var i = 0; i < 8; i++)
                    {
                        var color = this[counter];
                        counter++;
                        
                        var n = color.R % 2 == 0 ? (byte) 0 : (byte) 1;
                        value |= (byte)(n << i);
                    }
                    
                    if (value == 0)
                        yield break;

                    yield return value;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        public static string Decode(Bitmap bitmap)
        {
            var bytes = new BitmapIndexDecorator(bitmap).ToArray();

            return Encoding.UTF8.GetString(bytes.ToArray());
        }

        public static void Encode(Bitmap bitmap, string text)
        {
            var bytes = new BitArray(Encoding.UTF8.GetBytes(text));

            var counter = 0;
            
            var decorator = new BitmapIndexDecorator(bitmap);
            foreach (bool b in bytes)
            {
                var color = decorator[counter];
                
                var red = (int) color.R;
                if (color.R % 2 == 0 && b)
                    red = color.R + 1;
                else if (color.R % 2 == 1 && !b) 
                    red = color.R - 1;
                
                decorator[counter] = Color.FromArgb(color.A, red, color.G, color.B);
                
                counter++;
            }
        }
    }
}