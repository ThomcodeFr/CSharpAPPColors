namespace ColorsApp.Model;

public class PaletteDto
{
    public List<ColorDto> Colors { get; set; }

    public PaletteDto()
    {
        Colors = new List<ColorDto>();
    }
}