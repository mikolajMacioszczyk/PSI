using Common.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Common.Api.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ActionResult<T>> HandleRequest<T>(IRequest<Result<T>> request)
    {
        var result = await _mediator.Send(request);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else if (result.IsNotFound)
        {
            return NotFound(result.ErrorMessage);
        }
        return BadRequest(result.ErrorMessage);
    }
}
