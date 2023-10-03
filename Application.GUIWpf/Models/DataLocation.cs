using System.Collections.Generic;

namespace Application.GUIWpf.Models;

internal class DataLocation
{
    public string Namespace { get; set; }
    public ICollection<Location> LocationsList { get; set; }
}