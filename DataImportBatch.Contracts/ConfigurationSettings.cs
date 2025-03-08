using System;

namespace DataImportBatch.Contracts;

public class ConfigurationSettings
{
    public string? BaseFolderPath { get; set; }
    public string? FailedFolder { get; set; }
    public string? ArchiveFolder { get; set; }
    public string? ProcessFolder { get; set; }
    public string? InFromFolder { get; set; }
    public string? OutToFolder { get; set; }
    public int? BatchLimit {get; set;}
}