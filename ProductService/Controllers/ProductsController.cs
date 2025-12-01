using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Extensions;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service) => _service = service;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var userId = HttpContext.GetUserId();
            var product = await _service.GetByIdAsync(id, userId, ct);
            return product is null ? NotFound() : Ok(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO dto, CancellationToken ct)
        {
            var userId = HttpContext.GetUserId();
            var result = await _service.CreateAsync(userId, dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDTO dto, CancellationToken ct)
        {
            var userId = HttpContext.GetUserId();
            var success = await _service.UpdateAsync(id, userId, dto, ct);
            return success ? NoContent() : Forbid();
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var userId = HttpContext.GetUserId();
            var success = await _service.DeleteAsync(id, userId, ct);
            return success ? NoContent() : Forbid();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? search, [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice, [FromQuery] bool? isAvailable, [FromQuery] DateTime? createdFrom,
            [FromQuery] DateTime? createdTo, [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "CreatedAt", [FromQuery] bool desc = true, CancellationToken ct = default)
        {
            var userId = HttpContext.GetUserId();
            var q = new ProductQuery(search, minPrice, maxPrice, isAvailable, createdFrom, createdTo,
                                     page, pageSize, sortBy, desc);
            var result = await _service.SearchAsync(q, userId, ct);
            return Ok(result);
        }
    }

}
