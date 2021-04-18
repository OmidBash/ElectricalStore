using AutoMapper;
using Entities.Dtos;
using Entities.Filters;
using Entities.Models;
using Contracts.Requests;

namespace Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Category Maping
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            #endregion

            #region Feature Mapping
            CreateMap<Feature, FeatureReadDto>();
            CreateMap<FeatureCreateDto, Feature>();
            CreateMap<FeatureUpdateDto, Feature>();
            #endregion

            #region FeatureType Mapping
            CreateMap<FeatureType, FeatureTypeReadDto>();
            CreateMap<FeatureTypeCreateDto, FeatureType>();
            CreateMap<FeatureTypeUpdateDto, FeatureType>();
            #endregion

            #region Product Mapping
            CreateMap<Product, ProductReadDto>()
            .ForMember(
                dest => dest.Categories,
                opt => opt.MapFrom(src => src.CategoryProduct)
            )
            .ForMember(
                dest => dest.Features,
                opt => opt.MapFrom(src => src.ProductFeature)
            )
            .ForMember(
                dest => dest.ImagePaths,
                opt => opt.MapFrom(src => src.ProductImage)
            );

            CreateMap<ProductCreateDto, Product>()
            .ForMember(
                dest => dest.CategoryProduct,
                opt => opt.MapFrom(src => src.Categories)
            )
            .ForMember(
                dest => dest.ProductFeature,
                opt => opt.MapFrom(src => src.Features)
            )
            .ForMember(
                dest => dest.ProductImage,
                opt => opt.MapFrom(src => src.ImagePaths)
            );

            CreateMap<ProductUpdateDto, Product>()
            .ForMember(
                dest => dest.CategoryProduct,
                opt => opt.MapFrom(src => src.Categories)
            )
            .ForMember(
                dest => dest.ProductFeature,
                opt => opt.MapFrom(src => src.Features)
            )
            .ForMember(
                dest => dest.ProductImage,
                opt => opt.MapFrom(src => src.ImagePaths)
            );

            CreateMap<CategoryProductJunction, CategoryProductJunctionReadDto>();
            CreateMap<CategoryProductJunctionCreateDto, CategoryProductJunction>();
            CreateMap<CategoryProductJunctionUpdateDto, CategoryProductJunction>();

            CreateMap<ProductFeatureJunction, ProductFeatureJunctionReadDto>();
            CreateMap<ProductFeatureJunctionCreateDto, ProductFeatureJunction>();
            CreateMap<ProductFeatureJunctionUpdateDto, ProductFeatureJunction>();

            CreateMap<ProductImage, ProductImageReadDto>();
            CreateMap<ProductImageCreateDto, ProductImage>();
            CreateMap<ProductImageUpdateDto, ProductImage>();
            #endregion

            #region Pagination Mapping
            CreateMap<PaginationQuery, PaginationFilter>();
            #endregion
        }
    }
}
