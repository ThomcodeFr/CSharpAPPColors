using System.Collections.ObjectModel;

namespace ColorsApp.ViewModels;

public class ColorPaletteVm
{
    public ObservableCollection<ColorVm> Colors { get; set; }

    public ColorPaletteVm()
    {
        Colors = new ObservableCollection<ColorVm>();
    }

}