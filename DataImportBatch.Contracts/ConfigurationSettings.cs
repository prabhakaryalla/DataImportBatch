using System;

namespace DataImportBatch.Contracts;

public class ConfigurationSettings
{
    public string? BaseFolderPath { get; set; }
    public string? FailedFolderName { get; set; }
    public string? ArchiveFolderName { get; set; }
    public string? ProcessFolderName { get; set; }
    public string? InFromFolderName { get; set; }
    public string? OutToFolderName { get; set; }
}