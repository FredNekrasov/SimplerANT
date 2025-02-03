namespace ANTWebAPI.DTOs;

public class PagedResponse<T>(long pageNumber, long pageSize, long totalRecords, ICollection<T> data)
{
    public long PageNumber { get; set; } = pageNumber;
    public long PageSize { get; set; } = pageSize;
    public long TotalRecords { get; set; } = totalRecords;
    public ICollection<T> Data { get; set; } = data;
}
