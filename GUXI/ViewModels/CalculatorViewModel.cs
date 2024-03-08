using System;

using CommunityToolkit.Mvvm.ComponentModel;

using GUXI.Services;

namespace GUXI.ViewModels
{
public partial class CalculatorViewModel
(NotificationService notification) : ViewModelBase
{
    private readonly NotificationService _notification = notification;

    [ObservableProperty]
    private string _result = "0";

    private string _numberLeft = "0";
    private string _operation = "";
    private string _numberRight = "0";
    private bool _left = true;

    [ObservableProperty]
    private string expression = "";

    private void UpdateExpression(string nl = "")
    {
        if (nl == "" && _numberLeft == "0" && _numberRight == "0")
        {
            Expression = "";
            return;
        }

        nl = nl == "" ? _numberLeft : nl;
        if (_numberRight == "0")
        {
            Expression = $"{nl} {_operation} ";
        }
        else
        {
            Expression = $"{nl} {_operation} {_numberRight} = ";
        }
    }

    public void Calculate(string content)
    {
        try
        {
            switch (content)
            {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                HandleDigit(content);
                break;

            case "C":
                HandleClear();
                break;

            case ",":
                HandlePoint();
                break;

            case "+/-":
                HandleSign();
                break;

            case "+":
            case "-":
            case "x":
            case "/":
                HandleOperation(content);
                break;

            case "=":
                HandleEqual();
                break;

            default:
                throw new ArgumentException($"Invalid content {content}!");
            }
        }
        catch (Exception e)
        {
            _notification.NotifyE(e.Message);
        }
    }

    private void HandleDigit(string digit) => Apply(number =>
                                                    {
                                                        number = number.TrimStart('0');
                                                        if (number.EndsWith(".0"))
                                                        {
                                                            number = number.TrimEnd('0');
                                                        }

                                                        number += digit;
                                                        return number;
                                                    });

    private void HandleClear()
    {
        Result = "0";
        _numberLeft = "0";
        _operation = "";
        _numberRight = "0";
        ToggleNumber(true);

        UpdateExpression();
    }

    private void HandlePoint() => Apply(number => number.Contains('.') ? number : number + ".0");

    private void HandleSign() => Apply(number => number.StartsWith('-') ? number[1..] : "-" + number);

    private void HandleOperation(string operation)
    {
        _operation = operation;
        _numberRight = "0";
        ToggleNumber(false);

        UpdateExpression();
    }

    private void ToggleNumber(bool left) => _left = left;

    private void HandleEqual()
    {
        if (_operation == "/" && _numberRight == "0")
        {
            _notification.NotifyE("Cannot divide by zero!");
            return;
        }

        var nl = double.Parse(_numberLeft);
        var nr = double.Parse(_numberRight);
        var result = _operation switch { "+" => nl + nr, "-" => nl - nr, "x" => nl * nr, "/" => nl / nr,
                                         _ => default };

        Result = result.ToString();
        _numberLeft = Result;

        UpdateExpression(nl.ToString());
        ToggleNumber(true);
    }

    private void Apply(Func<string, string> action)
    {
        if (_left)
        {
            Result = _numberLeft = action(_numberLeft);
        }
        else
        {
            Result = _numberRight = action(_numberRight);
        }
    }
}
}
