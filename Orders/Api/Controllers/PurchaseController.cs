using Application.Requests.Purchases;
using Application.Requests.Purchases.CreateCheckoutSession;
using Application.Requests.Purchases.GetPurchaseById;
using Common.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PurchaseController : BaseApiController
{
    public PurchaseController(IMediator mediator) : base(mediator)
    {}

    [AllowAnonymous]
    [HttpGet("{PurchaseId}")]
    public Task<ActionResult<PurchaseResult>> GetPurchaseById([FromRoute] GetPurchaseByIdQuery query) =>
            HandleRequest(query);

    [AllowAnonymous]
    [HttpPost("session")]
    public Task<ActionResult<CreateCheckoutSessionCommandResult>> CreateCheckoutSession([FromBody] CreateCheckoutSessionCommand command) =>
            HandleRequest(command);

    // TODO: Other purchase for cash

    // TODO: Activate order
}
