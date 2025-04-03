using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;
using ColorsApp.Helper;
using ColorsApp.Model;
using ColorsApp.Services;

namespace ColorsApp.ViewModels;

public class ColorPalettesVm : INotifyPropertyChanged
{
    public ObservableCollection<ColorPaletteVm> ColorPalettes { get; set; } = new ObservableCollection<ColorPaletteVm>();
    public ObservableCollection<ColorVm> AllColors { get; } = new ObservableCollection<ColorVm>();
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
    
    public ICommand CreatePaletteCommand { get; }

    public ColorPalettesVm()
    {
        _apiService = new ColorApiService();
        CreatePaletteCommand = new Command(async () => await CreateRandomPaletteAsync());
    }

    /// <summary>
    /// Méthode récupérant le GET
    /// </summary>
    public async Task LoadColorsFromApiAsync()
    {
        try
        {
            var palettesDto = await _apiService.GetColorsPaletteAsync();
            if (palettesDto == null || palettesDto.Items == null || !palettesDto.Items.Any())
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    ErrorMessage = "Aucune donnée récupérée de l'API.";
                    HasError = true;
                    AllColors.Clear();
                });
                return;
            }

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                HasError = false;
                ColorPalettes.Clear();
                AllColors.Clear();
                foreach (var paletteDto in palettesDto.Items)
                {
                    var paletteVm = new ColorPaletteVm();
                    foreach (var colorDto in paletteDto.Colors)
                    {
                        var colorVm = new ColorVm
                        {
                            ColorType = colorDto.Type,
                            Color = Color.FromRgb(colorDto.Red, colorDto.Green, colorDto.Blue)
                        };
                        paletteVm.Colors.Add(colorVm);
                        AllColors.Add(colorVm);
                    }

                    ColorPalettes.Add(paletteVm);
                }
            });
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ErrorMessage = "Erreur lors du chargement des couleurs";
                HasError = true;
                AllColors.Clear();
            }); 
        }
    }
    
    /// <summary>
    /// Méthode utilisant le post
    /// </summary>
    private async Task CreateRandomPaletteAsync()
    {
        try
        {
            var random = new Random();
            var newPalette = new PaletteDto
            {
                Colors = new List<ColorDto>
                {
                    new ColorDto
                    {
                        Type = ColorType.Primary, Red = random.Next(256), Green = random.Next(256),
                        Blue = random.Next(256)
                    },
                }
            };
            var success = await _apiService.CreateColorPaletteAsync(newPalette);

            if (success)
            {
                await LoadColorsFromApiAsync();
                HasError = false;
            }
            else
            {
                ErrorMessage = "Erreur lors du chargement des couleurs";
                HasError = true;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Erreur lors de la création : {ex.Message}";
            HasError = true;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}