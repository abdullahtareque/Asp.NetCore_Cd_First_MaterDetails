using Core_Master.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core_Master.Controllers
{
    public class OrderController : Controller
    {
        private readonly myContext _db;
        private readonly IWebHostEnvironment webHost;

        public OrderController(myContext db, IWebHostEnvironment web)
        {
            _db = db;
            webHost = web;
        }

        public IActionResult Index()
        {
            var orders = _db.OrderMasters.Include(o => o.OrderDetails).ToList();
            return View(orders);
        }


        public IActionResult Create()
        {
            OrderVM vm = new OrderVM();
            vm.OrderMaster = new OrderMaster();
            vm.OrderMaster.OrderDetails.Add(new OrderDetail() { OrderDetailId = 1 });

            SelectList productList = new SelectList(_db.Products, "ProductId", "ProductName");

            ViewBag.ProductList = productList;

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderVM vm)
        {
            if (ModelState.IsValid)
            {
                var folder = "Images/" + Guid.NewGuid().ToString() + vm.imagefile.FileName;

                var path = Path.Combine(webHost.WebRootPath, folder);

                vm.imagefile.CopyTo(new FileStream(path, FileMode.Create));

                var data = new OrderMaster()
                {
                    OrderNote=vm.OrderMaster.OrderNote,
                    OrderDate=vm.OrderMaster.OrderDate,
                    ImagePath=folder,
                    Terms = vm.OrderMaster.Terms,
                    OrderDetails =vm.OrderMaster.OrderDetails
                };

                _db.OrderMasters.Add(data);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }


        public IActionResult Edit(int id)
        {
            OrderMaster order = _db.OrderMasters.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            OrderVM vm = new OrderVM
            {
                OrderMaster = order
            };

            SelectList productList = new SelectList(_db.Products, "ProductId", "ProductName");

            ViewBag.ProductList = productList;

            return View("Edit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(OrderVM vm)
        {
            if (ModelState.IsValid)
            {
                var orderMaster = _db.OrderMasters
                    .Include(o => o.OrderDetails)
                    .FirstOrDefault(o => o.OrderId == vm.OrderMaster.OrderId);

                if (orderMaster == null)
                {
                    return NotFound();
                }

                orderMaster.OrderNote = vm.OrderMaster.OrderNote;
                orderMaster.OrderDate = vm.OrderMaster.OrderDate;

                if (vm.imagefile != null)
                {
                    var folder = "Images/" + Guid.NewGuid().ToString() + vm.imagefile.FileName;
                    var path = Path.Combine(webHost.WebRootPath, folder);
                    vm.imagefile.CopyTo(new FileStream(path, FileMode.Create));
                    orderMaster.ImagePath = folder;

                }
                orderMaster.Terms = vm.OrderMaster.Terms;
                orderMaster.OrderDetails = vm.OrderMaster.OrderDetails;

                _db.OrderMasters.Update(orderMaster);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ProductList = new SelectList(_db.Products, "ProductId", "ProductName");
            return View("Edit", vm);
        }



        public IActionResult Delete(int? id)
        {
            OrderMaster order = _db.OrderMasters
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(OrderMaster orderMaster)
        {
            if (ModelState.IsValid)
            {
                _db.OrderMasters.Remove(orderMaster);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }


        public IActionResult Details(int id)
        {
            OrderMaster order = _db.OrderMasters
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

    }
}
