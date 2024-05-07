namespace Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        IShiftRepository Shift { get; }
        IProductRepository Product { get; }
        Task SaveAsync();
    }
}
