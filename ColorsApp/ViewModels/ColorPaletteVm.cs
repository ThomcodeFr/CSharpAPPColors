using System.Collections.ObjectModel;

namespace ColorsApp.ViewModels;

public class ColorPaletteVm
{
    public ObservableCollection<ColorVm> Colors { get; set; }
    
    /// <summary>
    /// Constructor by default.
    /// </summary>
    public ColorPaletteVm()
    {
        Colors = new ObservableCollection<ColorVm>();
    }

}