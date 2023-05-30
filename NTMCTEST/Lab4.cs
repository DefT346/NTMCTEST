using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Math42.NTMC
{
    internal class Lab4
    {

        public static void Run()
        {
            QuadraticComparison(
                (int)Common.Utils.ReadBigInt("Параметр a: "),
                (int)Common.Utils.ReadBigInt("Параметр p: "));
            Console.ReadKey();
        }

        public static BigInteger QuadraticComparison(int a, int p, bool debug = false)
        {
            //var a = 14;
            //var p = 193;
            BigInteger x = -100;


            // Символ Лежандра
            var sign = Lab3.Legandr(a, p);
            if (debug) Console.WriteLine(Lab3.row1Log);
            Console.WriteLine($"\nСимвол Лежандра: ({a}/{p}) = {sign}");
            if (sign == 1)
                Console.WriteLine("=> Сравнение разрешимо\n");
            else
            {
                Console.WriteLine("=> Сравнение неразрешимо\n");
                return x;
            }


            // Обработка случаев
            if (Functions.Mod(p, 4) == 3)
            {
                Console.WriteLine("Первый случай:");

                var m = BigInteger.DivRem(p, 4, out BigInteger rem);
                x = BigInteger.ModPow(a, m + 1, p);
            }
            else
            if (Functions.Mod(p, 8) == 5)
            {
                Console.WriteLine("Второй случай:");

                var m = BigInteger.DivRem(p, 8, out BigInteger rem);
                var l = BigInteger.ModPow(a, 2 * m + 1, p);

                if (l == p - 1) l = -1;

                if (l == 1)
                {
                    Console.WriteLine("Первый подслучай:");
                    x = BigInteger.ModPow(a, m + 1, p);
                }
                else if (l == -1)
                {
                    Console.WriteLine("Второй подслучай:");
                    var a1 = BigInteger.ModPow(a, m + 1, p);
                    var a2 = BigInteger.ModPow(2, 2 * m + 1, p);
                    x = a1 * a2  % p;
                }
                else
                    throw new Exception($"Ошибка: l была равна {l}");
            }
            else
            {
                Console.WriteLine($"{p} != 3 mod(4) и {p} != 5 mod(8) => Используем алгоритм");

                if (debug) Console.WriteLine($"\nВыбираем N:");
                int N = 2;
                while (true)
                {
                    var signN = Lab3.Legandr(N, p);
                    if (debug) Console.WriteLine($"({N}/{p}) = {signN}");

                    if (signN == -1) break;
                    N++;
                }

                if (debug) Console.WriteLine($"\nНайдём представление p = 2^k * h + 1");
                (int k, int h) = FindKH(p);
                if (debug) Console.WriteLine($"k = {k}");
                if (debug) Console.WriteLine($"h = {h}\n");

                var a1 = BigInteger.ModPow(a, (h + 1) / 2, p);
                var a2 = Functions.Inverse(a, p);

                var N1 = BigInteger.ModPow(N, h, p);
                BigInteger N2 = 1;
                var j = 0;

                if (debug) Common.Utils.WriteRow("i", "b", "c", "d", "j", "N2");

                for (int i = 0; i < k - 1; i++)
                {
                    var b = Functions.Mod(a1 * N2, p);
                    var c = Functions.Mod(a2 * b * b, p);
                    var d = BigInteger.ModPow(c, (int)Math.Pow(2, (k - 2 - i)), p);
                    if (d == p - 1) d = -1;

                    if (d == -1) j = 1;
                    if (d == 1) j = 0;
                    N2 = Functions.Mod(N2 * BigInteger.ModPow(N1, (int)Math.Pow(2, i) * j, p), p);

                    if (debug) Common.Utils.WriteRow(i, b, c, d, j, N2);
                }

                x = Functions.Mod(a1 * N2, p);
                Console.WriteLine();
            }



            if (debug) Console.WriteLine($"x = +-{x} mod({p})");
            // Проверка
            var check = BigInteger.ModPow(x, 2, p);

            if (debug)
            {
                Console.WriteLine("\nПроверка:");
                Console.WriteLine($"+-{x}^2 =? {a} mod({p})");
                Console.WriteLine($"{x * x} = {BigInteger.ModPow(x, 2, p)} mod({p})");
            }

            if (a == check)
                Console.WriteLine($"\nОтвет сравнения: x = +-{x} mod({p})");
            else
                Console.WriteLine($"\nПроверка не пройдёна");
            return x;
        }

        public static (int k, int h) FindKH(int q)
        {
            for (int k = (int)Math.Truncate(Math.Log2(q - 1)); k >= 0; k--)
            {
                for (int h = q; h >= 0; h--)
                {
                    var p = Math.Pow(2, k) * h + 1;
                    if (p == q)
                        return (k, h);
                }
            }
            return (0, 0);
        }
    }
}
