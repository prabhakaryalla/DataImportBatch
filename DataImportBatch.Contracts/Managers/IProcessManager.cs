using System;

namespace DataImportBatch.Contracts.Managers;

public interface IProcessManager
{
    Task RunAsync();
}
