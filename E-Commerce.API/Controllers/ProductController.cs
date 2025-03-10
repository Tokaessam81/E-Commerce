using AutoMapper;
using Ecommerce.Core.DTOS;
using E_Commerce.API.Error;
using E_Commerce.API.Helpers;
using Ecommerce.Core;
using Ecommerce.Core.Common.Constants;
using Ecommerce.Core.Entities;
using Ecommerce.Core.ServiceContract;
using Ecommerce.Core.Specifications.ProductSpecific;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitofWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [Cache(30)]
        public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetProducts([FromQuery] ProductSpecParams spec)
        {
            var specification = new ProductSpecification(spec);
            var products = await _unitOfWork.Repository<Product>().GetAllSpecification(specification);
            var mappedProducts = _mapper.Map<IReadOnlyList<ProductDTO>>(products);
            if (!mappedProducts.Any())
            {
                return NotFound(new ApiResponse(404, "No products found"));
            }
            return Ok(mappedProducts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Cache(30)]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var specification = new ProductSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdSpecification(specification);
            if (product == null)
            {
                return NotFound(new ApiResponse(404, "Product not found"));
            }
            var mappedProduct = _mapper.Map<ProductDTO>(product);
            return Ok(mappedProduct);
        }

        [HttpPost]
        [Authorize(Roles = AuthorizationConstants.AdminRole)]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid product data."));

            // التحقق من وجود المنتج بنفس الاسم
            var ProductExist = await _unitOfWork.Repository<Product>()
            .GetAllAsync();
            var isExist = ProductExist.Any(d => d.Name == request.Name);

            if (isExist)
            {
                return BadRequest(new ApiResponse(400, "The Product already exists!"));
            }

            // التحقق من صحة الـ CategoryId
            if (request.CategoryId.HasValue)
            {
                var categoryExists = await _unitOfWork.Repository<Category>().GetByIdAsync(request.CategoryId.Value);
                if (categoryExists == null)
                {
                    return BadRequest(new ApiResponse(400, "Invalid Category ID."));
                }
            }

            try
            {
                var product = _mapper.Map<Product>(request);

                await _unitOfWork.Repository<Product>().AddAsync(product);
                var saveResult = await _unitOfWork.completeAsync();

                if (saveResult <= 0)
                    return StatusCode(500, "An error occurred while saving the product.");

                var productToReturn = _mapper.Map<ProductDTO>(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, productToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, $"Error adding product: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AuthorizationConstants.AdminRole)]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, [FromBody] ProductDTO request)
        {
            var existingProduct = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new ApiResponse(404, "Product not found"));
            }

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid product data."));

            if (request.CategoryId.HasValue)
            {
                var categoryExists = await _unitOfWork.Repository<Category>().GetByIdAsync(request.CategoryId.Value);
                if (categoryExists == null)
                {
                    return BadRequest(new ApiResponse(400, "Invalid Category ID."));
                }
            }

            _mapper.Map(request, existingProduct);
            _unitOfWork.Repository<Product>().Update(existingProduct);

            var saveResult = await _unitOfWork.completeAsync();
            if (saveResult <= 0)
                return StatusCode(500, "An error occurred while updating the product.");

            var updatedProductDto = _mapper.Map<ProductDTO>(existingProduct);
            return Ok(updatedProductDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AuthorizationConstants.AdminRole)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new ApiResponse(404, "Product not found"));
            }
            _unitOfWork.Repository<Product>().Delete(product);
            var saveResult = await _unitOfWork.completeAsync();
            if (saveResult <= 0)
                return StatusCode(500, new ApiResponse(500, "An error occurred while deleting the product."));
            return NoContent();
        }
    }
}
