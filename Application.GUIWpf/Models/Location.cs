using Application.GUIWpf.Infrastructures.Interfaces;

namespace Application.GUIWpf.Models;

internal class Location : ICoordinatesCollection
{
    public double PointX { get; set; }
    public double PointY { get; set; }
}