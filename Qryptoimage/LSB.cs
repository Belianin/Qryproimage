using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Qryptoimage
{
    public static class LSB
    {
        private static string Watermaker = "\uF63F";
        
        public static bool CheckWatermark(Bitmap bitmap)
        {
            var bytes = new BitmapIndexDecorator(bitmap, c => c.B)
                .Take(Encoding.UTF8.GetBytes(Watermaker).Length)
                .ToArray();

            return Encoding.UTF8.GetString(bytes).Equals(Watermaker);
        }
        
        public static void SetWatermark(Bitmap bitmap)
        {
            var bytes = new BitArray(Encoding.UTF8.GetBytes(Watermaker));

            var counter = 0;
            
            var decorator = new BitmapIndexDecorator(bitmap, c => c.B);
            foreach (bool b in bytes)
            {
                var color = decorator[counter];
                
                var blue = (int) color.B;
                if (blue % 2 == 0 && b)
                    blue += 1;
                else if (blue == 1 && !b) 
                    blue -= 1;
                
                decorator[counter] = Color.FromArgb(color.A, color.R, color.G, blue);
                
                counter++;
            }
        }
        
        public static string Decode(Bitmap bitmap)
        {
            var bytes = new BitmapIndexDecorator(bitmap, c => c.R)
                .TakeWhile(b => b != 0)
                .ToArray();

            return Encoding.UTF8.GetString(bytes.ToArray());
        }

        public static void Encode(Bitmap bitmap, string text)
        {
            var bytes = new BitArray(Encoding.UTF8.GetBytes($"{text}\u0000"));

            var counter = 0;
            
            var decorator = new BitmapIndexDecorator(bitmap, c => c.R);
            foreach (bool b in bytes)
            {
                var color = decorator[counter];
                
                var red = (int) color.R;
                if (red % 2 == 0 && b)
                    red += 1;
                else if (red % 2 == 1 && !b) 
                    red -= 1;
                
                decorator[counter] = Color.FromArgb(color.A, red, color.G, color.B);
                
                counter++;
            }
        }
    }
}