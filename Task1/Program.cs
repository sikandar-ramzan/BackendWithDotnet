namespace Task1
{

    public class Calculator
    {
        public void Add(double a, double b) {
            Console.WriteLine("\n-------------");
            Console.WriteLine("Sum: " + (a + b)); 
        }
        public void Subtract(double a, double b) { 
            Console.WriteLine("Difference: " + (a - b)); 
        }
        public void Multiply(double a, double b)
        {
            double product = 0;
            for (int i = 0; i < b; i++)
            {
                product += a;
            }
            Console.WriteLine("Product: " + product);
        }

        public void Divide(double a, double b)
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

            Console.WriteLine("Quotient: " + (sign * quotient) + "." + a);
        }
    }


    public class Validations
    {
        public static Boolean IsUserOperationInputValid(string operation)
        {
            switch (operation)
            {
                case "add":
                case "+":
                case "subtract":
                case "-":
                case "multiply":
                case "*":
                case "divide":
                case "/":
                case "exit":
                    return true;

                default:
                    return false;
            }
        }

        public static Boolean IsUserInputInValid(string input)
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

        public static Boolean IsUserInputNonZero(double input)
        {
            if (input == 0) return false;
            return true;
        }

        public static Boolean IsUserInputNonNumeric(string input)
        {
            if (float.TryParse(input, out _)) 
                return true;
            else 
                return false;
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string? userOperation;
            string? inputValue1;
            string? inputValue2;

            do
            {


                Console.WriteLine("Enter the operation you want to perform.");
                Console.WriteLine("\nFor addition, enter add OR +");
                Console.WriteLine("\nFor subtraction, enter subtract OR -");
                Console.WriteLine("\nFor division, enter divide OR /");
                Console.WriteLine("\nFor multiplication, enter multiply OR *");
                Console.WriteLine("\nv. exit");
                Console.Write("\nChoice: ");



                do
                {
                    userOperation = Console.ReadLine();

                    if (!Validations.IsUserOperationInputValid(userOperation))
                    {
                        Console.WriteLine("invalid input! Enter correct value.");
                        Console.Write("\nChoice: ");
                    }
                } while (!Validations.IsUserOperationInputValid(userOperation));



                do
                {
                    Console.WriteLine("\nEnter value 1: ");
                    inputValue1 = Console.ReadLine();

                    if (Validations.IsUserInputInValid(inputValue1))
                        Console.WriteLine("invalid input! Enter correct value.");

                } while (Validations.IsUserInputInValid(inputValue1));

                do
                {
                    Console.WriteLine("\nEnter value 2: ");
                    inputValue2 = Console.ReadLine();

                } while (Validations.IsUserInputInValid(inputValue2));


                double parsedValue1 = double.Parse(inputValue1!);
                double parsedValue2 = double.Parse(inputValue2!);

                Calculator calculate = new Calculator();


                switch (userOperation)
                {
                    case "+":
                    case "add":
                        calculate.Add(parsedValue1, parsedValue2);
                        break;

                    case "-":
                    case "subtract":
                        calculate.Subtract(parsedValue1, parsedValue2);
                        break;

                    case "*":
                    case "multiply":
                        calculate.Multiply(parsedValue1, parsedValue2);
                        break;

                    case "/":
                    case "divide":
                        calculate.Divide(parsedValue1, parsedValue2);
                        break;

                    case "exit":
                        Environment.Exit(0);
                        break;

                    default:
                        break;

                }



                Console.WriteLine("-------------\n\n");
            } while (userOperation != "exit");

        }

    }

   
}
