namespace Common.Base.Interfaces;

public interface IReaderSupport
{
    string Namespace { get; set; }
    ICollection<ICoordinatesCollection> LocationsList { get; set; }
}