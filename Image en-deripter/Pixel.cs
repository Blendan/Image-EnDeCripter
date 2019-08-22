using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_en_deripter
{
    class Pixel
    {
        private byte red, green, blue;
        private int x, y;

        public Pixel(byte red, byte green, byte blue, int x, int y)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.X = x;
            this.Y = y;
        }

        public byte Red { get => red; set => red = value; }
        public byte Green { get => green; set => green = value; }
        public byte Blue { get => blue; set => blue = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
    }
}
