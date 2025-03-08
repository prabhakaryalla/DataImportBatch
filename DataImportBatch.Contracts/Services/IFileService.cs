
namespace DataImportBatch.Contracts.Services;

public interface IFileService
{
    List<T> GetRecords<T>(string filepath);
}