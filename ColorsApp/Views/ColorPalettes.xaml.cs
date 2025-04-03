using System.Collections.ObjectModel;
using ColorsApp.ViewModels;

namespace ColorsApp.Views;

public partial class ColorPalettes : ContentPage
{
    private readonly ColorPalettesVm _viewModel;

    public ColorPalettes()
    {
        InitializeComponent();
        _viewModel = new ColorPalettesVm();
        BindingContext = _viewModel;
        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        await _viewModel.LoadColorsFromApiAsync();
        
    }
}