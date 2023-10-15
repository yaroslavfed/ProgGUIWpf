namespace Common.Base.Interfaces;

public interface IReaderSupport
{
    string Namespace { get; set; }
    IList<ICoordinatesCollection> LocationsList { get; set; }
}