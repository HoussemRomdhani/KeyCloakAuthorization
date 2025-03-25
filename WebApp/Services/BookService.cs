using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services;

public class BookService
{
    private readonly Lock purchaseLock = new();

    private readonly BookRepository repository;
   
    public BookService(BookRepository repository)
    {
        this.repository = repository;
    }

    public IList<Book> GetAll()
    {
        return repository.GetAll();
    }

    public Book? GetById(int id)
    {
        return repository.GetById(id);
    }

    public void Add(Book book)
    {
        repository.Add(book);
    }

    public void Delete(int id)
    {
        repository.Delete(id);
    }

    public bool Purchase(int bookId, int quantity)
    {
        lock (purchaseLock)
        {
            Book? book = repository.GetById(bookId);

            if (book == null)
            {
                return false;
            }

            int newQuantity = book.Quantity - quantity;

            return repository.UpdateBookQuantity(book.Id, newQuantity);
        }
    }
}

