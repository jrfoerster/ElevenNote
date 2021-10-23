using ElevenNote.Data;
using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMvc.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private CategoryService CreateCategoryService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            return new CategoryService(userId);
        }

        // GET: Note
        public ActionResult Index()
        {
            var service = CreateCategoryService();
            var categories = service.GetCategories();
            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryCreate model)
        {
            if (ModelState.IsValid)
            {
                var service = CreateCategoryService();
                if (service.CreateCategory(model))
                {
                    TempData["SaveResult"] = "Your note was created.";
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("", "Note could not be created.");
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var service = CreateCategoryService();
            var detail = service.GetCategoryById(id);
            return View(detail);
        }

        public ActionResult Edit(int id)
        {
            var service = CreateCategoryService();
            var detail = service.GetCategoryById(id);
            var model = new CategoryEdit()
            {
                CategoryId = detail.CategoryId,
                CategoryName = detail.CategoryName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryEdit model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            if (model.CategoryId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateCategoryService();

            if (service.UpdateCategory(model))
            {
                TempData["SaveResult"] = "Your note was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your note could not be updated.");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var service = CreateCategoryService();
            var detail = service.GetCategoryById(id);
            return View(detail);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateCategoryService();
            service.DeleteCategory(id);
            TempData["SaveResult"] = "Your note was deleted";
            return RedirectToAction("Index");
        }
    }
}