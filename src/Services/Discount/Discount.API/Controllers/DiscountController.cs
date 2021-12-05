using Discount.API.Entities;
using Discount.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    public class DiscountController : BaseController
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName) {
            var coupon = await _discountRepository.GetDiscount(productName);
            if(coupon == null)
                return NotFound(coupon);
                
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateDiscount([FromBody]Coupon coupon) {
            var isCraeted = await _discountRepository.CreateDiscount(coupon);
            if (isCraeted)
                return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName}, coupon);

            return BadRequest("Oops! something was wrong");
        } 

        [HttpPut]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            var isUpdated = await _discountRepository.UpdateDiscount(coupon);
            if (isUpdated)
                return Ok("Discount coupon updated successfully");

            return BadRequest("Oops! something was wrong");
        }

        [HttpDelete("{productName}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteDiscount(string productName)
        {
            var isDeleted = await _discountRepository.DeleteDiscount(productName);
            if (isDeleted)
                return Ok("Discount coupon deleted successfully");

            return BadRequest("Oops! something was wrong");
        }
    }
}
