﻿using Common.Application.Models;
using MediatR;

namespace Application.Requests.Baskets.AddProductToBasket;

public record AddProductToBasketCommand(Guid BasketId, Guid ProductInCatalogId) : IRequest<Result<BasketResult>>;
