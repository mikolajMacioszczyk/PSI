﻿using Common.Application.Models;
using MediatR;

namespace Application.Requests.Baskets.SubstractProductFromBasket;
public record SubstractProductFromBasketCommand(Guid BasketId, Guid ProductInCatalogId) : IRequest<Result<BasketResult>>;
