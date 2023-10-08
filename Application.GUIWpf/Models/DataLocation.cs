using System.Collections.Generic;
using Application.GUIWpf.Infrastructures.Interfaces;

namespace Application.GUIWpf.Models;

internal class DataLocation : IReaderSupport
{
    public string Namespace { get; set; }
    public ICollection<ICoordinatesCollection> LocationsList { get; set; }
}