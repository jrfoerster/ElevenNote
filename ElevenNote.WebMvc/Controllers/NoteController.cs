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
    }
}