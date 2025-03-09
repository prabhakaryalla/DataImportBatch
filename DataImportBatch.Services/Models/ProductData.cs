using System;

namespace DataImportBatch.Services.Models;

public class ProductData
{
    public string? Name { get; set; }
    public string? ListPrice { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid? Identifier { get; set; }
}
