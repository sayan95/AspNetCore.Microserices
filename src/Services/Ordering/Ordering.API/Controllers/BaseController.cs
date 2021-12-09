using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Ordering.BusinessLogic.Core;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound(result);
            if (result.IsSuccess && result.Value == null) return NotFound();
            if(result.IsSuccess && result.Value != null ) return Ok(result.Value);

            return BadRequest(result.Error);
        }
    }
}
