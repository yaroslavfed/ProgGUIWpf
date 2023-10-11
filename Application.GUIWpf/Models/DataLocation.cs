using System.Collections.Generic;
using Common.Base.Interfaces;

namespace Application.GUIWpf.Models;

public class DataLocation : IReaderSupport
{
    public string Namespace { get; set; }
    public ICollection<ICoordinatesCollection> LocationsList { get; set; }
}