using System;
using DataImportBatch.Contracts.Models;
using DataImportBatch.Contracts.Services;
using DataImportBatch.Data.Data;
using AutoMapper;
using DataImportBatch.Data.Models;
using DataImportBatch.Services.Models;
using Microsoft.Extensions.Options;
using DataImportBatch.Contracts;

namespace DataImportBatch.Services;

public class TransactionService(AdWorksContext adWorksContext, IMapper mapper, IOptions<ConfigurationSettings> options) : ITransactionService
{
    private int batchLimit = options.Value.BatchLimit ?? 100;

    public List<TransactionHistoryOutputModel>? ImportTransactionHistoryData(List<TransactionHistoryRawModel> transactionHistoryRawModels)
    {
        var transactionHistoryModels = mapper.Map<List<TransactionHistoryOutputModel>>(transactionHistoryRawModels);
        var distinctProducts = (from s in transactionHistoryModels
                                select new
                                {
                                    Name = s.ProductName,
                                    ListPrice = s.ProductListPrice,
                                    
                                }).Distinct().ToList()
                                .Select(x => new ProductData()
                                {
                                    Name = x.Name,
                                    ListPrice = x.ListPrice,
                                }).ToList();

        var productsResponse = SaveProducts(distinctProducts);

        var transactionsTosave = transactionHistoryModels;
       
        if (!productsResponse.isSucess)
        {
            var failedproductNames = productsResponse.failedProducts.Select(x => x.Name).ToList();
            transactionsTosave = transactionHistoryModels.Where(item => !failedproductNames.Contains(item.ProductName))?.ToList();
        }

        List<string> successProductNames = transactionsTosave.Select(x => x.ProductName!).ToList();
        var dbProducts = adWorksContext.Products.Where(x => successProductNames.Contains(x.Name)).ToList();

        var transactions = (from s in transactionsTosave
                            join p in dbProducts on s.ProductName equals p.Name
                            select new TransactionHistoryData()
                            {
                                ProductId = p.ProductId,
                                TransactionDate = s.TransactionDate,
                                TransactionType = s.TransactionType,
                                ModifiedDate = s.ModifiedDate,
                                ActualCost = s.ActualCost,
                                Quantity = s.Quantity,
                                Identifier = s.Identifier
                            }).ToList();

        var transactionsResponse = SaveTransactionHistories(transactions);
       
        List<TransactionHistoryOutputModel> transactionHistoryOutputModels = (from s in transactionHistoryModels
                   join f in productsResponse.failedProducts
                      on s.ProductName equals f.Name  into sf_jointable
                   from p in sf_jointable.DefaultIfEmpty()
                   select new TransactionHistoryOutputModel()
                   {
                       ActualCost = s.ActualCost,
                       ModifiedDate = s.ModifiedDate,
                       ProductListPrice = s.ProductListPrice,
                       ProductName = s.ProductName,
                       Quantity = s.Quantity,
                       TransactionDate = s.TransactionDate,
                       TransactionType = s.TransactionType,
                       ErrorMessage = p?.ErrorMessage,
                       Identifier = s.Identifier,
                       IsProductSaved = p == null ? true : false
                   }).ToList();

        var res = (from s in transactionHistoryOutputModels
                   join f in transactionsResponse.failedTransactions
                   on s.Identifier equals f.Identifier 
                   into sf_jointable
                   from p in sf_jointable.DefaultIfEmpty()
                   select new TransactionHistoryOutputModel()
                   {
                       ActualCost = s.ActualCost,
                       ModifiedDate = s.ModifiedDate,
                       ProductListPrice = s.ProductListPrice,
                       ProductName = s.ProductName,
                       Quantity = s.Quantity,
                       TransactionDate = s.TransactionDate,
                       TransactionType = s.TransactionType,
                       ErrorMessage = p?.ErrorMessage ?? s.ErrorMessage,
                       IsProductSaved = s.IsProductSaved,
                       IsTransactionHistorySaved = s.IsProductSaved ? (p == null ? true : false) : false,
                       Identifier = s.Identifier
                   }).ToList();
        

        return res;
    }

    public (bool isSucess, List<ProductData> failedProducts) SaveProducts(List<ProductData> products)
    {
        List<ProductData> failedProducts = new List<ProductData>();
        for (int i = 0; i < products.Count; i = i + batchLimit)
        {
            var items = products.Skip(i).Take(batchLimit);
            try
            {
                var prods  = mapper.Map<List<Product>>(items);
                adWorksContext.AddRange(prods);
                adWorksContext.SaveChanges();
            }
            catch (Exception ex)
            {
                foreach(var product in items)
                    product.ErrorMessage = ex.InnerException?.Message ?? ex.Message;
                failedProducts.AddRange(items);
                adWorksContext.ChangeTracker.Clear();
            }
        }
        return (!failedProducts.Any(), failedProducts);
    }

    public (bool isSucess, List<TransactionHistoryData> failedTransactions) SaveTransactionHistories(List<TransactionHistoryData> transactionHistories)
    {
        List<TransactionHistoryData> failedTransactions = new List<TransactionHistoryData>();
        for (int i = 0; i < transactionHistories.Count; i = i + batchLimit)
        {
            var items = transactionHistories.Skip(i).Take(batchLimit);
            try
            {
                adWorksContext.AddRange(mapper.Map<List<TransactionHistory>>(items));
                adWorksContext.SaveChanges();
            }
            catch (Exception ex)
            {
                foreach(var trans in items)
                    trans.ErrorMessage = ex.InnerException?.Message ?? ex.Message;
                failedTransactions.AddRange(items);
                adWorksContext.ChangeTracker.Clear();
            }
        }
        return (!failedTransactions.Any(), failedTransactions);
    }
}
