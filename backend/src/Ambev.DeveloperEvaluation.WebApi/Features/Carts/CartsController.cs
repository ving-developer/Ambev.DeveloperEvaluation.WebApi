using Ambev.DeveloperEvaluation.Application.Commands.Carts.AddItemToCart;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.CancelCart;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.CompleteCart;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.RemoveItemFromCart;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.UpdateItemQuantity;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.SearchCarts;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddItemToCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CancelCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.SearchCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateItemQuantity;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

/// <summary>
/// Controller for managing cart operations
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CartsController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a paginated list of carts
    /// </summary>
    /// <param name="request">Request params containing the search query params</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of carts</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListCarts(
        [FromQuery] SearchCartsRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<SearchCartsQuery>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return OkPaginated(response);
    }

    /// <summary>
    /// Creates a new cart
    /// </summary>
    /// <param name="request">The cart creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCart(
        [FromBody] CreateCartRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateCartCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var cartResponse = _mapper.Map<CartResponse>(response);

        return Created("GetCartById", new { id = cartResponse.Id }, cartResponse);
    }

    /// <summary>
    /// Retrieves a cart by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the cart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart details if found</returns>
    [HttpGet("{id:guid}", Name = "GetCartById")]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new GetCartByIdQuery(id);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<CartResponse>(response), "Cart retrieved successfully");
    }

    /// <summary>
    /// Cancels a cart by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the cart to cancel</param>
    /// <param name="request">The cancellation request with reason</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the cart was canceled</returns>
    [HttpPatch("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelCart(
        [FromRoute] Guid id,
        [FromBody] CancelCartRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CancelCartCommand(id, request.Reason);
        await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Cart canceled successfully"
        });
    }


    /// <summary>
    /// Adds an item to an existing cart
    /// </summary>
    /// <param name="id">The cart ID</param>
    /// <param name="request">The item to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated cart details</returns>
    [HttpPost("{id}/items")]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItemToCart(
        [FromRoute] Guid id,
        [FromBody] AddItemToCartRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<AddItemToCartCommand>(request) with { CartId = id };
        var result = await _mediator.Send(command, cancellationToken);
        var cartResponse = _mapper.Map<CartResponse>(result);

        return Ok(cartResponse);
    }

    /// <summary>
    /// Removes an item from a cart
    /// </summary>
    /// <param name="cartId">The cart ID</param>
    /// <param name="itemId">The item ID to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated cart details</returns>
    [HttpDelete("{cartId}/items/{itemId}")]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItemFromCart(
        [FromRoute] Guid cartId,
        [FromRoute] Guid itemId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveItemFromCartCommand(cartId, itemId);
        var result = await _mediator.Send(command, cancellationToken);
        var cartResponse = _mapper.Map<CartResponse>(result);

        return Ok(cartResponse);
    }

    /// <summary>
    /// Completes a cart (finalizes the sale)
    /// </summary>
    /// <param name="id">The cart ID to complete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The completed cart details</returns>
    [HttpPost("{id}/complete")]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteCart(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CompleteCartCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        var cartResponse = _mapper.Map<CartResponse>(result);

        return Ok(cartResponse);
    }

    /// <summary>
    /// Updates item quantity in a cart
    /// </summary>
    /// <param name="cartId">The cart ID</param>
    /// <param name="itemId">The item ID</param>
    /// <param name="request">The quantity update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated cart details</returns>
    [HttpPut("{cartId}/items/{itemId}/quantity")]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItemQuantity(
        [FromRoute] Guid cartId,
        [FromRoute] Guid itemId,
        [FromBody] UpdateItemQuantityRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateItemQuantityCommand(cartId, itemId, request.Quantity);
        var result = await _mediator.Send(command, cancellationToken);
        var cartResponse = _mapper.Map<CartResponse>(result);

        return Ok(cartResponse);
    }
}