namespace ANTWebAPI.DTOs;

public record PagedResponse<T>(long pageNumber, long pageSize, long totalRecords, ICollection<T> data);
