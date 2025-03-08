
namespace DataImportBatch.Contracts.Services;

public interface IFileService
{
    Task<List<T>> ReadCsvFile<T>(string path);
}