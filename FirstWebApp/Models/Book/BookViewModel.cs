namespace FirstWebApp.Models.Book;

public class BookViewModel
{
    //Filters
    public string? Name { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public List<BookListItem>? Books { get; set; } = new List<BookListItem>();
}
