using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace Image_en_deripter
{
    public class BitImage
    {
        private int width, height;
        private List<Pixel> pixels;

        public void LoadImage(string url)
        {
            pixels = new List<Pixel>();

            Bitmap img = new Bitmap(url);
            width = img.Width;
            height = img.Height;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    pixels.Add(new Pixel(pixel.R, pixel.G, pixel.B, i, j));
                }
            }
        }

        private void EditPixels(String message)
        {
            byte[] array = Encoding.UTF8.GetBytes(message);
            BitArray bits = new BitArray(array);

            int i = 0;
            foreach(Pixel p in pixels)
            {
                bool gc = false, rc = false;
                byte[] bytes;
                if (i < bits.Length)
                {
                    BitArray bit = new BitArray(new BitArray(new byte[] { p.Red }));
                    bit[0] = bits[i];
                    bytes = new byte[1];
                    bit.CopyTo(bytes, 0);
                    p.Red = bytes[0];
                    i ++;

                    rc = true;
                }

                if (i < bits.Length)
                {
                    BitArray bit = new BitArray(new BitArray(new byte[] { p.Red }));
                    bit[1] = bits[i];
                    bytes = new byte[1];
                    bit.CopyTo(bytes, 0);
                    p.Red = bytes[0];
                    i++;

                    rc = true;
                }

                if (i < bits.Length)
                {
                    BitArray bit = new BitArray(new BitArray(new byte[] { p.Green }));
                    bit[0] = bits[i];
                    bytes = new byte[1];
                    bit.CopyTo(bytes, 0);
                    p.Green = bytes[0];
                    i++;

                    gc = true;
                }

                if (i < bits.Length)
                {
                    BitArray bit = new BitArray(new BitArray(new byte[] { p.Green }));
                    bit[1] = bits[i];
                    bytes = new byte[1];
                    bit.CopyTo(bytes, 0);
                    p.Green = bytes[0];
                    i++;

                    gc = true;
                }

                BitArray bitBlue = new BitArray(new BitArray(new byte[] { p.Blue }));

                bitBlue[0] = rc;
                bitBlue[1] = gc;

                bytes = new byte[1];
                bitBlue.CopyTo(bytes, 0);
                p.Blue = bytes[0];
            }
        }

        public String ReadMessage()
        {
            //BitArray bits = new BitArray();
            List<bool> boolBits = new List<bool>();
            foreach (Pixel p in pixels)
            {
                BitArray bitBlue = new BitArray(new BitArray(new byte[] { p.Blue }));

                if(bitBlue[0])
                {
                    BitArray bit = new BitArray(new BitArray(new byte[] { p.Red }));
                    boolBits.Add(bit[0]);
                    boolBits.Add(bit[1]);
                }

                if (bitBlue[1])
                {
                    BitArray bit = new BitArray(new BitArray(new byte[] { p.Green }));
                    boolBits.Add(bit[0]);
                    boolBits.Add(bit[1]);
                }
            }

            boolBits.Reverse();

            BitArray bits = new BitArray(boolBits.ToArray());

            //Console.Out.WriteLine(boolBits.Count);

            byte[] bytes = ToByteArray(bits);

            /*
            var sb = new StringBuilder();

            for (int i = 0; i < boolBits.Count; i++)
            {
                char c = boolBits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
            */

            String s = Encoding.UTF8.GetString(bytes);

            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private byte[] ToByteArray(BitArray bits)
        {
            int numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0) numBytes++;

            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }

        public Bitmap CreateBitmap(String message)
        {

            EditPixels(message);

            Bitmap bmp = new Bitmap(width, height);

            foreach(Pixel p in pixels)
            {
                Color color = Color.FromArgb(p.Red, p.Green, p.Blue);
                bmp.SetPixel(p.X, p.Y, color);
            }

            return bmp;
        }

        public int GetUsabelBitCount()
        {   
            if(pixels!=null)
            return pixels.Count*4;
            return 0;
        }
    }
}
