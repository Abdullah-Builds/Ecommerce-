using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
namespace MyApp.Controllers
{
    public class CouponController : Controller
    {
        private readonly IConfiguration _configuration;

        public CouponController (IConfiguration configuration)
        {
                       _configuration = configuration;
        }


        public async Task<CouponModel> ValidateCoupon(string coupon)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = "SELECT ExpiryDate,Discount FROM Coupons WHERE Coupon = @Coupon";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Coupon", coupon);

                    await connection.OpenAsync();

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var table = new DataTable();
                        adapter.Fill(table); 

                        if (table.Rows.Count == 0)
                        {
                            return new CouponModel { IsValid = false, Discount = 0}; 
                        }

                        var expiryDate = Convert.ToDateTime(table.Rows[0]["ExpiryDate"]);
                        var discount = Convert.ToInt32(table.Rows[0]["Discount"]);

                        bool flag =  expiryDate > DateTime.UtcNow;

                        return new CouponModel { IsValid=true, Discount = discount };
                    }
                }
            }
        }
        [Route("api/validate-coupon")]
        [HttpPost]
        public async Task<IActionResult> ValidateCouponEndpoint([FromBody] string request)
        {
            CouponModel response = new CouponModel ();
            response = await ValidateCoupon(request);

            return Json(response);
        }

        

    }
}
