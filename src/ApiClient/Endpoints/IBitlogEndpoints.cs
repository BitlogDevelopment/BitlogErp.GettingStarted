using ApiClient.Models;
using Refit;

namespace ApiClient.Endpoints;

public interface IBitlogEndpoints
{
    #region Company

    [Get("/api/company")]
    Task<ResponseModel<CompanyModel>> GetCompany();
    [Put("/api/company/attribute/{category}/{key}")]
    Task UpdateCompanyAttribute(string category, string key, [Body] AttributeModel attribute);
    [Get("/api/company/attribute/{category}")]
    Task<ResponseModel<AttributeCategoryModel>> GetCompanyAttributes(string category);
    [Get("/api/company/attribute/{category}/{key}")]
    Task<ResponseModel<AttributeModel>> GetCompanyAttribute(string category, string key);

    #endregion
    #region Currency

    [Get("/api/currency")]
    Task<ResponseModel<List<CurrencyModel>>> GetCurrencies();
    [Get("/api/currency/{code}")]
    Task<ResponseModel<CurrencyModel>> GetCurrency(string code);
    [Post("/api/currency")]
    Task CreateCurrency([Body] CurrencyModel currency);
    [Put("/api/currency")]
    Task UpdateCurrency([Body] CurrencyModel currency);
    [Delete("/api/currency/{code}")]
    Task DeleteCurrency(string code);
    [Delete("/api/currency")]
    Task ResetCurrencies(string resetKey);

    #endregion
    #region Customer

    [Get("/api/customer")]
    Task<ResponseModel<List<CustomerModel>>> GetCustomers([Query] PaginationModel pagination);
    [Get("/api/customer/{code}")]
    Task<ResponseModel<CustomerModel>> GetCustomer(string code);

    #endregion
    #region Language

    [Get("/api/language")]
    Task<ResponseModel<List<LanguageModel>>> GetLanguages();
    [Get("/api/language/{code}")]
    Task<ResponseModel<LanguageModel>> GetLanguage(string code);
    [Post("/api/language")]
    Task CreateLanguage([Body] LanguageModel language);
    [Put("/api/language")]
    Task UpdateLanguage([Body] LanguageModel language);
    [Delete("/api/language/{code}")]
    Task DeleteLanguage(string code);
    [Delete("/api/language")]
    Task ResetLanguages(string resetKey);

    #endregion
    #region Product

    [Get("/api/product")]
    Task<ResponseModel<List<ProductModel>>> GetProducts([Query] PaginationModel pagination);
    [Get("/api/product/{code}")]
    Task<ResponseModel<ProductModel>> GetProduct(string code);

    #endregion
    #region Sales order

    [Get("/api/sales/order")]
    Task<ResponseModel<List<SalesOrderModel>>> GetSalesOrders([Query] PaginationModel pagination);
    [Get("/api/sales/order/{orderNo}")]
    Task<ResponseModel<SalesOrderModel>> GetSalesOrder(string orderNo);
    [Post("/api/sales/order")]
    Task CreateSalesOrder([Body] SalesOrderModel order);
    [Put("/api/sales/order")]
    Task UpdateSalesOrder([Body] SalesOrderModel order);
    [Delete("/api/sales/order/{orderNo}")]
    Task DeleteSalesOrder(string orderNo);
    [Delete("/api/sales/order")]
    Task ResetSalesOrders(string resetKey);

    #endregion
    #region Transactions

    [Get("/api/sync/next")]
    Task<ResponseModel<TransactionModel>> GetNextTransaction();
    [Get("/api/sync/{id}")]
    Task<ResponseModel<TransactionModel>> GetTransaction(int id);
    [Put("/api/sync/{id}")]
    Task UpdateTransactionResult(int id, [Body] TransactionResultModel result);

    #endregion
    #region Warehouse

    [Get("/api/warehouse")]
    Task<ResponseModel<List<WarehouseModel>>> GetWarehouses();
    [Get("/api/warehouse/{code}")]
    Task<ResponseModel<WarehouseModel>> GetWarehouse(string code);
    [Post("/api/warehouse")]
    Task CreateWarehouse([Body] WarehouseModel warehouse);
    [Put("/api/warehouse")]
    Task UpdateWarehouse([Body] WarehouseModel warehouse);
    [Delete("/api/warehouse/{code}")]
    Task DeleteWarehouse(string code);
    [Delete("/api/warehouse")]
    Task ResetWarehouses(string resetKey);

    #endregion
}
