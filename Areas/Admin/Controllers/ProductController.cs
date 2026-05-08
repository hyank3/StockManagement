using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagement.Models;
using StockManagement.Reprository;   // hoặc DataContext ở đây
using System;                         // ← Thêm dòng này cho DateTime

namespace StockManagement.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _dbContext;   // ← Đã sửaTask<IActionResult>

        public ProductController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 10;                    // Số sản phẩm trên 1 trang

            var products = _dbContext.Products
                                     .OrderBy(p => p.Id)     // Giữ nguyên cách sắp xếp của bạn
                                     .Skip((page - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToList();

            int totalItems = _dbContext.Products.Count();

            // Truyền thông tin phân trang qua ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.UpdateDate = DateTime.Now;

                await _dbContext.Products.AddAsync(model);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Product");
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Product");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductModel model, int id)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                // Cập nhật các trường cần thiết
                product.ProductCode = model.ProductCode;
                product.ProductName = model.ProductName;
                product.ProductUnit = model.ProductUnit;
                product.ProductQuantity = model.ProductQuantity;
                product.Location = model.Location;
                product.ProductDescription = model.ProductDescription;
                product.UpdateDate = DateTime.Now;

                _dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction("Index", "Product");
            }
            return (View(model));
            }
        }
}