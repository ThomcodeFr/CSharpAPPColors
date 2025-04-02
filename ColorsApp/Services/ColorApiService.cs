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
    
}