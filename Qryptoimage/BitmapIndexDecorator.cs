using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Qryptoimage
{
    internal class BitmapIndexDecorator : IEnumerable<byte>
    {
        private readonly Bitmap bitmap;
        private readonly Func<Color, byte> colorPicker;

        public BitmapIndexDecorator(Bitmap bitmap, Func<Color, byte> colorPicker)
        {
            this.bitmap = bitmap;
            this.colorPicker = colorPicker;
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

                    var n = colorPicker(color) % 2 == 0 ? (byte) 0 : (byte) 1;
                    value |= (byte) (n << i);
                }

                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}