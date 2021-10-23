using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateNote(NoteCreate model)
        {
            var note = new Note()
            {
                OwnerId = _userId,
                Title = model.Title,
                Content = model.Content,
                CategoryId = model.CategoryId,
                CreatedUtc = DateTimeOffset.UtcNow
            };

            using (var context = ApplicationDbContext.Create())
            {
                context.Notes.Add(note);
                return context.SaveChanges() == 1;
            }
        }

        public List<NoteListItem> GetNotes()
        {
            using (var context = ApplicationDbContext.Create())
            {
                var query = context.Notes
                    .Include(n => n.Category)
                    .Where(n => n.OwnerId == _userId)
                    .Select(n => new NoteListItem()
                    {
                        NoteId = n.Id,
                        Title = n.Title,
                        CategoryName = n.Category.CategoryName,
                        IsStarred = n.IsStarred,
                        CreatedUtc = n.CreatedUtc
                    });

                return query.ToList();
            }
        }

        public NoteDetail GetNoteById(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var note = context.Notes
                    .Include(n => n.Category)
                    .Single(n => n.Id == id && n.OwnerId == _userId);

                var model = new NoteDetail()
                {
                    NoteId = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    CategoryName = note.Category.CategoryName,
                    IsStarred = note.IsStarred,
                    CreatedUtc = note.CreatedUtc,
                    ModifiedUtc = note.ModifiedUtc
                };

                return model;
            }
        }

        public bool UpdateNote(NoteEdit model)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var note = context.Notes.Single(n => n.Id == model.NoteId && n.OwnerId == _userId);
                note.Title = model.Title;
                note.Content = model.Content;
                note.IsStarred = model.IsStarred;
                note.ModifiedUtc = DateTimeOffset.UtcNow;

                return context.SaveChanges() == 1;
            }
        }

        public bool DeleteNote(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var note = context.Notes.Single(n => n.Id == id && n.OwnerId == _userId);
                context.Notes.Remove(note);
                return context.SaveChanges() == 1;
            }
        }
    }
}
