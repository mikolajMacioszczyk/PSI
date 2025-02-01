using Application.Requests.Purchases.CreateCheckoutSession;
using Common.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PurchaseController : BaseApiController
{
    public PurchaseController(IMediator mediator) : base(mediator)
    {}

    // TODO: Get By ID

    [AllowAnonymous]
    [HttpPost("session")]
    // TODO: Create puchase
    public Task<ActionResult<CreateCheckoutSessionCommandResult>> CreateCheckoutSession([FromBody] CreateCheckoutSessionCommand command) =>
            HandleRequest(command);

    // TODO: Other purchase for cash
}
