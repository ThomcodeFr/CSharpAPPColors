using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using ColorsApp.Model;
using ColorsApp.Services;

namespace ColorsApp.ViewModels;

public class ColorPalettesVm : INotifyPropertyChanged
{
    public ObservableCollection<ColorPaletteVm> ColorPalettes { get; set; } = new ObservableCollection<ColorPaletteVm>();
    private readonly ColorApiService _apiService;
    private string _errorMessage;
    private bool _hasError;
    
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }
    
    public bool HasError
    {
        get => _hasError;
        set
        {
            _hasError = value;
            OnPropertyChanged();
        }
    }

    public ColorPalettesVm()
    {
        _apiService = new ColorApiService();
    }
    
    public async Task LoadColorsFromApiAsync()
    {
        try
        {
            var palettesDto = await _apiService.GetColorsPaletteAsync();
            if (palettesDto == null)
            {
                ErrorMessage = "Aucune donnée récupérée de l'API.";
                HasError = true;
                return;
            }
            
            ColorPalettes.Clear();
            foreach (var paletteDto in palettesDto.Items)
            {
                var paletteVm = new ColorPaletteVm();
                foreach (var colorDto in paletteDto.Colors)
                {
                    paletteVm.Colors.Add(new ColorVm
                    {
                        ColorType = colorDto.Type,
                        Color = Color.FromRgb(colorDto.Red, colorDto.Green, colorDto.Blue)
                    });
                }
                ColorPalettes.Add(paletteVm);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Erreur lors du chargement des couleurs";
            HasError = true;
            return;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}