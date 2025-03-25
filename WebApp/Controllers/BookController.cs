using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Constants;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class BookController : Controller
{
    private readonly BookService bookService;
  
    public BookController(BookService bookService)
    {
       this.bookService = bookService;
    }

    public ActionResult Index()
    {
        IList<BookViewModel> vm = bookService.GetAll().Select(MapToBookViewModel).ToList();
       
        return View(vm);
    }

    public ActionResult Details(int id)
    {
        Book? book = bookService.GetById(id);
      
        if (book == null)
        {
            return NotFound();
        }

        BookViewModel vm = MapToBookViewModel(book);

        return View(vm);
    }

    [Authorize(Roles = Roles.Author)]
    public ActionResult Create()
    {
        CreateBookViewModel vm = new()
        {
            Year = BookYearRange.MIN
        };

        return View(vm);
    }

    [HttpGet]
    public IActionResult Purchase(int id)
    {
        var book = bookService.GetById(id);

        if (book == null)
        {
            return NotFound();
        }

        var model = new PurchaseViewModel
        {
            BookId = book.Id,
            BookTitle = book.Title, 
            Quantity = 1
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Roles.Author)]
    public ActionResult Create(CreateBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if ((User.FindFirst(ClaimTypes.NameIdentifier)?.Value) == null ||
            !int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
          return Unauthorized();
        }

        Book book = new()
        {
            Title = model.Title,
            Quantity = model.Quantity,
            AuthorId = userId,
            Year = model.Year,
        };

        bookService.Add(book);

        return RedirectToAction(nameof(Index));
    }


    [Authorize(Roles = Roles.Author, Policy = "MustOwnBook")]
    public ActionResult Delete(int id)
    {
        Book? book = bookService.GetById(id);

        if (book == null)
        {
            return NotFound();
        }

        BookViewModel vm = MapToBookViewModel(book);

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Roles.Author, Policy = "MustOwnBook")]
    public ActionResult DeletePost(int id)
    {
        bookService.Delete(id);
       
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Purchase(PurchaseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Book? book = bookService.GetById(model.BookId);

        if (book == null)
        {
            return NotFound();
        }

        if (model.Quantity > book.Quantity)
        {
            ModelState.AddModelError("Quantity", $"Only {book.Quantity} books available.");
            return View(model); 
        }

        bookService.Purchase(model.BookId, model.Quantity);

        return RedirectToAction("Index", "Book");
    }

    private static BookViewModel MapToBookViewModel(Book book)
    {
        return new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Quantity = book.Quantity,
            Author = book.Author,
            Year = book.Year
        };
    }
}
