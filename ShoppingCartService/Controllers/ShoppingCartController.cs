using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartManager _shoppingCartManager;
        private readonly ILogger<ShoppingCartController> _logger;


        public ShoppingCartController(ShoppingCartManager shoppingCartManager, ILogger<ShoppingCartController> logger)
        {
            _shoppingCartManager = shoppingCartManager;
            _logger = logger;
        }

        /// <summary>
        /// Get all shopping carts
        /// </summary>
        [HttpGet]
        public IEnumerable<ShoppingCartDto> GetAll()
        {
            return _shoppingCartManager.GetAllShoppingCarts();
        }

        /// <summary>
        /// Get cart by id
        /// </summary>
        /// <param name="id">Shopping cart id</param>
        [HttpGet("{id:length(24)}", Name = "GetCart")]
        public ActionResult<ShoppingCartDto> FindById(string id)
        {
            var cart = _shoppingCartManager.GetCart(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        /// <summary>
        /// Checkout shopping cart and calculate total cost
        /// </summary>
        /// <param name="id">Shopping cart id</param>
        [HttpGet("checkout/{id:length(24)}")]
        public ActionResult<CheckoutDto> CalculateTotals(string id)
        {
            try
            {
                return _shoppingCartManager.CalculateTotals(id);
            }
            catch (ShoppingCartNotFoundException)
            {
                _logger.LogError($"Shopping cart {id} not found");

                return NotFound();
            }
        }

        /// <summary>
        /// Create a new shopping cart
        /// </summary>
        [HttpPost]
        public ActionResult<ShoppingCartDto> Create([FromBody] CreateCartDto createCart)
        {
            try
            {
                var result = _shoppingCartManager.Create(createCart);

                return CreatedAtRoute("GetCart", new {id = result.Id}, result);
            }
            catch (InvalidInputException ex)
            {
                _logger.LogError($"Failed to create new shopping cart:\n{ex}");

                return BadRequest();
            }
        }

        /// <summary>
        /// Add product to existing cart
        /// </summary>
        /// <param name="id">Shopping cart id</param>
        /// <param name="item">Item details</param>
        [HttpPut("{id:length(24)}/item")]
        public ActionResult<ShoppingCartDto> AddItemToCart(string id, ItemDto item)
        {
            try
            {
                _shoppingCartManager.AddItemToCart(id, item);

                return Ok();
            }
            catch (ShoppingCartNotFoundException)
            {
                _logger.LogError($"Shopping cart {id} not found");

                return NotFound();
            }
        }

        /// <summary>
        /// Remove item from shopping cart
        /// </summary>
        /// <param name="id">Shopping cart id</param>
        /// <param name="productId">The item's product id</param>
        [HttpDelete("{id:length(24)}/{productId:length(24)}")]
        public IActionResult RemoveItemFromCart(string id, String productId)
        {
            try
            {
                _shoppingCartManager.RemoveFromShoppingCart(id, productId);

                return Ok();
            }
            catch (ShoppingCartNotFoundException)
            {
                _logger.LogError($"Shopping cart {id} not found");

                return NotFound();
            }
            catch (ItemNotFoundInCartException)
            {
                _logger.LogError($"Product {productId} not found in Shopping cart {id}");

                return NotFound();
            }
        }

        /// <summary>
        /// Delete shopping cart
        /// </summary>
        /// <param name="id">Shopping cart id</param>
        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteCart(string id)
        {
            _shoppingCartManager.DeleteCart(id);

            return NoContent();
        }
    }
}
