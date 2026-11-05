using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class BorrowerRepository : IBorrowerRepository
    {
        public BorrowerRepository()
        {
        }
        public List<Borrower> GetBorrowers()
        {
            using (var context = new LibraryContext())
            {
                var list = context.Borrowers
                    .Include(x => x.Fines)
                    .ToList();
                return list;
            }
        }

        public Guid AddBorrower(Borrower borrower)
        {
            using (var context = new LibraryContext())
            {
                context.Borrowers.Add(borrower);
                context.SaveChanges();
                return borrower.Id;
            }
        }

        public void AddFine(Fine fine)
        {
            using (var context = new LibraryContext())
            {
                context.Fines.Add(fine);
                context.SaveChanges();
            }
        }

        public bool CheckIfBorrowerExists(Guid id)
        {
            using (var context = new LibraryContext())
            {
                var borrower = context.Borrowers.FirstOrDefault(x => x.Id == id);
                return borrower != null;
            }
        }
    }
}
