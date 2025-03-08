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
    public Task ImportTransactionHistoryData()
    {
        var data = adWorksContext.Products.ToList();
        return Task.CompletedTask;
    }

    public (bool isSuccess, List<TransactionHistoryModel>? successRecords, List<TransactionHistoryModel>? failedRecords) ImportTransactionHistoryData(List<TransactionHistoryModel> transactionHistoryModels)
    {
        var allTransactionHistoryRecords = transactionHistoryModels;
        var distinctProducts = (from s in transactionHistoryModels
                                select new
                                {
                                    Name = s.ProductName,
                                    ListPrice = s.ProductListPrice
                                }).Distinct().ToList()
                                .Select(x => new ProductData()
                                {
                                    Name = x.Name,
                                    ListPrice = x.ListPrice
                                }).ToList();

        var productsResponse = SaveProducts(mapper.Map<List<Product>>(distinctProducts));

        var transactionsTosave = transactionHistoryModels;
       
        if (!productsResponse.isSucess)
        {
            var failedProductNames = productsResponse.failedProducts.Select(x => x.Name).ToList();
            transactionsTosave = transactionHistoryModels.Where(item => !failedProductNames.Contains(item.ProductName))?.ToList();
        }

        List<string> successProductNames = transactionsTosave.Select(x => x.ProductName).ToList();
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
                                Quantity = s.Quantity
                            }).ToList();

        var transactionsResponse = SaveTransactionHistories(mapper.Map<List<TransactionHistory>>(transactions));
        if (!transactionsResponse.isSucess)
        {
            
        }

        List<TransactionHistoryOutputModel> transactionHistoryOutputModels = (from s in allTransactionHistoryRecords
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
                       ErrorMessage = p?.ErrorMessage
                   }).ToList();

        // var res = (from s in transactionHistoryOutputModels
        //            join f in transactionsResponse.failedTransactions
        //            on s.ProductName equals f.Product.Name  
        //            into sf_jointable
        //            from p in sf_jointable.DefaultIfEmpty()
        //            select new TransactionHistoryOutputModel()
        //            {
        //                ActualCost = s.ActualCost,
        //                ModifiedDate = s.ModifiedDate,
        //                ProductListPrice = s.ProductListPrice,
        //                ProductName = s.ProductName,
        //                Quantity = s.Quantity,
        //                TransactionDate = s.TransactionDate,
        //                TransactionType = s.TransactionType,
        //                ErrorMessage = p?.ErrorMessage
        //            }).ToList();
        

        return (productsResponse.isSucess && transactionsResponse.isSucess, null, null);
    }

    public (bool isSucess, List<ProductResponse> failedProducts) SaveProducts(List<Product> products)
    {
        List<ProductResponse> failedProducts = new List<ProductResponse>();
        for (int i = 0; i < products.Count; i = i + batchLimit)
        {
            var items = products.Skip(i).Take(batchLimit);
            try
            {
                adWorksContext.AddRange(items);
                adWorksContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var productsResponses = mapper.Map<List<ProductResponse>>(items);
                foreach(var product in productsResponses)
                    product.ErrorMessage = ex.Message;
                
                failedProducts.AddRange(productsResponses);
                adWorksContext.ChangeTracker.Clear();
            }
        }
        return (!failedProducts.Any(), failedProducts);
    }

    public (bool isSucess, List<TransactionHistoryResponse> failedTransactions) SaveTransactionHistories(List<TransactionHistory> transactionHistories)
    {
        List<TransactionHistoryResponse> failedTransactions = new List<TransactionHistoryResponse>();
        for (int i = 0; i < transactionHistories.Count; i = i + batchLimit)
        {
            var items = transactionHistories.Skip(i).Take(batchLimit);
            try
            {
                adWorksContext.AddRange(items);
                adWorksContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var transacRes = mapper.Map<List<TransactionHistoryResponse>>(items);
                foreach(var trans in transacRes)
                    trans.ErrorMessage = ex.Message;
                transactionHistories.AddRange(items);
                adWorksContext.ChangeTracker.Clear();
            }
        }
        return (!failedTransactions.Any(), failedTransactions);
    }
}
