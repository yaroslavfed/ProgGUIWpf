using Common.Base.Interfaces;

namespace Application.GUIWpf.Models;

internal class Location : ICoordinatesCollection
{
    public double PointX { get; set; }
    public double PointY { get; set; }
}