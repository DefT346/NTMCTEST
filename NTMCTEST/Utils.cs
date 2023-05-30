using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Math42.Common
{

    internal class Utils
    {
        public static void WriteRow(params object[] elements)
        {
            WriteRow<object>(elements);
        }

        public static void WriteRow<T>(params T[] elements)
        {
            var x = Console.GetCursorPosition().Left;
            var y = Console.GetCursorPosition().Top;
            var step = 3;

            for (int i = 0; i < elements.Length; i++)
            {
                Console.SetCursorPosition(x + i * step, y);
                Console.Write(elements[i]);
            }
            Console.WriteLine();
        }

        public static int ReadInt(string message, int tempValue = 0)
        {
            Console.Write(message);
            var r = Console.ReadLine();
            if (r == "*")
                return tempValue;
            else
                return int.Parse(r);
        }

        public static BigInteger ReadBigInt(string message)
        {
            Console.Write(message);
            return BigInteger.Parse(Console.ReadLine());
        }

    }
}
