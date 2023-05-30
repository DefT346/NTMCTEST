using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Math42.NTMC
{
    internal class Lab5
    {

        public static void Run()
        {
            Console.Write("Введите кол-во элементов системы сравнений: ");
            var comparisonSystem = ReadComparisonSystem(int.Parse(Console.ReadLine()));
            Console.WriteLine("\nПреобразование...\n");

            var NCS = new List<(int a, int m)>();
            bool isRelPrimeNums = true;

            var gcdTemp = Functions.GCD(comparisonSystem[0].m, comparisonSystem[1].m);
            for (int i = 0; i < comparisonSystem.Count; i++)
            {
                if (i >= 2)
                {
                    gcdTemp = Functions.GCD(comparisonSystem[i].m, gcdTemp);
                    if (gcdTemp != 1) isRelPrimeNums = false;
                }

                var roots = Functions.Comparison(comparisonSystem[i].a, comparisonSystem[i].b, comparisonSystem[i].m, out BigInteger d);
                if (roots != null)
                {
                    var x = (int)roots[0];
                    (int c, int m) newC = (x, comparisonSystem[i].m / (int)d);
                    NCS.Add(newC);
                    Console.WriteLine($"x = {newC.c} (mod {newC.m})");
                }
                else
                {
                    Console.WriteLine("Одно из сравнений не имеет корней");
                    return;
                }
            }

            Console.WriteLine("\nВычисление...\n");

            if (isRelPrimeNums == false)
            {
                Console.WriteLine("Значения модулей попарно взаимно не простые!");
                return;
            }

            var x0Calc = "";

            try
            {
                var x0 = 0;
                var gm = 1;
                for (int i = 0; i < NCS.Count; i++)
                {
                    var M = 1;
                    var MiCalc = "";
                    for (int j = 0; j < NCS.Count; j++)
                    {
                        if (i != j) 
                        {
                            M *= NCS[j].m;
                            MiCalc += $"{NCS[j].m}";
                            if (j != NCS.Count - 1) MiCalc += " * ";
                            else MiCalc += $" = {M}";
                        }
                        else
                        {
                            if (j == NCS.Count - 1) MiCalc += $" = {M}";
                        }          
                    }

                    Console.WriteLine($"M{i + 1} = " + MiCalc);

                    var N = (int)Functions.Inverse(M, NCS[i].m);

                    Console.WriteLine($"N{i + 1} = {M}^(-1) mod {NCS[i].m} = {N}\n");

                    x0 += NCS[i].a * M * N;
                    gm *= NCS[i].m;

                    x0Calc += $"{NCS[i].a} * {M} * {N}";
                    if (i != NCS.Count - 1) x0Calc += " + ";
                    else x0Calc += $" = {x0}";
                }

                Console.WriteLine("x0 = " + x0Calc);

                x0 = Functions.Mod(x0, gm);

                Console.WriteLine($"\nx0 = {x0} mod {gm}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public static (int a, int b, int m) ReadComparison()
        {
            return ReadComparisonSystem(1)[0];
        }

        public static List<(int a, int b, int m)> ReadComparisonSystem(int n)
        {
            var comparisionParams = new List<(int a, int b, int m)>();

            var pattern = $"_   x = _   (mod _   )";
            var marker = "_";

            var y = Console.GetCursorPosition().Top;

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(pattern);
            }

            for (int i = 0; i < n; i++)
            {
                var data = FormatIntRead(i + y, pattern, marker);
                comparisionParams.Add((data[0], data[1], data[2]));
            }

            return comparisionParams;
        }

        public static int[] FormatIntRead(int rowIndex, string row, string marker)
        {
            var args = new List<int>();

            var indeces = GetSubstringIndeces(row, marker);

            var x = Console.GetCursorPosition().Left;
            var y = rowIndex;

            for(int i = 0; i < indeces.Length; i++)
            {
                Console.SetCursorPosition(x + indeces[i], y);

                var input = Console.ReadLine();
                if (input == "")
                {
                    input = "1";
                }
                args.Add(int.Parse(input));
            }

            return args.ToArray();
        }

        public static int[] GetSubstringIndeces(string source, string substring)
        {
            var indices = new List<int>();

            int index = source.IndexOf(substring, 0);
            while (index > -1)
            {
                indices.Add(index);
                index = source.IndexOf(substring, index + substring.Length);
            }

            return indices.ToArray();
        }
    }
}
