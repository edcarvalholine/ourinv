using AutoMapper;
using ourinv.WebAPI.DTOs.CategoryDTO;
using ourinv.WebAPI.DTOs.ProductDTO;
using ourinv.WebAPI.Models;

namespace ourinv.WebAPI
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            AllowNullCollections = true;

            CreateMap<Category, BaseCategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<UpdateCategoryDTO, Category>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NewCategoryName));
            CreateMap<Product, BaseProductDTO>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (src.Category != null)
                    {
                        dest.Category = new()
                        {
                            Name = src.Category.Name,
                        };
                    }
                });

            CreateMap<UpdateProductDTO, Product>();
            CreateMap<CreateProductDTO, Product>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { Name = src.Category.Name }));
        }
    }
}
