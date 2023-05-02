namespace ApiClient.Models;

public class PaginationModel
{
	public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public PaginationModel(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

