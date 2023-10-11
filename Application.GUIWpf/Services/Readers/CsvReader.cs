using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.GUIWpf.Models;
using Common.Base.Abstractions;
using Common.Base.Converters;
using Common.Base.Interfaces;

namespace Application.GUIWpf.Services.Readers;

public sealed class CsvReader : ReaderBase
{
    #region Private fields

    private bool _isActionStart;

    #endregion

    #region Properties

    public override string? FileName { get; set; }

    public override required string FileExtension { get; set; }

    #endregion

    #region Constructors

    public CsvReader()
    {
    }

    #endregion

    #region Methods

    public override async Task<IReaderSupport?> StartupAsync()
    {
        FileName = OpenTargetFile(FileExtension, out _isActionStart);

        if (!_isActionStart)
        {
            return null;
        }

        var stringPointPairsCollection = ReadFileAsync();

        var coordinatesCollection = DataCollectionInitialization(await stringPointPairsCollection);

        var dataLocation = new DataLocation
        {
            Namespace = FileName.ToShortFileName(),
            LocationsList = await coordinatesCollection
        };

        return dataLocation;
    }

    public override string OpenTargetFile(string? fileExtension, out bool result)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            DefaultExt = FileExtension,
            Filter = $"Text documents ({FileExtension})|*{FileExtension}"
        };

        result = (bool)dialog.ShowDialog()!;

        if (result)
        {
            string filename = dialog.FileName;
            return filename;
        }

        return string.Empty;
    }

    public override async Task<IList<string>> ReadFileAsync()
    {
        var pointPairs = new List<string>();

        using var reader = new StreamReader(FileName!);
        while (await reader.ReadLineAsync() is { } line)
        {
            pointPairs.Add(line);
        }

        return pointPairs;
    }

    public override Task<IList<ICoordinatesCollection>> DataCollectionInitialization(
        IEnumerable<string> pointsCollection)
    {
        var coordinatesCollection = new List<ICoordinatesCollection>();

        foreach (var pointPair in pointsCollection)
        {
            var data = new Location();
            var coordinate = pointPair.Split(',');

            double.TryParse(coordinate[0], out var pointX);
            double.TryParse(coordinate[1], out var pointY);

            data.PointX = pointX;
            data.PointY = pointY;

            coordinatesCollection.Add(data);
        }

        return Task.FromResult<IList<ICoordinatesCollection>>(coordinatesCollection);
    }

    #endregion
}