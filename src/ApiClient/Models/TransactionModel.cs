namespace ApiClient.Models;

public class TransactionModel
{
    public int Id { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public DateTime Time { get; set; }
    public DateTime? SyncTime { get; set; }
    public int? Result { get; set; }
    public string Error { get; set; }
    public string OrderNo { get; set; }
    public string Warehouse { get; set; }
    public string Remark { get; set; }
    public decimal FreightCosts { get; set; }
    public List<TransactionLineModel> Lines { get; set; }
}

public class TransactionLineModel
{
    public int LineNo { get; set; }
    public string StockCode { get; set; }
    public decimal Quantity { get; set; }
    public string Remark { get; set; }
    public string Warehouse { get; set; }
    public List<string> SerialNumbers { get; set; }
}
public class TransactionResultModel
{
    public int Result { get; set; }
    public string Message { get; set; }
}