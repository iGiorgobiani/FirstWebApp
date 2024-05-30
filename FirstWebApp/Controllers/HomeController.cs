using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FirstWebApp.EF;
using Microsoft.EntityFrameworkCore;
using FirstWebApp.Models.Book;
using FirstWebApp.Models.Author;
using X.PagedList;

namespace FirstWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(BookViewModel model)
        {
            LibraryContext context = new LibraryContext();

            var queryResult = context.Books
                .Include(x => x.Genre)
                .Include(x => x.BookAuthors)
                .ThenInclude(x => x.Author)
                .AsQueryable();

            if(!string.IsNullOrEmpty(model.Name))
            {
                queryResult = queryResult.Where(x => x.Name.Contains(model.Name));
            }

            if (!string.IsNullOrEmpty(model.Firstname))
            {
                queryResult = queryResult.Where(x => x.BookAuthors.Any(a => a.Author.Firstname.Contains(model.Firstname)));
            }

            if (!string.IsNullOrEmpty(model.Lastname))
            {
                queryResult = queryResult.Where(x => x.BookAuthors.Any(a => a.Author.Lastname.Contains(model.Lastname)));
            }

            var bookListItem = queryResult.Select(x => new BookListItem()
            {
                BookId = x.BookId,
                Name = x.Name,
                Year = x.Year ?? 0,
                Genre = x.Genre.Name,
                Authors = x.BookAuthors.Select(a => new AuthorViewModel()
                {
                    Firstname = a.Author.Firstname,
                    Lastname = a.Author.Lastname
                }).ToList()
            });

            model.Books.AddRange(bookListItem);

            return View(model);
        }

        public IActionResult RemoveBook(int bookId)
        {

            LibraryContext context = new LibraryContext();

            var book = context.Books
               .Include(x => x.BookAuthors)
                .SingleOrDefault(x => x.BookId == bookId);
            //context.BookAuthors.RemoveRange(book.BookAuthors);
            context.Books.Remove(book);
            _ = context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Authors(AuthorsViewModel model, int? page)
        {
            LibraryContext context = new LibraryContext();

            var queryResult = context.Authors
                .Include(x => x.BookAuthors)
                .AsQueryable();


            if (!string.IsNullOrEmpty(model.Firstname))
            {
                queryResult = queryResult.Where(x => x.Firstname.Contains(model.Firstname));
            }

            if (!string.IsNullOrEmpty(model.Lastname))
            {
                queryResult = queryResult.Where(x => x.Lastname.Contains(model.Lastname));
            }

            int pageNumber = page ?? 1;
            int numberOfItemsPerPage = 10;
            model.Total = queryResult.Count();

            model.Authors = queryResult.Select(x => new AuthorListItem()
            {
                AuthorId = x.AuthorId,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                Birthdate = x.Birthdate,
                Booknumber = x.BookAuthors.Count
            }).ToPagedList(pageNumber, numberOfItemsPerPage);


            return View(model);
        }

        public IActionResult AddAuthor()
        { 

            return View(); 
        }

        [HttpPost]
		public IActionResult AddAuthor(AddAuthorModel model)
		{
            if (ModelState.IsValid) 
            {
                LibraryContext context = new LibraryContext();

                context.Authors.Add(new Author()
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Birthdate = model.Birthdate,
                });
                context.SaveChanges();

                return RedirectToAction("Authors");
            }
			return View(model);
		}
        //
		public IActionResult EditAuthor(int authorId)
        {
            LibraryContext context = new LibraryContext();
            if (context.Authors.Any(x => x.AuthorId == authorId))
            {
                var author = context.Authors.SingleOrDefault(x => x.AuthorId == authorId);

                var model = new EditAuthorModel()
                {
                    AuthorId = author.AuthorId,
                    Firstname = author.Firstname,
                    Lastname = author.Lastname,
                    //Birthdate = string.Format("{0: mm.DD.yyyy}" , author.Birthdate)


                };
                return View(model);
            }
            return RedirectToAction("Authors");
           
		}
		[HttpPost]
		public IActionResult EditAuthor(EditAuthorModel model)
		{
			if (ModelState.IsValid)
			{
				LibraryContext context = new LibraryContext();

				var author = context.Authors.SingleOrDefault(x => x.AuthorId == model.AuthorId);

                author.Firstname = model.Firstname;
                author.Lastname = model.Lastname;
                //author.Birthdate = model.Birthdate;

				context.SaveChanges();

				return RedirectToAction("Authors");
			}
			return View(model);
		}

        public IActionResult RemoveAuthor(int authorId)
        {

            LibraryContext context = new LibraryContext();

            var author = context.Authors
                .Include(x => x.BookAuthors)
                .SingleOrDefault(x => x.AuthorId == authorId);
            context.BookAuthors.RemoveRange(author.BookAuthors);
            context.Authors.Remove(author);
            context.SaveChanges();
            
            return RedirectToAction("Authors");
        }
    }
}