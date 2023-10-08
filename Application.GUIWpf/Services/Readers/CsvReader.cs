using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Application.GUIWpf.Infrastructures.Interfaces;
using Application.GUIWpf.Models;

namespace Application.GUIWpf.Services.Readers;

public class CsvReader
{
    private string _fileFullName;

    private IReaderSupport? _fileData;

    public CsvReader(string fileFullName)
    {
        _fileFullName = fileFullName;
    }

    public async Task<IReaderSupport> Startup()
    {
        var pointPairs = new List<string>();
        
        using (var reader = new StreamReader(_fileFullName))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                MessageBox.Show(line);
            }
        }
        
        return _fileData;
    }
}