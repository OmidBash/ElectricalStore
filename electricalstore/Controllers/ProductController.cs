using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.Repositories;
using Contracts.Requests;
using Contracts.Responses;
using Entities.Dtos;
using Entities.Filters;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace electricalstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public ProductController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] PaginationQuery paginationQuery)
        {
            try
            {
                var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);

                if (paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
                    return BadRequest("Invalid query object");

                var products = await _repository.Product.GetAllProductsAsync(paginationFilter);

                var productsResult = _mapper.Map<IEnumerable<ProductReadDto>>(products);

                return Ok
                (
                    new PaginationResponse<IEnumerable<ProductReadDto>>(productsResult)
                    {
                        PageNumber = paginationFilter.PageNumber,
                        PageSize = paginationFilter.PageSize
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Code = "500",
                    Header = "Internal server error",
                    Description = ex.Message
                });
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCountProducts()
        {
            try
            {
                var count = await _repository.Product.GetCountProductAsync();

                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error ({ex.Message})");
            }
        }

        //GET api/product/{id}
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var product = await _repository.Product.GetProductByIdAsync(id);

                if (product is null)
                {
                    return NotFound();
                }
                else
                {
                    var productResult = _mapper.Map<ProductReadDto>(product);

                    return Ok(new Response<ProductReadDto>(productResult));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Code = "500",
                    Header = "Internal server error",
                    Description = ex.Message
                });
            }
        }

        //GET api/product/{id}/detail
        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetProductWithDetails(Guid id)
        {
            try
            {
                var product = await _repository.Product.GetProductWithDetailsAsync(id);
                if (product is null)
                {
                    return NotFound();
                }
                else
                {
                    var productResult = _mapper.Map<ProductReadDto>(product);
                    return Ok(new Response<ProductReadDto>(productResult));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Code = "500",
                    Header = "Internal server error",
                    Description = ex.Message
                });
            }
        }

        //POST api/product
        [HttpPost]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            try
            {
                if (productCreateDto is null)
                {
                    return BadRequest("Product object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var productEntity = _mapper.Map<Product>(productCreateDto);

                _repository.Product.CreateProduct(productEntity);

                await _repository.SaveAsync();

                var createdProduct = _mapper.Map<ProductReadDto>(productEntity);

                return CreatedAtRoute("GetProductById", new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Code = "500",
                    Header = "Internal server error",
                    Description = ex.Message
                });
            }
        }

        //PUT api/product/{id}
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateProduct(ProductUpdateDto productUpdateDto)
        {
            try
            {
                if (productUpdateDto is null || productUpdateDto.Id == Guid.Empty)
                {
                    return BadRequest("Product object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var productEntity = await _repository.Product.GetProductWithDetailsAsync(productUpdateDto.Id);

                if (productEntity is null)
                {
                    return NotFound();
                }

                if (productEntity.CategoryProduct.Any())
                {
                    var deletedCategory = productEntity.CategoryProduct
                        .Where(categoryEntity => productUpdateDto.Categories
                        .All(categoryUpdate => categoryUpdate.CategoryId != categoryEntity.CategoryId))
                        .Select(cp => new CategoryProductJunction 
                            { 
                                CategoryId = cp.CategoryId, 
                                ProductId = cp.ProductId 
                            }
                        )
                        .ToList();

                    if (deletedCategory.Any())
                    {
                        _repository.CategoryProductJunc.DeleteCategoryProductJuncRange(deletedCategory);

                        productEntity.CategoryProduct = productEntity.CategoryProduct.Except(deletedCategory).ToList();
                    }
                }

                if (productEntity.ProductFeature.Any())
                {
                    var deletedFeatures = productEntity.ProductFeature
                        .Where(featureEntity => productUpdateDto.Features
                        .All(featureUpdate => featureUpdate.FeatureId != featureEntity.FeatureId))
                        .Select(pf => new ProductFeatureJunction
                            { 
                                FeatureId = pf.FeatureId, 
                                ProductId = pf.ProductId 
                            }
                        )
                        .ToList();

                    if (deletedFeatures.Any())
                    {
                        _repository.ProductFeatureJunc.DeleteProductFeatureJuncRange(deletedFeatures);

                        productEntity.ProductFeature = productEntity.ProductFeature.Except(deletedFeatures).ToList();
                    }
                }

                if (productEntity.ProductImage.Any())
                {
                    var deletedImages = productEntity.ProductImage
                        .Where(imageEntity => productUpdateDto.ImagePaths
                        .All(imageUpdate => imageUpdate.Id != imageEntity.Id))
                        .Select(img => new ProductImage { Id = img.Id })
                        .ToList();

                    if (deletedImages.Any())
                    {
                        _repository.ProductImage.DeleteImageProductRange(deletedImages);

                        productEntity.ProductImage = productEntity.ProductImage.Except(deletedImages).ToList();
                    }
                }

                _mapper.Map(productUpdateDto, productEntity);

                _repository.Product.UpdateProduct(productEntity);

                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Code = "500",
                    Header = "Internal server error",
                    Description = ex.Message
                });
            }
        }

        //DELETE api/product/{id}
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var product = await _repository.Product.GetProductWithDetailsAsync(id);

                if (product is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Code = "404",
                        Header = "Not Found",
                        Description = "Object not found"
                    });
                }

                _repository.Product.DeleteProduct(product);

                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Code = "500",
                    Header = "Internal server error",
                    Description = ex.Message
                });
            }
        }
    }
}