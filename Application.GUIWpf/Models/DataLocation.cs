using System.Collections.Generic;
using Common.Base.Interfaces;

namespace Application.GUIWpf.Models;

public class DataLocation : IReaderSupport
{
    public required string Namespace { get; set; }
    public IList<ICoordinatesCollection> LocationsList { get; set; }
}