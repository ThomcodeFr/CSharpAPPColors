using ColorsApp.Helper;

namespace ColorsApp.ViewModels;

public class ColorVm
{
    public string Name => ColorType.ToString();
    public Color Color { get; set; }
    public ColorType ColorType { get; set; }
}