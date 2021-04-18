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
    public class CategoryController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CategoryController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/category
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] PaginationQuery paginationQuery)
        {
            try
            {
                var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);

                if (paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
                    return BadRequest(new ErrorResponse
                    {
                        Code = "400",
                        Header = "Invalid query object",
                        Description = "Invalid query object"
                    });

                var categories = await _repository.Category.GetAllCategoriesAsync(paginationFilter);

                var categoriesResult = _mapper.Map<IEnumerable<CategoryReadDto>>(categories);

                return Ok
                    (
                        new PaginationResponse<IEnumerable<CategoryReadDto>>(categoriesResult)
                        {
                            PageNumber = paginationFilter.PageNumber,
                            PageSize = paginationFilter.PageSize,
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

         //GET api/category/count
        [HttpGet("count")]
        public async Task<IActionResult> GetCountCategories()
        {
            try
            {
                var count = await _repository.Category.GetCountCategoryAsync();

                return Ok(count);
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

        //GET api/category/{id}
        [HttpGet("{id}", Name = "GetCategoryById")]
        public async Task<IActionResult> GetCategoryById([FromQuery] Guid id)
        {
            try
            {
                var category = await _repository.Category.GetCategoryByIdAsync(id);

                if (category is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Code = "404",
                        Header = "Not Found",
                        Description = "Object not found"
                    });
                }
                else
                {
                    var categoryResult = _mapper.Map<CategoryReadDto>(category);

                    return Ok(new Response<CategoryReadDto>(categoryResult));
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

        //GET api/category/{id}/detail
        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetCategoryWithDetails(Guid id)
        {
            try
            {
                var category = await _repository.Category.GetCategoryWithDetailsAsync(id);
                if (category is null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Code = "404",
                        Header = "Not Found",
                        Description = "Object not found"
                    });
                }
                else
                {
                    var categoryResult = _mapper.Map<CategoryReadDto>(category);

                    return Ok(new Response<CategoryReadDto>(categoryResult));
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

        //POST api/category
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            try
            {
                if (categoryCreateDto is null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = "400",
                        Header = "Object is null",
                        Description = "Category object is null"
                    });
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = "400",
                        Header = "Invalid model object",
                        Description = "Invalid model object"
                    });
                }

                var categoryEntity = _mapper.Map<Category>(categoryCreateDto);

                _repository.Category.CreateCategory(categoryEntity);

                await _repository.SaveAsync();

                var createdCategory = _mapper.Map<CategoryReadDto>(categoryEntity);

                return CreatedAtRoute("GetCategoryById", new { id = createdCategory.Id }, createdCategory);
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

        //PUT api/category
        [HttpPut()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                if (categoryUpdateDto is null || categoryUpdateDto.Id == Guid.Empty)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = "400",
                        Header = "Object is null",
                        Description = "Category object is null"
                    });
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = "400",
                        Header = "Invalid model object",
                        Description = "Invalid model object"
                    });
                }

                var categoryEntity = await _repository.Category.GetCategoryByIdAsync(categoryUpdateDto.Id);

                if (categoryEntity is null)
                {
                    return NotFound();
                }
                _mapper.Map(categoryUpdateDto, categoryEntity);

                _repository.Category.UpdateCategory(categoryEntity);

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

        //DELETE api/category/{id}
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var category = await _repository.Category.GetCategoryWithDetailsAsync(id);

                if (category is null)
                {
                    return NotFound();
                }

                if (category.CategoryProduct.Any())
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = "400",
                        Header = "Cannot Delete",
                        Description = "Cannot delete category. It has related products. Delete those products first"
                    });
                }

                _repository.Category.DeleteCategory(category);

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