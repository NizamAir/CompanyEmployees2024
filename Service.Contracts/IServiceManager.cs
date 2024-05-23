namespace Service.Contracts
{
    public interface IServiceManager
    {
        ICompanyService CompanyService { get; }
        IEmployeeService EmployeeService { get; }
        IAuthenticationService AuthenticationService { get; }
        IShiftService ShiftService { get; }
        IProductService ProductService { get; }
        IDoctorService DoctorService { get; }
    }
}
