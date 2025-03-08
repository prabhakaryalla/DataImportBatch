using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DataImportBatch.Contracts;
using DataImportBatch.Contracts.Services;
using Microsoft.Extensions.Options;

namespace DataImportBatch.Services;

public class FileService : IFileService
{

    public List<T> GetRecords<T>(string filepath)
    {
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture){
            HasHeaderRecord = false
        };
        List<T> records;
        using (var reader = new StreamReader(filepath))
        using (var csv = new CsvReader(reader, csvConfiguration))
        {
            records = csv.GetRecords<T>().ToList();
        }
        return records;
    }
}
