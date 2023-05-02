using System.Net;
using ApiClient.Configuration;
using ApiClient.Endpoints;
using ApiClient.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;

namespace ApiClient;

public class App
{
    private readonly IBitlogEndpoints _bitlog;
    private readonly string _resetKey;

    public App(IBitlogEndpoints endpoints, IOptions<BitlogConfiguration> options)
	{
        _bitlog = endpoints;
        _resetKey = options.Value.ResetKey;

        ShowData("Bitlog API client");
        ShowData($" Host: {options.Value.ApiHost}");
    }

    public async Task RunAsync()
    {
        bool exit = false;

        do
        {
            ShowPrompt();

            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;

            try
            {
                switch (input[0])
                {
                    case 'm':
                        ShowAppMenu();
                        break;

                    case 'a':
                        if (input.Length == 1)
                        {
                            ShowCompanyMenu();
                            break;
                        }

                        switch (input[1])
                        {
                            case 'a':
                                await GetCompanyAttributesAsync();
                                break;
                            case 'g':
                                await GetCompanyAttributeAsync();
                                break;
                            case 'i':
                                await GetCompanyAsync();
                                break;
                            case 'u':
                                await UpdateCompanyAttributeAsync();
                                break;
                            default:
                                ShowError("Invalid command");
                                break;
                        }
                        break;

                    case 'c':
                        if (input.Length == 1)
                        {
                            ShowCurrencyMenu();
                            break;
                        }

                        switch (input[1])
                        {
                            case 'a':
                                await GetCurrenciesAsync();
                                break;
                            case 'c':
                                await CreateCurrencyAsync("AAA");
                                break;
                            case 'g':
                                await GetCurrencyAsync("AAA");
                                break;
                            case 'u':
                                await UpdateCurrencyAsync("AAA");
                                break;
                            case 'd':
                                await DeleteCurrencyAsync("AAA");
                                break;
                            default:
                                ShowError("Invalid command");
                                break;
                        }
                        break;

                    case 'l':
                        if (input.Length == 1)
                        {
                            ShowLanguageMenu();
                            break;
                        }

                        switch (input[1])
                        {
                            case 'a':
                                await GetLanguagesAsync();
                                break;
                            case 'c':
                                await CreateLanguageAsync();
                                break;
                            case 'r':
                                await ResetLanguagesAsync();
                                break;
                            default:
                                ShowError("Invalid command");
                                break;
                        }
                        break;

                    case 's':
                        if (input.Length == 1)
                        {
                            ShowSalesMenu();
                            break;
                        }

                        switch (input[1])
                        {
                            case 'a':
                                await GetSalesOrdersAsync();
                                break;
                            case 'c':
                                await CreateSalesOrderAsync();
                                break;
                            case 'g':
                                await GetSalesOrderAsync();
                                break;
                            default:
                                ShowError("Invalid command");
                                break;
                        }
                        break;

                    case 't':
                        await ProcessTransactionAsync();
                        break;

                    case 'w':
                        await GetWarehousesAsync();
                        break;

                    case 'x':
                        exit = true;
                        break;

                    default:
                        ShowError("Invalid command");
                        break;
                }
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ShowError("Not found");
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                ShowError("Already exists");
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ShowBadRequest(ex);
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                ShowBadModel(ex);
            }
            catch (ApiException ex)
            {
                ShowError($"{ex.StatusCode}: {ex.Message}");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

        } while (!exit);
    }

    private async Task GetCompanyAsync()
    {
        Console.WriteLine("Retrieving company...");
        var response = await _bitlog.GetCompany();
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var company = response.Result;

        ShowData($" Company code: {company.Code}");
        ShowData($" Company name: {company.Name}");
        ShowData($" Currency: {company.Currency}");
        ShowData($" Warehouse: {company.Warehouse}");
    }
    private async Task UpdateCompanyAttributeAsync()
    {
        Console.WriteLine("Updating company attribute...");
        var attribute = new AttributeModel
        {
            Key = "Homepage",
            Value = "www.bitlog.se"
        };

        await _bitlog.UpdateCompanyAttribute("info", "Homepage", attribute);
    }
    private async Task GetCompanyAttributesAsync()
    {
        Console.WriteLine("Retrieving company attributes...");
        var response = await _bitlog.GetCompanyAttributes("info");
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var category = response.Result;
        ShowData($"Category: {category.Category}");
        ShowAttributes(category.Attributes);
    }
    private async Task GetCompanyAttributeAsync()
    {
        Console.WriteLine("Retrieving company attribute...");
        var response = await _bitlog.GetCompanyAttribute("info", "Homepage");
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        ShowAttribute(response.Result);
    }

    private async Task GetCurrenciesAsync()
    {
        Console.WriteLine("Retrieving currencies...");
        var response = await _bitlog.GetCurrencies();
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var items = response.Result;
        Console.WriteLine($"Count: {items.Count}");
        items.ForEach(x => ShowData($" {x.Code}: {x.Name}"));
    }
    private async Task GetCurrencyAsync(string currencyCode)
    {
        Console.WriteLine($"Retrieving currency {currencyCode}...");
        var response = await _bitlog.GetCurrency(currencyCode);
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var currency = response.Result;

        ShowData($" Currency code: {currency.Code}");
        ShowData($" Currency name: {currency.Name}");
        ShowData($" Currency rate: {currency.Rate:#.###}");
        ShowAttributes(currency.Attributes);
    }
    private async Task CreateCurrencyAsync(string currencyCode)
    {
        Console.WriteLine($"Creating currency {currencyCode}...");
        var currency = new CurrencyModel
        {
            Code = currencyCode,
            Name = "My currency",
            Rate = 1
        };

        await _bitlog.CreateCurrency(currency);
    }
    private async Task UpdateCurrencyAsync(string currencyCode)
    {
        Console.WriteLine($"Updating currency {currencyCode}...");
        var currency = new CurrencyModel
        {
            Code = currencyCode,
            Name = "My currency",
            Rate = 10,
            Attributes = new AttributeModel[]
            {
                new AttributeModel
                {
                    Key = "Continent",
                    Value = "Europe"
                }
            }
        };

        await _bitlog.UpdateCurrency(currency);
    }
    private async Task DeleteCurrencyAsync(string currencyCode)
    {
        Console.WriteLine($"Deleting currency {currencyCode}...");
        await _bitlog.DeleteCurrency(currencyCode);
    }

    private async Task GetCustomersAsync()
    {
        Console.WriteLine("Retrieving customers...");
        var response = await _bitlog.GetCustomers(new PaginationModel(1, 10));
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var customers = response.Result;
        Console.WriteLine($"Customers retrieved: {customers.Count}");
        customers.ForEach(x => ShowData($" {x.Code}: {x.Name}"));
    }
    private async Task GetCustomerAsync(string customerCode)
    {
        Console.WriteLine($"Retrieving customer {customerCode}...");
        var response = await _bitlog.GetCustomer(customerCode);
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var customer = response.Result;
        ShowData($" Customer code: {customer.Code}");
        ShowData($" Customer name: {customer.Name}");
    }

    private async Task GetProductsAsync()
    {
        Console.WriteLine("Retrieving products...");
        var response = await _bitlog.GetProducts(new PaginationModel(1, 10));
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var products = response.Result;
        Console.WriteLine($"Count: {products.Count}");
        products.ForEach(x => ShowData($" {x.StockCode}: {x.Description1}"));
    }
    private async Task GetProductAsync(string stockCode)
    {
        Console.WriteLine($"Retrieving product {stockCode}...");
        var response = await _bitlog.GetProduct(stockCode);
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var product = response.Result;
        ShowData($" Stock code:  {product.StockCode}");
        ShowData($" Description: {product.Description1}");
        ShowData($" Sales price: {product.SalesPrice:#,##0.##}");
    }

    private async Task GetSalesOrdersAsync()
    {
        Console.WriteLine("Retrieving sales orders...");
        var response = await _bitlog.GetSalesOrders(new PaginationModel(1, 10));
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var orders = response.Result;
        Console.WriteLine($"Orders retrieved: {orders.Count}");
        orders.ForEach(x => ShowData($" {x.OrderNo}: {x.CustomerName}"));
    }
    private async Task GetSalesOrderAsync()
    {
        Console.WriteLine("Retrieving sales order...");
        var response = await _bitlog.GetSalesOrder("GS100");
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var order = response.Result;
        ShowData($" OrderNo:  {order.OrderNo}");
        ShowData($" Date:     {order.OrderDate:g}");
        ShowData($" Customer: {order.CustomerCode}");
        ShowData($" Name:     {order.CustomerName}");
        ShowData($" Total:    {order.OrderTotal:#,##0.##}");
        ShowData(" Order lines:");
        foreach (var line in order.Lines)
            ShowOrderLine(line);
    }
    private async Task CreateSalesOrderAsync()
    {
        Console.WriteLine("Creating sales order...");

        var order = new SalesOrderModel
        {
            OrderNo = "GS100",
            OrderDate = DateTime.Today,
            OrderType = SalesOrderType.Sales,
            CurrencyCode = "EUR",
            CompleteShipment = true,
            CustomerCode = "GC100",
            CustomerName = "Bob Hope",
            DeliveryDate = DateTime.Today,
            DeliveryMethod = "UPS",
            OrderStatus = SalesOrderStatus.Open,
            OrderTotal = 500,
            PaymentTerms = "CASH",
            Warehouse = "UP",
            Address = new AddressModel
            {
                City = "Stockholm",
                Country = "SE",
                Line1 = "Kistagången 20B",
                Line2 = "16432 Kista",
                Zip = "16432"
            },
            Lines = new SalesOrderLineModel[]
            {
                new SalesOrderLineModel
                {
                    LineNo = 1,
                    StockCode = "GP100",
                    Description1 = "Apple Watch",
                    Description2 = "Gen3",
                    DateRequested = DateTime.Today,
                    CurrencyCode = "EUR",
                    LineType = SalesOrderLineType.Stock,
                    Price = 500,
                    QuantityOrdered = 1,
                    UnitCode = "PCS",
                    VatPercent = 25,
                    Warehouse = "UP"
                },
                new SalesOrderLineModel
                {
                    LineNo = 2,
                    StockCode = "GP101",
                    Description1 = "Bonus check",
                    Description2 = "100€",
                    DateRequested = DateTime.Today,
                    LineType = SalesOrderLineType.Intangible,
                    QuantityOrdered = 1,
                    UnitCode = "PCS",
                    Warehouse = "UP"
                }
            }
        };

        await _bitlog.CreateSalesOrder(order);
    }

    private async Task GetLanguagesAsync()
    {
        Console.WriteLine("Retrieving languages...");
        var response = await _bitlog.GetLanguages();
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var items = response.Result;
        Console.WriteLine($"Count: {items.Count}");
        items.ForEach(x => ShowData($" {x.Code}: {x.Name}"));
    }
    private async Task CreateLanguageAsync()
    {
        Console.WriteLine("Creating language...");
        var langauge = new LanguageModel
        {
            Code = "en-us",
            Name = "US English"
        };

        await _bitlog.CreateLanguage(langauge);
    }
    private async Task ResetLanguagesAsync()
    {
        Console.WriteLine("Resetting languages...");
        await _bitlog.ResetLanguages(_resetKey);
    }

    private async Task GetWarehousesAsync()
    {
        Console.WriteLine("Retrieving warehouses...");
        var response = await _bitlog.GetWarehouses();
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var items = response.Result;
        Console.WriteLine($"Count: {items.Count}");
        items.ForEach(x => ShowData($" {x.WarehouseCode}: {x.WarehouseName}"));
    }

    private async Task ProcessTransactionAsync()
    {
        Console.WriteLine("Retrieving next transaction...");
        var response = await _bitlog.GetNextTransaction();
        if (!response.IsSuccess)
        {
            ShowError(response);
            return;
        }

        var transaction = response.Result;
        ShowData($" Id:    {transaction.Id}");
        ShowData($" Type:  {transaction.Type}");
        ShowData($" Order: {transaction.OrderNo}");
        ShowData(" Lines:");
        foreach (var line in transaction.Lines)
            ShowTransactionLine(line);

        // Handle transaction
        var result = new TransactionResultModel
        {
            Result = 1,
            Message = "success"
        };

        ShowData("Updating transaction result...");
        await _bitlog.UpdateTransactionResult(transaction.Id, result);
    }

    private static void ShowAttributes(AttributeModel[] attributes)
    {
        if (attributes is not null)
            foreach (var attribute in attributes)
                ShowAttribute(attribute);
    }
    private static void ShowAttribute(AttributeModel attribute)
    {
        ShowData($" {attribute.Key}: {attribute.Value}");
    }
    private static void ShowBadRequest(ApiException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Bad request");

        ShowError(JsonConvert.DeserializeObject<ResponseModel<object>>(ex.Content));
    }
    private static void ShowBadModel(ApiException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Bad model");

        ShowError(JsonConvert.DeserializeObject<ResponseModel<object>>(ex.Content));
    }
    private static void ShowData(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message);
    }
    private static void ShowError<T>(ResponseModel<T> response)
    {
        ShowError($"Error {response.ResultCode}: {response.Message}");

        if (response.ErrorDetails is not null)
            foreach (var error in response.ErrorDetails)
                ShowError(error?.ToString());
    }
    private static void ShowError(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
    }
    private static void ShowOrderLine(SalesOrderLineModel line)
    {
        ShowData($" {line.LineNo}: {line.StockCode}");
        ShowData($"  Description: {line.Description1}");
        ShowData($"  Quantity:    {line.QuantityOrdered:#,##0.##}");
    }
    private static void ShowTransactionLine(TransactionLineModel line)
    {
        ShowData($" {line.LineNo}: {line.StockCode}");
        ShowData($"  Quantity:    {line.Quantity:#,##0.##}");
    }
    private static void ShowPrompt()
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write("api>");
        Console.ResetColor();
    }

    private static void ShowAppMenu()
    {
        Console.WriteLine("API menu:");
        Console.WriteLine(" a: Company");
        Console.WriteLine(" c: Currencies");
        Console.WriteLine(" l: Languages");
        Console.WriteLine(" s: Sales orders");
        Console.WriteLine(" t: Transactions");
        Console.WriteLine(" w: Warehouses");
        Console.WriteLine(" x: Exit");
    }
    private static void ShowCompanyMenu()
    {
        Console.WriteLine("Company commands:");
        Console.WriteLine(" i: Get company information");
        Console.WriteLine(" a: Get all attributes");
        Console.WriteLine(" g: Get attribute");
        Console.WriteLine(" u: Update attribute");
    }
    private static void ShowCurrencyMenu()
    {
        Console.WriteLine("Currency commands:");
        Console.WriteLine(" a: Get all currencies");
        Console.WriteLine(" g: Get currency");
        Console.WriteLine(" c: Create currency");
        Console.WriteLine(" u: Update currency");
        Console.WriteLine(" d: Delete currency");
    }
    private static void ShowLanguageMenu()
    {
        Console.WriteLine("Language commands:");
        Console.WriteLine(" a: Get all languages");
        Console.WriteLine(" c: Create language");
        Console.WriteLine(" r: Reset languages (delete all)");
    }
    private static void ShowSalesMenu()
    {
        Console.WriteLine("Sales order commands:");
        Console.WriteLine(" a: Get all orders");
        Console.WriteLine(" g: Get order");
        Console.WriteLine(" c: Create order");
    }
}
