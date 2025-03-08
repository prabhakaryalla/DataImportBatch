using System;
using DataImportBatch.Data.Models;

namespace DataImportBatch.Services.Models;

public class ProductResponse : Product
{
    public bool IsCreated { get; set; }
    public string ErrorMessage { get; set; }
}
