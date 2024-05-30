using System;
using System.Collections.Generic;

namespace FirstWebApp.EF
{
    public partial class Book
    {
        public Book()
        {
            BookAuthors = new HashSet<BookAuthor>();
        }

        public int BookId { get; set; }
        public string Name { get; set; } = null!;
        public int? Pages { get; set; }
        public int? Year { get; set; }
        public int? GenreId { get; set; }

        public virtual Genre? Genre { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
