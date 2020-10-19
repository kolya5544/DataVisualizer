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
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: DataVisualization.exe <data> <output filename> [optional]<stepsize> [optional]<bias>");
                return;
            }
            string Input = args[0];
            string Output = args[1];

            int StepSize = 1;
            int Bias = 0;

            if(args.Length >= 3)
                StepSize = Int32.Parse(args[2]);
            if(args.Length >= 4)
                Bias = Int32.Parse(args[3]);

            byte[] bytes = File.ReadAllBytes(Input);

            int Res = (int)Math.Ceiling(Math.Sqrt(bytes.Length/StepSize));

            Bitmap bmp = new Bitmap(Res, Res);
            Color trans = Color.FromArgb(0, 0, 0, 0);

            for (int X = 0; X < Res; X++)
                for (int Y = 0; Y < Res; Y++)
                {
                    bmp.SetPixel(X, Y, trans);
                }

            for (int i = 0; i < bytes.Length/StepSize; i++)
            {
                int Y = i / Res;
                int X = i % Res;

                byte b = bytes[(i*StepSize)+Bias];

                int RGB = ColorHLSToRGB(Math.Min(b, (byte)240), 120, 240);
                Color c = Color.FromArgb((RGB >> 0) & 0xff, (RGB >> 8) & 0xff, (RGB >> 16) & 0xff);


                bmp.SetPixel(X, Y, c);
            }

            bmp.Save(Output);
        }
    }
}
