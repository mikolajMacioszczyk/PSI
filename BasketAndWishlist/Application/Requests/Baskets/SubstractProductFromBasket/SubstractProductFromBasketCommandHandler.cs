﻿using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Requests.Baskets.SubstractProductFromBasket;
// TODO: Tests
public class SubstractProductFromBasketCommandHandler : IRequestHandler<SubstractProductFromBasketCommand, Result<BasketResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubstractProductFromBasketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BasketResult>> Handle(SubstractProductFromBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await _unitOfWork.BasketRepository.GetByIdWithProducts(request.BasketId);

        if (basket is null)
        {
            return new NotFound(request.BasketId, $"Basket with provided id {request.BasketId} does not exists");
        }

        var existingProductEntry = basket.ProductsInBaskets.FirstOrDefault(p => p.ProductInCatalogId == request.ProductInCatalogId);
        if (existingProductEntry is null)
        {
            return new Failure($"Product with provided id {request.ProductInCatalogId} is not part of the busket");
        }

        existingProductEntry.PieceCount -= 1;

        if (existingProductEntry.PieceCount > 0)
        {
            _unitOfWork.ProductInBasketRepository.Update(existingProductEntry);
        }
        else
        {
            _unitOfWork.ProductInBasketRepository.Remove(existingProductEntry);
        }

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BasketResult>(basket);
    }
}