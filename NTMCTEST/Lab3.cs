using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math42.NTMC
{
    class Fraction
    {
        public int a { get; set; } = 0;
        public int p { get; set; } = 1;
        public int sign { get; set; } = 1;

        public bool hasSubIterations = false;

        public Fraction(int a, int p)
        {
            this.p = p;
            this.a = a;
        }

        public Fraction(Fraction value)
        {
            this.p = value.p;
            this.a = value.a;
            this.sign = value.sign;
        }

        public void Turn()
        {
            var temp = a;
            a = p;
            p = temp;
        }

        public override string ToString()
        {
            return $"{(sign < 0 ? "-" : "")}{a}/{p}";
        }
    }

    internal class Lab3
    {
        public static string row1Log = "";

        public static int tab = 0;
        public static string output
        {
            set
            {
                if (value.Contains('\n'))
                {
                    row1Log += "\n";
                    for (int i = 0; i < tab; i++)
                    {
                        if (i == tab-3)
                            row1Log += "|";
                        else
                            row1Log += " ";
                    }
                    row1Log += value.Replace("\n", String.Empty);
                }
                else
                    row1Log += value;
            }
            get
            {
                return row1Log;
            }
        }

        public static int Legandr(int a, int p, bool debug = true)
        {
            var frac = new Fraction(a, p);

            var localSign = 1;

            if (frac.a == frac.p)
            {
                Console.WriteLine(0);
                return 0;
            }

            try
            {
                if (Functions.IsPrime(frac.p))
                {
                    localSign *= Properties.Iterate(frac);
                }
                else
                {
                    Console.WriteLine("\nЗнаменатель является составным числом, используем символ Якоби");
                    var ps = Properties.Factor(frac.p);
                    for (int i = 0; i < ps.Length; i++)
                    {
                        var subFrac = new Fraction(frac.a, ps[i]);
                        localSign *= Properties.Iterate(subFrac);
                    }
                }
            }
            catch (Exception)
            {
                localSign = 0;
            }

            return localSign;
        }

        public static void Run()
        {
            var a = Common.Utils.ReadInt("Введите числитель: ");
            var p = Common.Utils.ReadInt("Введите знаменатель: ");

            var sign = Legandr(a, p);

            Console.WriteLine(row1Log);
            Console.WriteLine("\nСимвол Лежандра: " + sign);

            Console.ReadLine();
        }
    }

    internal class Properties
    {
        public static int Iterate(Fraction frac)
        {
            Lab3.output = $"\n({frac}) ";

            var answer = Properties.Compute(frac); // получаем результат каждой дроби
            if (answer == -1000) Lab3.output = $"= ({frac}) ";

            while (answer == -1000)
            {
                answer = Properties.Compute(frac);
                if (answer == -1000) Lab3.output = $"= ({frac}) ";
            }
            if (answer == 0) throw new Exception(/*"Встречен 0 числитель, остановка вычислений"*/);
            if (!frac.hasSubIterations) Lab3.output = $"= ({answer}) ";
            return answer;
        }

        public static int Compute(Fraction frac)
        {
            if (frac.a == 0)
                return 0;
            else
            if (frac.a == 1)
            {
                return P4(frac);
            }
            else
            if (frac.a == 2)
            {
                return P6(frac);
            }
            else
            if (frac.a > frac.p)
            {
                P1(frac);
            }
            else
            if (Functions.IsPrime(frac.a))
            {
                P7(frac);
            }
            else
            if(frac.a < frac.p)
            {
                Lab3.tab += 3;
                // P3
                // факторизируем ткущий frac
                // происходит разветвеление

                var nums = Factor(frac.a);
                Lab3.output = "= ↓";
                var localSign = frac.sign; //Достаём знак дроби

                for (int i = 0; i < nums.Length; i++)
                {
                    frac.hasSubIterations = true;
                    var subFrac = new Fraction(nums[i], frac.p);
                    var answr = Iterate(subFrac);
                    localSign *= answr;

                    //if (!subFrac.hasSubIterations) Lab3.output = $"= ({answr})";
                }

                Lab3.tab -= 3;

                return localSign; // результат вычисления данной дроби
            }

            return -1000;
        }

        private static void P1(Fraction frac)
        {
            frac.a = frac.a % frac.p;
        }

        // Сделать проверку вычисляемого знака степенью
        private static int P6(Fraction frac)
        {
            var m = Functions.Mod(frac.p, 8);

            if (m == 1 || m == 7) return 1 * frac.sign;
            if (m == 3 || m == 5) return -1 * frac.sign;

            return -1000;

        }

        //Вопрос про -1 в свойствах в лекции тут возможны ошибки знака
        private static int P4(Fraction frac)
        {
            if (frac.sign == 1)
            {
                return 1;
            }
            else
            {
                if (Functions.Mod(frac.p, 4) == 1) return 1;
                if (Functions.Mod(frac.p, 4) == 3) return -1;
            }
            throw new Exception("Неизвестно");
        }

        private static void P7(Fraction frac)
        {
            frac.Turn();
            frac.sign *= (int)Math.Pow(-1, (frac.p - 1) / 2 * (frac.a - 1) / 2);
        }

        public static int[] Factor(int n)
        {
            var i = 2;
            List<int> primfac = new List<int>();
            while (i * i <= n)
            {
                while (n % i == 0)
                {
                    primfac.Add(i);
                    n = n / i;
                }
                i = i + 1;
            }
            if (n > 1)
                primfac.Add(n);

            return primfac.ToArray();
        }
    }
}
