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
    public class FeatureTypeController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public FeatureTypeController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/featureType
        [HttpGet]
        public async Task<IActionResult> GetAllFeatureTypes([FromQuery] PaginationQuery paginationQuery)
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

                var featureTypes = await _repository.FeatureType.GetAllFeatureTypesAsync(paginationFilter);

                var featureTypesResult = _mapper.Map<IEnumerable<FeatureTypeReadDto>>(featureTypes);

                return Ok
                (
                    new PaginationResponse<IEnumerable<FeatureTypeReadDto>>(featureTypesResult)
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

        //GET api/featuretypes/count
        [HttpGet("count")]
        public async Task<IActionResult> GetCountFeatureTypes()
        {
            try
            {
                var count = await _repository.FeatureType.GetCountFeatureTypeAsync();

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

        //GET api/featureType/{id}
        [HttpGet("{id}", Name = "GetFeatureTypeById")]
        public async Task<IActionResult> GetFeatureTypeById(Guid id)
        {
            try
            {
                var featureType = await _repository.FeatureType.GetFeatureTypeByIdAsync(id);

                if (featureType is null)
                {
                    return NotFound();
                }
                else
                {
                    var featureTypeResult = _mapper.Map<FeatureTypeReadDto>(featureType);

                    return Ok(new Response<FeatureTypeReadDto>(featureTypeResult));
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

        //GET api/featureTpe/{id}/detail
        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetFeatureTypeWithDetails(Guid id)
        {
            try
            {
                var featureType = await _repository.FeatureType.GetFeatureTypeWithDetailsAsync(id);
                if (featureType is null)
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
                    var featureTypeResult = _mapper.Map<FeatureTypeReadDto>(featureType);

                    return Ok(new Response<FeatureTypeReadDto>(featureTypeResult));
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

        //POST api/feature
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateFeatureType([FromBody] FeatureTypeCreateDto featureTypeCreateDto)
        {
            try
            {
                if (featureTypeCreateDto is null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = "400",
                        Header = "Object is null",
                        Description = "Feature object is null"
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

                var featureTypeEntity = _mapper.Map<FeatureType>(featureTypeCreateDto);

                _repository.FeatureType.CreateFeatureType(featureTypeEntity);

                await _repository.SaveAsync();

                var createdFeatureType = _mapper.Map<FeatureTypeReadDto>(featureTypeEntity);

                return CreatedAtRoute("GetFeatureTypeById", new { id = createdFeatureType.Id }, createdFeatureType);
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

        //PUT api/featureType/{id}
        [HttpPut()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateFeatureType(FeatureTypeUpdateDto featureTypeUpdateDto)
        {
            try
            {
                if (featureTypeUpdateDto is null || featureTypeUpdateDto.Id == Guid.Empty)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = "400",
                        Header = "Object is null",
                        Description = "Feature object is null"
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

                var featureTypeEntity = await _repository.FeatureType.GetFeatureTypeWithDetailsAsync(featureTypeUpdateDto.Id);

                if (featureTypeEntity is null)
                {
                    return NotFound();
                }

                if (featureTypeEntity.Features.Any())
                {
                    var deletedFeatures = featureTypeEntity.Features
                        .Where(featureEntity => featureTypeUpdateDto.Features
                        .All(featureUpdate => featureUpdate.Id != featureEntity.Id))
                        .ToList();

                    if (deletedFeatures.Any())
                    {
                        foreach (Feature deletedFeature in deletedFeatures)
                        {
                            var feature = await _repository.Feature.GetFeatureWithDetailsAsync(deletedFeature.Id);

                            if (feature is null || feature.ProductFeature.Any())
                            {
                                return BadRequest(new ErrorResponse
                                {
                                    Code = "400",
                                    Header = "Cannot Delete",
                                    Description = "Cannot delete this feature type. Some features have related product. Delete those products first"
                                });
                            }

                            _repository.Feature.DeleteFeature(deletedFeature);
                        }
                    }
                }

                _mapper.Map(featureTypeUpdateDto, featureTypeEntity);

                _repository.FeatureType.UpdateFeatureType(featureTypeEntity);

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

        //DELETE api/featureType{id}
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteFeatureType(Guid id)
        {
            try
            {
                var featureType = await _repository.FeatureType.GetFeatureTypeWithDetailsAsync(id);

                if (featureType is null)
                {
                    return NotFound();
                }
                if (featureType.Features is not null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = "400",
                        Header = "Cannot Delete",
                        Description = "Cannot delete feature type. It has related features. Delete those features first"
                    });
                }

                _repository.FeatureType.DeleteFeatureType(featureType);

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