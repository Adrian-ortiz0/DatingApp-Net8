namespace DatingApp.Helpers;

public class PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
{
    public int currentPage { get; set; } = currentPage;
    public int itemsPerPage { get; set; } = itemsPerPage;
    public int totalItems { get; set; } = totalItems;
    public int totalPages { get; set; } = totalPages;
}