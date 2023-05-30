using System.Numerics;

namespace Math42.NTMC
{
    internal class Functions
    {
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            a = BigInteger.Abs(a);
            b = BigInteger.Abs(b);

            var t = BigInteger.Min(a, b);
            a = BigInteger.Max(a, b);
            b = t;

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        public static BigInteger ExtGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }
            BigInteger g = ExtGCD(b, a % b, out y, out x); // x и y - переставляются
            //Console.Write($"({g}) y = {y} - ({a} / {b}) * {x} = ");
            y = y - (a / b) * x;
            //Console.WriteLine($"{y}");
            return g;
        }

        public static BigInteger Mod(BigInteger x, BigInteger m)
        {
            return (x % m + m) % m;
        }

        public static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        public static BigInteger Inverse(BigInteger value, BigInteger modulus)
        {
            value = Mod(value, modulus);
            if (GCD(value, modulus) == 1)
            {
                ExtGCD(value, modulus, out BigInteger x, out BigInteger y);
                return Mod(x, modulus);
            }
            else
            {
                throw new Exception($"Обратного значения {value} по модулю {modulus} не сущетвует.");
            }
        }

        public static BigInteger[] Comparison(BigInteger a, BigInteger b, BigInteger m, out BigInteger d)
        {
            d = GCD(a, m);
            var roots = new BigInteger[(int)d];

            if (b % d == 0)
            {
                a /= d;
                b /= d;
                m /= d;

                if (GCD(a, m) == 1)
                {
                    ExtGCD(a, m, out BigInteger x, out BigInteger y);
                    roots[0] = Mod(x * b, m); 
                    if (d > 1)
                        for (int n = 1; n < d; n++)
                            roots[n] = roots[0] + n * m;
                }
                return roots;
            }
            else
                return null;
        }
        public static bool IsPrime(int number)
        {
            if (number <= 3 && number > 1)
                return true;            // as 2 and 3 are prime
            else if (number == 1 || number % 2 == 0 || number % 3 == 0)
                return false;     // check if number is divisible by 2 or 3
            else
            {
                int i;
                for (i = 5; i * i <= number; i += 6)
                {
                    if (number % i == 0 || number % (i + 2) == 0)
                        return false;
                }
                return true;
            }
        }     
    }
}
