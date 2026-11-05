namespace OneBeyondApi.Services;

public interface ILoanService
{
    public void ReturnBook(Guid bookStockId);
    public void Reserve(Guid bookStockId, Guid borrowerId);
}
