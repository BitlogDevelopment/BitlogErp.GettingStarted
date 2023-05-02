namespace ApiClient.Models;

public class SalesOrderModel
{
    public string OrderNo { get; set; }
    public DateTime OrderDate { get; set; }
    public SalesOrderStatus OrderStatus { get; set; }
    public SalesOrderType OrderType { get; set; }
    public decimal OrderTotal { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string Warehouse { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string CustomerReference { get; set; }
    public string CustomerOrderNo { get; set; }
    public string OurReference { get; set; }
    public string PaymentTerms { get; set; }
    public string DeliveryTerms { get; set; }
    public string DeliveryMethod { get; set; }
    public string CurrencyCode { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool CompleteShipment { get; set; }
    public string LanguageCode { get; set; }
    public string Notes { get; set; }
    public string PickupId { get; set; }
    public string ReturnOrderNo { get; set; }
    public string Remarks { get; set; }
    public bool BackOrder { get; set; }
    public string SortingKey { get; set; }
    public AddressModel Address { get; set; }

    public SalesOrderLineModel[] Lines { get; set; }
    public AttributeModel[] Attributes { get; set; }
}

