using System.Text;
using System.Text.Json;
using ColorsApp.Model;

namespace ColorsApp.Services;

public class ColorApiService
{
    private readonly HttpClient _httpClient;
    private const string apiUrl = "http://localhost:5050/ColorPalette";

    public ColorApiService()
    {
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// GET : Requête get pour obtenir une palette.
    /// </summary>
    /// <returns></returns>
    public async Task<PalettesDto> GetColorsPaletteAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(apiUrl);
            var palettesDto = JsonSerializer.Deserialize<PalettesDto>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return palettesDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    /// <summary>
    /// POST : Requête post pour transmettre une couleur. 
    /// </summary>
    /// <param name="palette"></param>
    /// <returns></returns>
    public async Task<bool> CreateColorPaletteAsync(PaletteDto palette)
    {
        try
        {
            var json = JsonSerializer.Serialize(palette);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}