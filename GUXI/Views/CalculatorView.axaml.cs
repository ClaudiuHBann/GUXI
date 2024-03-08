using GUXI.Pages;

namespace GUXI.Views
{
public partial class CalculatorView : ViewBase
{
    private readonly CalculatorPage _calculatorPage = new();

    public CalculatorView()
    {
        InitializeComponent();
        Content = _calculatorPage;
    }

    public override void Initialize()
    {
        _calculatorPage.Initialize();
    }
}
}
