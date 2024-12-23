using Domain.Entities;

namespace Services.RPN;

public class RPN
{
    static public (double, ErrorEntity?) Calculate(string input)
    {
        string output = GetExpression(input);
        double result = Counting(output);
        return (result, null);
    }

    static private string GetExpression(string input)
    {
        string output = string.Empty; 
        Stack<char> operStack = new Stack<char>(); 

        for (int i = 0; i < input.Length; i++)
        {
            if (IsDelimeter(input[i]))
                continue;
            
            if (Char.IsDigit(input[i]))
            {
                while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                {
                    output += input[i];
                    i++;

                    if (i == input.Length) break;
                }

                output += " ";
                i--;
            }

            if (IsOperator(input[i]))
            {
                if (input[i] == '(')
                    operStack.Push(input[i]);
                else if (input[i] == ')')
                {
                    char s = operStack.Pop();

                    while (s != '(')
                    {
                        output += s.ToString() + ' ';
                        s = operStack.Pop();
                    }
                }
                else
                {
                    if (operStack.Count > 0)
                        if (GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                            output += operStack.Pop() + " ";

                    operStack.Push(char.Parse(input[i].ToString()));
                }
            }
        }

        while (operStack.Count > 0)
            output += operStack.Pop() + " ";

        return output;
    }
    
    static private double Counting(string input)
    {
        double result = 0;
        Stack<double> temp = new Stack<double>();

        for (int i = 0; i < input.Length; i++)
        {
            if (Char.IsDigit(input[i])) 
            {
                string a = string.Empty;

                while (!IsDelimeter(input[i]) && !IsOperator(input[i])) 
                {
                    a += input[i]; 
                    i++;
                    if (i == input.Length) break;
                }
                temp.Push(double.Parse(a)); 
                i--;
            }
            else if (IsOperator(input[i])) 
            {
                double a = temp.Pop();
                double b = input[i] == 'r' ? 2 : temp.Pop();
                
                switch (input[i])
                { 
                    case '+': result = b + a; break;
                    case '-': result = b - a; break;
                    case '*': result = b * a; break;
                    case '/': result = b / a; break;
                    case '^': result = double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()); break;
                    case 'r': result = double.Parse(Math.Pow(double.Parse(a.ToString()), double.Parse((1.0 / b).ToString())).ToString()); break;
                }
                temp.Push(result); 
            }
        }
        return temp.Peek();
    }
    
    static private bool IsDelimeter(char c)
    {
        if ((" =".IndexOf(c) != -1))
            return true;
        return false;
    }
    
    static private bool IsOperator(char с)
    {
        if (("+-/*^()r".IndexOf(с) != -1))
            return true;
        return false;
    }
    
    static private byte GetPriority(char s)
    {
        switch (s)
        {
            case '(': return 0;
            case ')': return 1;
            case '+': return 2;
            case '-': return 3;
            case '*': return 4;
            case '/': return 4;
            case '^': return 5;
            case 'r': return 5;
            default: return 6;
        }
    }
}

public class Rpn
{
    static public bool IsDelimeter(char c)
    {
        if ((" =".IndexOf(c) != -1))
            return true;
        return false;
    }

    static public bool IsOperator(char с)
    {
        if (("+-/*^()r".IndexOf(с) != -1))
            return true;
        return false;
    }

    static public byte GetPriority(char s)
    {
        switch (s)
        {
            case '(': return 0;
            case ')': return 1;
            case '+': return 2;
            case '-': return 3;
            case '*': return 4;
            case '/': return 4;
            case '^': return 5;
            case 'r': return 5;
            default: return 6;
        }
    }

    static public (double? result, ErrorEntity? error) Calculate(string input)
        {
            var (output, error) = GetExpression(input);
            if (error != null)
                return (null, error);

            return Counting(output);
        }

        static private (string? output, ErrorEntity? error) GetExpression(string input)
        {
            string output = string.Empty;
            Stack<char> operStack = new Stack<char>();

            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (IsDelimeter(input[i]))
                        continue;

                    if (Char.IsDigit(input[i]))
                    {
                        while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                        {
                            output += input[i];
                            i++;

                            if (i == input.Length) break;
                        }

                        output += " ";
                        i--;
                    }

                    if (IsOperator(input[i]))
                    {
                        if (input[i] == '(')
                            operStack.Push(input[i]);
                        else if (input[i] == ')')
                        {
                            if (operStack.Count == 0)
                                return (null, new ErrorEntity("Несбалансированные скобки", 400));

                            char s = operStack.Pop();
                            while (s != '(')
                            {
                                output += s.ToString() + ' ';
                                if (operStack.Count == 0)
                                    return (null, new ErrorEntity("Несбалансированные скобки", 400));
                                s = operStack.Pop();
                            }
                        }
                        else
                        {
                            while (operStack.Count > 0 && GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                                output += operStack.Pop().ToString() + " ";

                            operStack.Push(input[i]);
                        }
                    }
                }

                while (operStack.Count > 0)
                {
                    char op = operStack.Pop();
                    if (op == '(')
                        return (null, new ErrorEntity("Несбалансированные скобки", 400));
                    output += op + " ";
                }

                return (output, null); 
            }
            catch (Exception ex)
            {
                return (null, new ErrorEntity($"Ошибка при разборе выражения: {ex.Message}", 500));
            }
        }

        static private (double? result, ErrorEntity? error) Counting(string input)
        {
            double result = 0;
            Stack<double> temp = new Stack<double>();

            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (Char.IsDigit(input[i]))
                    {
                        string a = string.Empty;

                        while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                        {
                            a += input[i];
                            i++;
                            if (i == input.Length) break;
                        }

                        if (!double.TryParse(a, out double number))
                            return (null, new ErrorEntity($"Не удалось преобразовать строку в число: {a}", 400));

                        temp.Push(number);
                        i--;
                    }
                    else if (IsOperator(input[i]))
                    {
                        if (temp.Count < 2)
                            return (null, new ErrorEntity("Недостаточно операндов для операции", 400));

                        double a = temp.Pop();
                        double b = input[i] == 'r' ? 2 : temp.Pop();

                        switch (input[i])
                        {
                            case '+':
                                result = b + a;
                                break;
                            case '-':
                                result = b - a;
                                break;
                            case '*':
                                result = b * a;
                                break;
                            case '/':
                                if (a == 0)
                                    return (null, new ErrorEntity("Деление на ноль невозможно", 400));
                                result = b / a;
                                break;
                            case '^':
                                result = Math.Pow(b, a);
                                break;
                            case 'r': 
                                result = double.Parse(Math.Pow(double.Parse(a.ToString()), double.Parse((1.0 / b).ToString())).ToString()); 
                                break;
                            default:
                                return (null, new ErrorEntity($"Неизвестный оператор: {input[i]}", 400));
                        }

                        temp.Push(result);
                    }
                }

                if (temp.Count != 1)
                    return (null, new ErrorEntity("Некорректное выражение", 400));

                return (temp.Pop(), null); 
            }
            catch (Exception ex)
            {
                return (null, new ErrorEntity($"Ошибка при вычислении: {ex.Message}", 500));
            }
        }
}