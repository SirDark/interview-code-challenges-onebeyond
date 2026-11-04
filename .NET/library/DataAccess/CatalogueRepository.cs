using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Exceptions;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess;

public class CatalogueRepository : ICatalogueRepository
{
    public CatalogueRepository()
    {
    }
    public List<BookStock> GetCatalogue()
    {
        using (var context = new LibraryContext())
        {
            var list = context.Catalogue
                .Include(x => x.Book)
                .ThenInclude(x => x.Author)
                .Include(x => x.OnLoanTo)
                .ToList();
            return list;
        }
    }

    public List<BookStock> SearchCatalogue(CatalogueSearch search)
    {
        using (var context = new LibraryContext())
        {
            var list = context.Catalogue
                .Include(x => x.Book)
                .ThenInclude(x => x.Author)
                .Include(x => x.OnLoanTo)
                .AsQueryable();

            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.Author)) {
                    list = list.Where(x => x.Book.Author.Name.Contains(search.Author));
                }
                if (!string.IsNullOrEmpty(search.BookName)) {
                    list = list.Where(x => x.Book.Name.Contains(search.BookName));
                }
            }
                
            return list.ToList();
        }
    }
    public List<BookStock>? GetBorrowers()
    {
        using (var context = new LibraryContext())
        {
            var list = context.Catalogue
                .Include(x => x.OnLoanTo)
                .Include(x => x.Book)
                .Where(x => x.OnLoanTo != null);
            return list.ToList();
        }
    }

    public void ReturnBook(Guid id)
    {
        using (var context = new LibraryContext())
        {
            var bookStock = context.Catalogue
                .Include(x => x.OnLoanTo)
                .FirstOrDefault(x => x.Id == id);

            if (bookStock == null)
                throw new BookStockNotFoundException($"no BookStock found with id: {id}");

            //from what I understand OnLoanTo and LoanEndDate is null ot not null at the same time so it is enough just to check only one of them 
            if (bookStock.OnLoanTo == null)
                throw new BookNotLoanedException($"Bookstock is not loaned with Id: {id}");

            bookStock.LoanEndDate = null;
            bookStock.OnLoanTo = null;

            context.SaveChanges();
        }
    }
}
