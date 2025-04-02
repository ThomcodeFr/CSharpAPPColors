namespace ColorsApp.Model;

public class PalettesDto
{
    public List<PaletteDto> Items { get; set; }

    public PalettesDto()
    {
        Items = new List<PaletteDto>();
    }
}