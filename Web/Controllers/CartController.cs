using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NuGet.Frameworks;
using Web.Models;

namespace Web.Controllers
{
    public class CartController : Controller, IActionFilter
    {
        private readonly DevXuongMocContext _context;
        private List<Cart> _carts = new List<Cart>();
        public CartController(DevXuongMocContext context)
        {
            _context = context;
        }

        // PT OnActionExecuting 
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cartInSession = HttpContext.Session.GetString("My-Cart");
            if (cartInSession != null)
            {
                // chuyển dữ liệu json 
                _carts = JsonConvert.DeserializeObject<List<Cart>>(cartInSession);
            }
            base.OnActionExecuting(context);
        }

        // 
        public IActionResult Index()
        {
            return View(_carts);
        }
        //
        public IActionResult Add(int id)
        {
            if (_carts.Any(c => c.Id == id))
            {
                _carts.Where(c => c.Id == id).First().Quantity += 1;
            }
            else
            {
                var p = _context.Products.Find(id);
                var item = new Cart()
                {
                    Id = p.Id,
                    Name = p.Title,
                    Price = (float)p.PriceNew.Value,
                    Quantity = 1,
                    Image = p.Image,
                    Total = (float)p.PriceNew.Value * 1
                };
                _carts.Add(item);
            }
            HttpContext.Session.SetString("My_Cart", JsonConvert.SerializeObject(_carts));
            return RedirectToAction("Index");
        }


        public IActionResult Remove(int id)
        {
            if (_carts.Any(c => c.Id == id))
            {
                // tìm sản phẩm trong giỏ hàng
                var item = _carts.Where(c => c.Id == id).First();
                //thực hiện xóa
                _carts.Remove(item);
                // lưu carts vào session, cần phải chuyển sang dữ liệu json
                HttpContext.Session.SetString("My-Cart",JsonConvert.SerializeObject(_carts));
            }
            return RedirectToAction("Index");
        }



        public IActionResult Update(int id, int quantity)
        {
            if (_carts.Any(c => c.Id == id))
            {
                // tìm sản phẩm trong giỏ hàng
                _carts.Where(c => c.Id == id).First().Quantity = quantity;
             
                // lưu carts vào session, cần phải chuyển sang dữ liệu json
                HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(_carts));
            }
            return RedirectToAction("Index");
        }




        public IActionResult Clear()
        {
                // lưu carts vào session, cần phải chuyển sang dữ liệu json
            HttpContext.Session.Remove("My-Cart");
            
            return RedirectToAction("Index");
        }










    }

}