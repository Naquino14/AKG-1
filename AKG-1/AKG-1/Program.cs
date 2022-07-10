using System;
using c = System.Console;
using ADIS;

namespace Sandbox
{
    public class Prorgam
    {
        public static void Main(string[] args)
        {
            for (; ; )
            {
                c.WriteLine("Enter a number:");
                var n = int.Parse(c.ReadLine() ?? throw new ArgumentNullException());
                c.WriteLine($"{n} {(AKG1.IsPrime(n) ? "is" : "is not")} a prime number.");
            }
            // uh

            //var result = akg.GenerateKeysT3();

            //c.Write("Generated key 1: ");
            //foreach (var b in result.Key1)
            //    c.Write($"0x{b:X}");
            //c.Write("\nGenerated key 2: ");
            //foreach (var b in result.Key2)
            //    c.Write($"0x{b:X}");

            //int a = 2, b = 12;
            //int k = 10;
            //c.WriteLine($"{a} ^ {b} % {k} is {AKG1.EBSMK(a, b, k)}");

            //int ai = 1245682;
            //byte[] ab = new byte[4];
            //var ar = AKG1.I2B(ai);
            //var ao = AKG1.B2I(ar[0], ar[1], ar[2], ar[3]);
            //c.WriteLine($"{ao} as a byte[] is {{ 0x{ar[0]:X}, 0x{ar[1]:X}, 0x{ar[2]:X}, 0x{ar[3]:X} }}");

            //int[] polyC = new int[] { 2, 6, -3, 2 };
            //int s = 3;
            //c.WriteLine($"F(x) := {polyC[0]}x^3 + {polyC[1]}x^2 + {polyC[2]}x + {polyC[3]} | F({s}) = {AKG1.SolvePoly(in polyC, s)}");

            //float lole = 0;
            //c.WriteLine(69 / lole);
            //while (true)
            //{
            //    #nullable disable
            //    c.WriteLine("a?");
            //    int a = Int32.Parse(c.ReadLine());
            //    c.WriteLine("b?");
            //    int b = Int32.Parse(c.ReadLine());
            //    c.WriteLine($"{a} ^ {b} = {xor(a,b)}");

            //}
        }

        static int xor(int a, int b)
        { return a ^ b; }
    }
}