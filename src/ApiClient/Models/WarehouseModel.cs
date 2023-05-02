namespace ApiClient.Models;

public class WarehouseModel
{
    public string WarehouseCode { get; set; }
    public string WarehouseName { get; set; }
    public AttributeModel[] Attributes { get; set; }
}

