using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess;
using ShoppingCartService.Models;

namespace ShoppingCartService.BusinessLogic
{
    public class CouponManager
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly ICouponEngine _couponEngine;

        public CouponManager(ICouponRepository couponRepository, IMapper mapper, ICouponEngine couponEngine)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _couponEngine = couponEngine;
        }

        public IEnumerable<CouponDto> GetAllCoupons()
        {
            var all = _couponRepository.FindAll();

            return _mapper.Map<IEnumerable<CouponDto>>(all);
        }

        public CouponDto GetCoupon(string id)
        {
            var coupon = _couponRepository.FindById(id);

            return _mapper.Map<CouponDto>(coupon);
        }

        public CouponDto Create(CouponDto couponDto, DateTime? nullableToday = null)
        {
            var coupon = _mapper.Map<CouponBase>(couponDto);

            if (_couponRepository.FindById(coupon.Id) != null)
            {
                throw new InvalidInputException($"Fail to create coupon with duplicated id: {coupon.Id}");
            }

            var today = nullableToday ?? DateTime.Today;
            if (!coupon.IsUsableAt(today))
            {
                throw new InvalidInputException(
                    $"Fail to create expired coupon {nameof(coupon.ExpiredAt)} {coupon.ExpiredAt}");
            }

            _couponRepository.Create(coupon);

            return _mapper.Map<CouponDto>(coupon);
        }

        public void Delete(string couponId)
        {
            _couponRepository.Remove(couponId);
        }
    }
}