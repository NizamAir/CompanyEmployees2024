using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.DoctorDTOs;
using Shared.DataTransferObjects.ProductDTOs;
using Shared.DataTransferObjects.ReviewDTOs;
using Shared.DataTransferObjects.ShiftDTOs;

namespace CompanyEmployees2024
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForCtorParam("FullAddress", opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));


            CreateMap<Employee, EmployeeDto>();

            CreateMap<CompanyForCreationDto, Company>();

            CreateMap<EmployeeForCreationDto, Employee>();

            CreateMap<EmployeeForUpdateDto, Employee>();

            CreateMap<CompanyForUpdateDto, Company>();

            CreateMap<UserForRegistrationDto, User>();

            CreateMap<Shift, ShiftDto>()
                .ForCtorParam("ShiftDate", opt => opt.MapFrom(x => x.ShiftDate.ToString("dd/MM/yyyy")))
                .ForCtorParam("ShiftTime", opt => opt.MapFrom(x => x.ShiftDate.ToString("HH:mm")))
                .ForCtorParam("DoctorName", opt => opt.MapFrom(x => string.Join(' ', x.DoctorUser.FirstName.ToString(), x.DoctorUser.LastName.ToString())))
                .ForCtorParam("AssistantName", opt => opt.MapFrom(x => string.Join(' ', x.AssistentUser.FirstName.ToString(), x.AssistentUser.LastName.ToString())))
                .ForCtorParam("ClientName", opt => opt.MapFrom(x => string.Join(' ', x.ClientUser.FirstName.ToString(), x.ClientUser.LastName.ToString())))
                .ForCtorParam("ProductName", opt => opt.MapFrom(x => x.Product.Name));

            CreateMap<ShiftForDoctorCreationDto, Shift>();
            CreateMap<ShiftForUpdateDto, Shift>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductForCreationDto, Product>();
            CreateMap<ProductForUpdateDto, Product>();

            CreateMap<Doctor, DoctorDto>();
            CreateMap<DoctorForInitialDto, Doctor>();
            CreateMap<DoctorForUpdateDto, Doctor>();

            CreateMap<Review, ReviewDto>()
                .ForCtorParam("CreationDate", opt => opt.MapFrom(x => x.CreationDate.ToString("dd/MM/yyyy")))
                .ForCtorParam("CreationTime", opt => opt.MapFrom(x => x.CreationDate.ToString("HH:mm")))
                .ForCtorParam("ClientName", opt => opt.MapFrom(x => string.Join(' ', x.UserWhoRated.FirstName.ToString(), x.UserWhoRated.LastName.ToString())));
            CreateMap<ReviewForCreationDto, Review>();
        }
    }
}
