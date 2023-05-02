namespace ApiClient.Models;

public class CurrencyModel
{
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Rate { get; set; }
    public AttributeModel[] Attributes { get; set; }
}
