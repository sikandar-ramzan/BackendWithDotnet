namespace Task1
{

   /* public class Calculator
    {
        public void Add(int a, int b) { Console.WriteLine("Sum: " + (a + b)); }
        public void Subtract(int a, int b) { Console.WriteLine("Difference: " + (a - b)); }
        public void Multiply(int a, int b)
        {
            int product = 0;
            for (int i = 0; i < b; i++)
            {
                product += a;
            }
            Console.WriteLine("Product: " + product);
        }

        public void Divide(int a, int b)
        {
            if (!Validations.IsUserInputNonZero(b))
            {
                Console.WriteLine("Quotient: Cannot divide by zero");
                Environment.Exit(0);
            }

            int quotient = 0;
            int sign = (a < 0 ^ b < 0) ? -1 : 1; // Determine the sign of the quotient
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (a >= b)
            {
                a -= b;
                quotient++;
            }

            Console.WriteLine("Quotient: " + (sign * quotient));
        }
    }*/
    public class Calculator
    {
        public void Add(int a, int b) { Console.WriteLine("Sum: " + (a + b)); }
        public void Subtract(int a, int b) { Console.WriteLine("Difference: " + (a - b)); }
        public void Multiply(int a, int b) { Console.WriteLine("Product: " + (a * b)); }
        public void Divide(int a, int b) { 
            if (!Validations.IsUserInputNonZero(b))
            {
                Console.WriteLine("Quotient: Cannot divide by zero");
                Environment.Exit(0);

            }

            Console.WriteLine("Quotient: " + (a / b)); 
        }
    }

    public class Validations
    {
        public static Boolean IsUserOperationInputValid(string operation)
        {
            switch (operation)
            {
                case "add":
                case "subtract":
                case "multiply":
                case "divide":
                case "exit":
                    return true;

                default:
                    return false;
            }
        }

        public static Boolean IsUserInputValid(string input)
        {
            return !IsUserInputEmpty(input) && IsUserInputNonNull(input) && !IsUserInputNonNumeric(input);
        }

        public static Boolean IsUserInputNonNull(string? input)
        {
            if (input == null) return false;
            return true;
        }

        public static Boolean IsUserInputEmpty(string input)
        {
            if (input.Length < 1) return true;
            return false;
        }

        public static Boolean IsUserInputNonZero(int input)
        {
            if (input == 0) return false;
            return true;
        }

        public static Boolean IsUserInputNonNumeric(string input)
        {
            if (int.TryParse(input, out _)) 
                return true;
            else 
                return false;
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter the operation you want to perform.");
            Console.WriteLine("\ni. add");
            Console.WriteLine("\nii. subtract");
            Console.WriteLine("\niii. divide");
            Console.WriteLine("\niv. multiply");
            Console.WriteLine("\nv. exit");
            Console.Write("\nChoice: ");

            string? userOperation;
            string? inputValue1;
            string? inputValue2;

            do
            {
                userOperation = Console.ReadLine();

                if (!Validations.IsUserOperationInputValid(userOperation))
                {
                    Console.Write("\nChoice: ");
                }
            } while (!Validations.IsUserOperationInputValid(userOperation));



            do
            {
                Console.WriteLine("\nEnter value 1: ");
                inputValue1 = Console.ReadLine();

            } while (Validations.IsUserInputValid(inputValue1));

            do
            {
                Console.WriteLine("\nEnter value 2: ");
                inputValue2 = Console.ReadLine();

            } while (Validations.IsUserInputValid(inputValue2));


            int parsedValue1 = int.Parse(inputValue1!);
            int parsedValue2 = int.Parse(inputValue2!);

            Calculator calculate = new Calculator();



            switch (userOperation)
            {
                case "add":
                    calculate.Add(parsedValue1, parsedValue2);
                    break;

                case "subtract":
                    calculate.Subtract(parsedValue1, parsedValue2);
                    break;

                case "multiply":
                    calculate.Multiply(parsedValue1, parsedValue2);
                    break;

                case "divide":
                    calculate.Divide(parsedValue1, parsedValue2);
                    break;

                case "exit":
                    Environment.Exit(0);
                    break;

                    default:
                    break;

            }

            Console.WriteLine("Executed Successfully");

        }

    }

   
}
