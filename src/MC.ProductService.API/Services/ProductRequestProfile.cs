using AutoMapper;
using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Data.Models;

namespace MC.ProductService.API.Services
{
    /// <summary>
    /// Configures mappings for the product data transfer object to the product entity model.
    /// This mapping configuration is used by AutoMapper to convert between <see cref="ProductRequest"/> DTOs
    /// and <see cref="Product"/> entities, ensuring that data is transferred correctly and efficiently
    /// between different layers of the application.
    /// </summary>
    public class ProductRequestProfile : Profile
    {
        public ProductRequestProfile()
        {
            CreateMap<ProductRequest, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));  // Ignoramos la Age;
        }
    }

    /// <summary>
    /// Defines an AutoMapper profile for mapping between different instances of the <see cref="Product"/> class.
    /// This configuration is typically used for cloning objects or applying updates to existing objects without
    /// affecting certain system-managed properties like timestamps and record statuses.
    /// </summary>
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, Product>()
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());
        }
    }
}
