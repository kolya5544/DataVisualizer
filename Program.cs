using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace DataVisualization
{
    class Program
    {
        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: DataVisualization.exe <data> <output filename>");
                return;
            }
            string Input = args[0];
            string Output = args[1];

            byte[] bytes = File.ReadAllBytes(Input);

            int Res = (int)Math.Ceiling(Math.Sqrt(bytes.Length));

            Bitmap bmp = new Bitmap(Res, Res);
            Color trans = Color.FromArgb(0, 0, 0, 0);

            for (int X = 0; X < Res; X++)
                for (int Y = 0; Y < Res; Y++)
                {
                    bmp.SetPixel(X, Y, trans);
                }

            for (int i = 0; i < bytes.Length; i++)
            {
                int Y = i / Res;
                int X = i % Res;

                byte b = bytes[i];

                int RGB = ColorHLSToRGB(Math.Min(b, (byte)240), 120, 240);
                Color c = Color.FromArgb((RGB >> 16) & 0xff, (RGB >> 8) & 0xff, (RGB >> 0) & 0xff);


                bmp.SetPixel(X, Y, c);
            }

            bmp.Save(Output);
        }
    }
}
