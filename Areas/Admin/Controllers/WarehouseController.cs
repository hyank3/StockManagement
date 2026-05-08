using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StockManagement.Models;
using StockManagement.Reprository;

namespace StockManagement.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class WarehouseController : Controller
    {
        private readonly DataContext _dbContext;

        public WarehouseController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }



        [HttpGet]
        public IActionResult Import()
        {
            var productList = _dbContext.Products
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.ProductName} (Tồn: {p.ProductQuantity})"
                })
                .ToList();

            ViewBag.Products = productList;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(WarehouseTransactionModel model)
        {
            ModelState.Remove("TransactionType");
            ModelState.Remove("Product");

            if (ModelState.IsValid)
            {
                model.TransactionType = "Import";
                model.TransactionDate = DateTime.Now;

                // Lưu giao dịch nhập kho
                _dbContext.WarehouseTransactions.Add(model);

                // Cập nhật số lượng sản phẩm
                var product = await _dbContext.Products.FindAsync(model.ProductId);
                if (product != null)
                {
                    product.ProductQuantity += model.Quantity;   // Giả sử tên thuộc tính là ProductQuantity
                    _dbContext.Products.Update(product);
                }

                TempData["SuccessMessage"] = "Nhập số lượng thành công cho sản phẩm.";
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Import", "Warehouse");
            }

            // Nếu lỗi validation, load lại danh sách sản phẩm
            var productList = _dbContext.Products
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.ProductName} (Tồn: {p.ProductQuantity})"
                })
                .ToList();

            ViewBag.Products = productList;

            return View(model);
        }

        [HttpGet]
        public IActionResult Export()
        {
            var productList = _dbContext.Products
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.ProductName} (Tồn: {p.ProductQuantity})"
                })
                .ToList();

            ViewBag.Products = productList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Export(WarehouseTransactionModel model)
        {
            ModelState.Remove("TransactionType");
            ModelState.Remove("Product");

            if (ModelState.IsValid)
            {
                model.TransactionType = "Export";
                model.TransactionDate = DateTime.Now;

                // Lưu giao dịch nhập kho
                _dbContext.WarehouseTransactions.Add(model);

                // Cập nhật số lượng sản phẩm
                var product = await _dbContext.Products.FindAsync(model.ProductId);
                if (product != null)
                {
                    product.ProductQuantity -= model.Quantity;   // Giả sử tên thuộc tính là ProductQuantity
                    _dbContext.Products.Update(product);
                }

                TempData["SuccessMessage"] = "Xuất số lượng thành công cho sản phẩm.";
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Export", "Warehouse");
            }

            // Nếu lỗi validation, load lại danh sách sản phẩm
            var productList = _dbContext.Products
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.ProductName} (Tồn: {p.ProductQuantity})"
                })
                .ToList();

            ViewBag.Products = productList;

            return View(model);
        }
    }
}
