using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using ColorsApp.Model;

namespace ColorsApp.ViewModels;

public class ColorPalettesVm
{
    public ObservableCollection<ColorPaletteVm> ColorPalettes { get; set; } = new ObservableCollection<ColorPaletteVm>();

    public async Task LoadColorsFromApiAsync()
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync("http://localhost:5050/ColorPalette");
            var palettesDto = JsonSerializer.Deserialize<PalettesDto>(response, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true
            });

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
            Console.WriteLine($"Erreur lors du chargement des couleurs : {ex.Message}");
        }
    }
}