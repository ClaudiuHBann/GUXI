using Avalonia;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Layout;
using Avalonia.Controls;
using Avalonia.Interactivity;

using GUXI.ViewModels;

namespace GUXI.Pages
{
public partial class CalculatorPage : PageBase
{
    private const int _calculatorButtonRows = 5;
    private const int _calculatorButtonColumns = 4;

    private readonly string[,] _calculatorButtonsContent =
        new string[_calculatorButtonRows, _calculatorButtonColumns] { { "C", "C", ",", "," },
                                                                      { "7", "8", "9", "/" },
                                                                      { "4", "5", "6", "x" },
                                                                      { "1", "2", "3", "-" },
                                                                      { "+/-", "0", "=", "+" } };

    private readonly Button[,] _calculatorButtons = new Button[_calculatorButtonRows, _calculatorButtonColumns];
    private Grid _buttonsGrid = new() { RowDefinitions = new RowDefinitions("*,5,*,5,*,5,*,5,*"),
                                        ColumnDefinitions = new ColumnDefinitions("*,5,*,5,*,5,*") };

    public CalculatorPage()
    {
        InitializeComponent();
    }

    private void InitializeCalculatorButtons()
    {
        for (int row = 0; row < _calculatorButtonRows; row++)
        {
            for (int column = 0; column < _calculatorButtonColumns; column++)
            {
                if (row > 0 && _calculatorButtonsContent[row - 1, column] == _calculatorButtonsContent[row, column])
                {
                    _calculatorButtons[row, column] = _calculatorButtons[row - 1, column];
                    Grid.SetRowSpan(_calculatorButtons[row, column],
                                    Grid.GetRowSpan(_calculatorButtons[row, column]) + 2);
                }
                else if (column > 0 &&
                         _calculatorButtonsContent[row, column - 1] == _calculatorButtonsContent[row, column])
                {
                    _calculatorButtons[row, column] = _calculatorButtons[row, column - 1];
                    Grid.SetColumnSpan(_calculatorButtons[row, column],
                                       Grid.GetColumnSpan(_calculatorButtons[row, column]) + 2);
                }
                else
                {
                    _calculatorButtons[row, column] =
                        new Button() { Content = _calculatorButtonsContent[row, column],
                                       HorizontalAlignment = HorizontalAlignment.Stretch,
                                       VerticalAlignment = VerticalAlignment.Stretch,
                                       HorizontalContentAlignment = HorizontalAlignment.Center,
                                       VerticalContentAlignment = VerticalAlignment.Center };
                    _calculatorButtons[row, column].Click += OnButtonClickCalculator;

                    Grid.SetRow(_calculatorButtons[row, column], row * 2);
                    Grid.SetColumn(_calculatorButtons[row, column], column * 2);

                    _buttonsGrid.Children.Add(_calculatorButtons[row, column]);
                }
            }
        }
    }

    public override void Initialize()
    {
        if (DataContext is not CalculatorViewModel calculatorViewModel)
        {
            return;
        }

        InitializeCalculatorButtons();

        var calculatorGrid = new Grid() { RowDefinitions = new RowDefinitions("auto,auto,*"),
                                          ColumnDefinitions = new ColumnDefinitions("*"), Margin = new Thickness(5) };

        var expressionTextBlock =
            new TextBlock() { Text = calculatorViewModel.Expression,
                              TextAlignment = TextAlignment.Right,
                              [!TextBlock.TextProperty] = new Binding("Expression", BindingMode.OneWay),
                              VerticalAlignment = VerticalAlignment.Center,
                              FontSize = 18,
                              Margin = new Thickness(0, 10),
                              Padding = new Thickness(0, 0, 10, 0) };
        calculatorGrid.Children.Add(expressionTextBlock);

        var resultTextBlock = new TextBlock() { Text = calculatorViewModel.Result,
                                                TextAlignment = TextAlignment.Right,
                                                [!TextBlock.TextProperty] = new Binding("Result", BindingMode.OneWay),
                                                VerticalAlignment = VerticalAlignment.Center,
                                                FontSize = 22,
                                                Margin = new Thickness(0, 10),
                                                Padding = new Thickness(0, 0, 10, 0) };
        calculatorGrid.Children.Add(resultTextBlock);
        Grid.SetRow(resultTextBlock, 1);

        calculatorGrid.Children.Add(_buttonsGrid);
        Grid.SetRow(_buttonsGrid, 2);

        Content = calculatorGrid;
    }

    private void OnButtonClickCalculator(object? sender, RoutedEventArgs args)
    {
        if (sender is not Button button || button.Content is not string content ||
            DataContext is not CalculatorViewModel calculatorViewModel)
        {
            return;
        }

        calculatorViewModel.Calculate(content);
    }
}
}
