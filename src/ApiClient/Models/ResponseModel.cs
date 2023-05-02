namespace ApiClient.Models;

public sealed class ResponseModel<TResult>
{
    public long ResultCode { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public TResult Result { get; set; }
    public ErrorDetail[] ErrorDetails { get; set; }
}
