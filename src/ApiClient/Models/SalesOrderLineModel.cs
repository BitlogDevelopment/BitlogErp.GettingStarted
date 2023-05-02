namespace ApiClient.Models;

public class SalesOrderLineModel
{
    public int LineNo { get; set; }
    public string StockCode { get; set; }
    public decimal QuantityOrdered { get; set; }
    public decimal QuantityDelivered { get; set; }
    public string UnitCode { get; set; }
    public decimal UnitFactor { get; set; }
    public string Warehouse { get; set; }
    public DateTime DateRequested { get; set; }
    public DateTime? DateConfirmed { get; set; }
    public string Description1 { get; set; }
    public string Description2 { get; set; }
    public decimal? Price { get; set; }
    public string CurrencyCode { get; set; }
    public SalesOrderLineType LineType { get; set; }
    public int? ParentLineNo { get; set; }
    public decimal? VatPercent { get; set; }
    public string Notes { get; set; }

    public AttributeModel[] Attributes { get; set; }
}
