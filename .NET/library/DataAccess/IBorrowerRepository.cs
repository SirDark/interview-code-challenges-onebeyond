using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IBorrowerRepository
    {
        public List<Borrower> GetBorrowers();

        public Guid AddBorrower(Borrower borrower);
        public void AddFine(Fine fine);
        public bool CheckIfBorrowerExists(Guid id);
    }
}
