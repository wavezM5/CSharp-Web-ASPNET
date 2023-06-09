﻿using System.Text;
using System.Text.Json;
using Microsoft.Net.Http.Headers;
using MVCIntroDemo.Models.Product;

namespace MVCIntroDemo.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        private IEnumerable<ProductViewModel> _products
            = new List<ProductViewModel>()
            {
                new ProductViewModel()
                {
                    Id = 1,
                    Name = "Cheese",
                    Price = 7.00
                },
                new ProductViewModel()
                {
                    Id = 2,
                    Name = "Ham",
                    Price = 5.00
                },
                new ProductViewModel()
                {
                    Id = 3,
                    Name = "Bread",
                    Price = 1.50
                }
            };

 
        public IActionResult All(string keyword)
        {
            if (keyword != null)
            {
                var foundProducts = _products
                    .Where(p => p.Name.ToLower().Contains(keyword.ToLower()));
                return View(foundProducts);
            }

            return View(_products);
        }

        public IActionResult ById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                //return BadRequest();
                return this.RedirectToAction("All");
            }

            return View(product);
        }

        public IActionResult AllAsJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return Json(_products, options);
        }

        public IActionResult AllAsTextFile()
        {
            StringBuilder sb = new StringBuilder();

            foreach (ProductViewModel product in _products)
            {
                sb.AppendLine($"Product: {product.Id}, {product.Name} - {product.Price:f2} $");
            }

            Response.Headers.Add(HeaderNames.ContentDisposition, @"attachment;filename=products.txt");
            //Response.Headers.Add(HeaderNames.ContentDisposition, @"filename=products.txt"); //bez da se otwarq w browsera awtomat

            return File(Encoding.UTF8.GetBytes(sb.ToString().TrimEnd()), "text/plain");
        }
    }
}