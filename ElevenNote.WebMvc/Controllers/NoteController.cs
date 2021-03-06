using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ElevenNote.WebMvc.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            return new NoteService(userId);
        }

        // GET: Note
        public ActionResult Index()
        {
            var service = CreateNoteService();
            var notes = service.GetNotes();
            return View(notes);
        }

        public ActionResult Create()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CategoryService(userId);
            var categories = service.GetCategories();
            ViewBag.Categories = categories
                .OrderBy(c => c.CategoryName)
                .Select(c => new SelectListItem()
                {
                    Text = c.CategoryName,
                    Value = c.CategoryId.ToString()
                });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if (ModelState.IsValid)
            {
                var service = CreateNoteService();
                if (service.CreateNote(model))
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
            var service = CreateNoteService();
            var detail = service.GetNoteById(id);
            return View(detail);
        }

        public ActionResult Edit(int id)
        {
            var service = CreateNoteService();
            var detail = service.GetNoteById(id);
            var model = new NoteEdit()
            {
                NoteId = detail.NoteId,
                Title = detail.Title,
                Content = detail.Content,
                IsStarred = detail.IsStarred
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            if (model.NoteId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateNoteService();

            if (service.UpdateNote(model))
            {
                TempData["SaveResult"] = "Your note was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your note could not be updated.");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var service = CreateNoteService();
            var model = service.GetNoteById(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateNoteService();
            service.DeleteNote(id);
            TempData["SaveResult"] = "Your note was deleted";
            return RedirectToAction("Index");
        }
    }
}