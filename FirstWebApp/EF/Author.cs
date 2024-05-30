using System;
using System.Collections.Generic;

namespace FirstWebApp.EF
{
    public partial class Author
    {
        public Author()
        {
            BookAuthors = new HashSet<BookAuthor>();
        }

        public int AuthorId { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public DateTime? Birthdate { get; set; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
