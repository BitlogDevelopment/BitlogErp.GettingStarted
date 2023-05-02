namespace ApiClient.Models;

public enum SalesOrderLineType
{
    Stock,
    Intangible,
    Structure,
    Part
}
public enum SalesOrderStatus
{
    Unknown,
    Open,
    Delivered,
    Cancelled
}
public enum SalesOrderType
{
    Sales,
    Return,
    Transfer,
    CashierSales,
    CashierReturn
}
public enum TransactionType
{
    In,
    Out,
    Move,
    Take,
    CreateReturnOrder,
    CreateReturnLine,
    ReceiveReturnLine
}
public enum TransactionStatus
{
    Waiting,
    Processed,
    Failed,
    Blocked,
    Resolved,
    Preparing
}