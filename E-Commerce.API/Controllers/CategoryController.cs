using AutoMapper;
using Ecommerce.Core.DTOS;
using E_Commerce.API.Error;
using E_Commerce.API.Helpers;
using Ecommerce.Core;
using Ecommerce.Core.Common.Constants;
using Ecommerce.Core.Entities;
using Ecommerce.Core.ServiceContract;
using Ecommerce.Core.Specifications.CategorySpecific;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitofWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [Cache(30)]
        public async Task<ActionResult<IReadOnlyList<CategoryDTO>>> GetCategories([FromQuery] CategorySpecParams spec)
        {
            var specification = new CategorySpecification(spec);
            var categories = await _unitOfWork.Repository<Category>().GetAllSpecification(specification);
            var mappedCategories = _mapper.Map<IReadOnlyList<CategoryDTO>>(categories);
            if (!mappedCategories.Any())
            {
                return NotFound(new ApiResponse(404, "No categories found"));
            }
            return Ok(mappedCategories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Cache(30)]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new ApiResponse(404, "Category not found"));
            }
            var mappedCategory = _mapper.Map<CategoryDTO>(category);
            return Ok(mappedCategory);
        }

        [HttpPost]
        [Authorize(Roles = AuthorizationConstants.AdminRole)]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid category data."));

            var CategoryExist = await _unitOfWork.Repository<Category>()
             .GetAllAsync();
            var isExist = CategoryExist.Any(d => d.EnglishName == request.EnglishName);

            if (isExist)
            {
                return BadRequest(new ApiResponse(400, "The Category is already exist!"));
            }
            if (request.DepartmentId.HasValue)
            {
                var departmentExists = await _unitOfWork.Repository <Category>().GetByIdAsync(request.DepartmentId.Value);
                if (departmentExists == null)
                {
                    return BadRequest(new ApiResponse(400, "Invalid Department ID."));
                }
            }

            try
            {
                var category = _mapper.Map<Category>(request);
                await _unitOfWork.Repository<Category>().AddAsync(category);

                var saveResult = await _unitOfWork.completeAsync();
                if (saveResult <= 0)
                    return StatusCode(500, "An error occurred while saving the category.");

                var categoryToReturn = _mapper.Map<CategoryDTO>(category);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, categoryToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, $"Error adding category: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AuthorizationConstants.AdminRole)]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id, [FromBody] CategoryDTO request)
        {
            var existingCategory = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new ApiResponse(404, "Category not found"));
            }

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid category data."));

            if (request.DepartmentId.HasValue)
            {
                var departmentExists = await _unitOfWork.Repository<Department>().GetByIdAsync(request.DepartmentId.Value);
                if (departmentExists == null)
                {
                    return BadRequest(new ApiResponse(400, "Invalid Department ID."));
                }
            }

            _mapper.Map(request, existingCategory);
            _unitOfWork.Repository<Category>().Update(existingCategory);

            var saveResult = await _unitOfWork.completeAsync();
            if (saveResult <= 0)
                return StatusCode(500, "An error occurred while updating the category.");

            var updatedCategoryDto = _mapper.Map<CategoryDTO>(existingCategory);
            return Ok(updatedCategoryDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AuthorizationConstants.AdminRole)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new ApiResponse(404, "Category not found"));
            }
            _unitOfWork.Repository<Category>().Delete(category);
            var saveResult = await _unitOfWork.completeAsync();
            if (saveResult <= 0)
                return StatusCode(500, new ApiResponse(500, "An error occurred while deleting the category."));
            return NoContent();
        }
    }
}
