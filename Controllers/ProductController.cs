using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProductCatalog.Models;

namespace ProductCatalog.Controllers
{
    public class ProductController : Controller
    {
        static List<Product> productList = new List<Product>()
        {
            new Product() { Id = 1,
                            Name = "Mouse",
                            Category = "Electronics", 
                            Description = "Wireless mouse", 
                            Price = 500, 
                            Image = "https://via.placeholder.com/80", 
                            Quantity = 20 
                        },

            new Product() { Id = 2, 
                            Name = "Chair",
                            Category = "Furniture", 
                            Description = "Office chair",
                            Price = 3000, 
                            Image = "https://via.placeholder.com/80",
                            Quantity = 8 
                         },

            new Product() { Id = 3, 
                            Name = "Keyboard", 
                            Category = "Electronics", 
                            Description = "Mechanical keyboard", 
                            Price = 1500, Image = "https://via.placeholder.com/80", 
                            Quantity = 15 
                        }
        };

        // GET: Product
        public ActionResult Index()
        {
            return View(productList);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            Product p = productList.FirstOrDefault(x => x.Id == id);
            return View(p);
        }
    }
}