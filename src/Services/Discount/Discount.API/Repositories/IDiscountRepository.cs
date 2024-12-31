using System;
using Discount.API.entities;

namespace Discount.API.Repositories
{
	public interface IDiscountRepository
	{
		Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productId);
    }
}

