using DataImportBatch.Contracts.Services;

namespace DataImportBatch.Services;

public class FileService() : IFileService
{
    public Task<List<T>> ReadCsvFile<T>(string path)
    {
        throw new NotImplementedException();
    }
}
