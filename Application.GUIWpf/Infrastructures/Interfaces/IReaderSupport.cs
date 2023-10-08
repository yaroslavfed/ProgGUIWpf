using System.Collections;
using System.Collections.Generic;

namespace Application.GUIWpf.Infrastructures.Interfaces;

public interface IReaderSupport
{
    string Namespace { get; set; }
    ICollection<ICoordinatesCollection> LocationsList { get; set; }
}