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
    public class FeatureController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public FeatureController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/feature
        [HttpGet]
        public async Task<IActionResult> GetAllFeatures([FromQuery] PaginationQuery paginationQuery)
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

                var features = await _repository.Feature.GetAllFeaturesAsync(paginationFilter);

                var featuresResult = _mapper.Map<IEnumerable<FeatureReadDto>>(features);

                return Ok
                (
                    new PaginationResponse<IEnumerable<FeatureReadDto>>(featuresResult)
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

        //GET api/feature/count
        [HttpGet("count")]
        public async Task<IActionResult> GetCountFeatures()
        {
            try
            {
                var count = await _repository.Feature.GetCountFeatureAsync();

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

        //GET api/feature/{id}
        [HttpGet("{id}", Name = "GetFeatureById")]
        public async Task<IActionResult> GetFeatureById(Guid id)
        {
            try
            {
                var feature = await _repository.Feature.GetFeatureByIdAsync(id);

                if (feature is null)
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
                    var featureResult = _mapper.Map<FeatureReadDto>(feature);

                    return Ok(new Response<FeatureReadDto>(featureResult));
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

        //GET api/feature/{id}/detail
        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetFeatureWithDetails(Guid id)
        {
            try
            {
                var feature = await _repository.Feature.GetFeatureWithDetailsAsync(id);
                if (feature is null)
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
                    var featureResult = _mapper.Map<FeatureReadDto>(feature);

                    return Ok(new Response<FeatureReadDto>(featureResult));
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
        public async Task<IActionResult> CreateFeature([FromBody] FeatureCreateDto featureCreateDto)
        {
            try
            {
                if (featureCreateDto is null)
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

                var featureEntity = _mapper.Map<Feature>(featureCreateDto);

                _repository.Feature.CreateFeature(featureEntity);

                await _repository.SaveAsync();

                var createdFeature = _mapper.Map<FeatureReadDto>(featureEntity);

                return CreatedAtRoute("GetFeatureById", new { id = createdFeature.Id }, createdFeature);
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

        //PUT api/feature
        [HttpPut()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateFeature(FeatureUpdateDto featureUpdateDto)
        {
            try
            {
                if (featureUpdateDto is null || featureUpdateDto.Id == Guid.Empty)
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

                var featureEntity = await _repository.Feature.GetFeatureByIdAsync(featureUpdateDto.Id);

                if (featureEntity is null)
                {
                    return NotFound();
                }

                _mapper.Map(featureUpdateDto, featureEntity);

                _repository.Feature.UpdateFeature(featureEntity);

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

        //DELETE api/feature/{id}
        [HttpDelete("id")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteFeature(Guid id)
        {
            try
            {
                var feature = await _repository.Feature.GetFeatureWithDetailsAsync(id);

                if (feature is null)
                {
                    return NotFound();
                }
                if (feature.ProductFeature.Any())
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = "400",
                        Header = "Cannot Delete",
                        Description = "Cannot delete feature. It has related products. Delete those products first"
                    });
                }

                _repository.Feature.DeleteFeature(feature);

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