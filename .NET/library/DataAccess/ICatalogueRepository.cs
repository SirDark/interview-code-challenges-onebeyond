using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface ICatalogueRepository
    {
        public List<BookStock> GetCatalogue();

        public List<BookStock> SearchCatalogue(CatalogueSearch search);

        public List<BookStock>? GetBorrowers();
        public BookStock ReturnBook(Guid id);

        public BookStock GetBookStock(Guid id);
    }
}
