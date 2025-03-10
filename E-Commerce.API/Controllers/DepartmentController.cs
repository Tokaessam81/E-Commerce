using AutoMapper;
using Ecommerce.Core.DTOS;
using E_Commerce.API.Error;
using E_Commerce.API.Helpers;
using Ecommerce.Core;
using Ecommerce.Core.Common.Constants;
using Ecommerce.Core.Entities;
using Ecommerce.Core.ServiceContract;
using Ecommerce.Core.Specifications.DepartmentSpecific;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public DepartmentController(IUnitofWork unitofWork,IMapper mapper,IImageService imageService)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
           _imageService = imageService;
            
        }
        [HttpGet]
        [ProducesResponseType(typeof(DepartmentDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [Cache(30)]
        public async Task<ActionResult<IReadOnlyList<DepartmentDTO>>> GetDepartments([FromQuery] DepartmentSpecParams spec)
        {
            var specification = new DepartmentSpecification(spec);
            var departments = await _unitofWork.Repository<Department>().GetAllSpecification(specification);
            var mappedDepartments = _mapper.Map<IReadOnlyList<DepartmentDTO>>(departments);
            if(mappedDepartments.Count == 0)
            {
                return NotFound(new ApiResponse(404, "No departments found"));
            }
            return Ok(mappedDepartments);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Cache(30)]
        public async Task<ActionResult<DepartmentDTO>> GetDepartmentById(int id)
        {
            var department = await _unitofWork.Repository<Department>().GetByIdAsync(id);
            if (department == null)
            {
                return NotFound(new ApiResponse(404, "Department not found"));
            }
            var mappedDepartment = _mapper.Map<DepartmentDTO>(department);
            return Ok(mappedDepartment);
        }

        [HttpPost]
        [Authorize(Roles = AuthorizationConstants.AdminRole)]
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(request.PictureUrl))
                return BadRequest("Picture URL is required.");
           
                var departmentExist = await _unitofWork.Repository<Department>()
                .GetAllAsync();
                var isExist = departmentExist.Any(d => d.EnglishName == request.EnglishName);
            
            if (isExist)
            {
                return BadRequest(new ApiResponse(400, "The Department is already exist!"));
            }

            try
            {
                var metadata = await _imageService.ExtractMetadataFromUrlAsync(request.PictureUrl);

                var department = _mapper.Map<Department>(request);
                department.PictureUrl = request.PictureUrl;


                await _unitofWork.Repository<Department>().AddAsync(department);

                var saveResult = await _unitofWork.completeAsync();
                if (saveResult <= 0)
                    return StatusCode(500, "An error occurred while saving the department.");

                var departmentToReturn = _mapper.Map<DepartmentDTO>(department);
                return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, departmentToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing the image: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = AuthorizationConstants.AdminRole)]
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartmentDTO>> UpdateDepartment(int id, [FromBody] DepartmentDTO request)
        {
            var existingDepartment = await _unitofWork.Repository<Department>().GetByIdAsync(id);
            if (existingDepartment == null)
            {
                return NotFound(new ApiResponse(404, "Department not found"));
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!string.IsNullOrEmpty(request.PictureUrl))
            {
                try
                {
                    var metadata = await _imageService.ExtractMetadataFromUrlAsync(request.PictureUrl);
                    existingDepartment.PictureUrl = request.PictureUrl;

                }
                catch (Exception ex)
                {
                    return BadRequest(new ApiResponse(400, $"Failed to process the image: {ex.Message}"));
                }
            }

            _mapper.Map(request, existingDepartment);

            _unitofWork.Repository<Department>().Update(existingDepartment);

            var saveResult = await _unitofWork.completeAsync();
            if (saveResult <= 0)
                return StatusCode(500, "An error occurred while updating the department.");

            var updatedDepartmentDto = _mapper.Map<DepartmentDTO>(existingDepartment);
            return Ok(updatedDepartmentDto);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = AuthorizationConstants.AdminRole)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _unitofWork.Repository<Department>().GetByIdAsync(id);
            if (department == null)
            {
                return NotFound(new ApiResponse(404, "Department not found"));
            }
            _unitofWork.Repository<Department>().Delete(department);
            var saveResult = await _unitofWork.completeAsync();
            if (saveResult <= 0)
                return StatusCode(500, new ApiResponse(500, "An error occurred while deleting the department."));
            return NoContent();
        }



    }
}
