using Contracts;

namespace Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<ICompanyRepository> _companyRepository;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;
        private readonly Lazy<IShiftRepository> _shiftRepository;
        private readonly Lazy<IProductRepository> _productRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(repositoryContext));
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(repositoryContext));
            _shiftRepository = new Lazy<IShiftRepository>(() => new ShiftRepository(repositoryContext));
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(repositoryContext));
        }
        public ICompanyRepository Company => _companyRepository.Value;
        public IEmployeeRepository Employee => _employeeRepository.Value;
        public IShiftRepository Shift => _shiftRepository.Value;
        public IProductRepository Product => _productRepository.Value;
        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
