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
        private readonly Lazy<IDoctorRepository> _doctorRepository;
        private readonly Lazy<IReviewRepository> _reviewRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(repositoryContext));
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(repositoryContext));
            _shiftRepository = new Lazy<IShiftRepository>(() => new ShiftRepository(repositoryContext));
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(repositoryContext));
            _doctorRepository = new Lazy<IDoctorRepository>(() => new DoctorRepository(repositoryContext));
            _reviewRepository = new Lazy<IReviewRepository>(() => new ReviewRepository(repositoryContext));
        }
        public ICompanyRepository Company => _companyRepository.Value;
        public IEmployeeRepository Employee => _employeeRepository.Value;
        public IShiftRepository Shift => _shiftRepository.Value;
        public IProductRepository Product => _productRepository.Value;
        public IDoctorRepository Doctor => _doctorRepository.Value;
        public IReviewRepository Review => _reviewRepository.Value;
        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
