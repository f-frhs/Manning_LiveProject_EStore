using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.Controllers.Models;

namespace ShoppingCartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private CouponManager _couponManager;
        private ILogger<CouponController> _logger;

        public CouponController(CouponManager couponManager, ILogger<CouponController> logger)
        {
            _couponManager = couponManager;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<CouponDto> GetAll()
        {
            return _couponManager.GetAllCoupons();
        }

        [HttpGet("{id:length(24}", Name="GetCoupon")]
        public ActionResult<CouponDto> FindById(string id)
        {
            var coupon = _couponManager.GetCoupon(id);

            if (coupon == null)
            {
                return NotFound();
            }

            return coupon;
        }

        [HttpPost]
        public ActionResult<CouponDto> Create([FromBody] CouponDto coupon, DateTime? today = null)
        {
            try
            {
                var result = _couponManager.Create(coupon, today);
                return CreatedAtRoute("GetCoupon", new { id = result.Id }, result);
            }
            catch (InvalidInputException ex)
            {
                _logger.LogError($"Failed to create new coupon:\n{ex}");

                return BadRequest();
            }
        }

        [HttpDelete("{id:length(24)}")]
        public NoContentResult Delete(CouponDto couponDto)
        {
            _couponManager.Delete(couponDto.Id);

            return NoContent();
        }
    }
}