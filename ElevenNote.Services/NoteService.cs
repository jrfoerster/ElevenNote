using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                CreatedUtc = DateTimeOffset.Now
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
                    .Where(n => n.OwnerId == _userId)
                    .Select(n => new NoteListItem()
                    {
                        NoteId = n.Id,
                        Title = n.Title,
                        CreatedUtc = n.CreatedUtc
                    });

                return query.ToList();
            }
        }
    }
}
