using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DataImportBatch.Contracts;
using DataImportBatch.Contracts.Services;
using Microsoft.Extensions.Options;

namespace DataImportBatch.Services;

public class FileService : IFileService
{

    private readonly CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false
    };

    public List<T> GetRecords<T>(string filepath)
    {
        List<T> records;
        using (var reader = new StreamReader(filepath))
        using (var csv = new CsvReader(reader, csvConfiguration))
        {
            records = csv.GetRecords<T>().ToList();
        }
        return records;
    }

    public void WriteRecords<T>(List<T> data, string filepath)
    {
        using (var writer = new StreamWriter(filepath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(data);
        }
    }
}
