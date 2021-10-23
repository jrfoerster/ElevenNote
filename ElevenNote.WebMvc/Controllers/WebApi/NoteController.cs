using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Http;

namespace ElevenNote.WebMvc.Controllers.WebApi
{
    [Authorize]
    [RoutePrefix("api/Note")]
    public class NoteController : ApiController
    {
        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            return new NoteService(userId);
        }

        private bool SetStarState(int noteId, bool newState)
        {
            var service = CreateNoteService();
            var detail = service.GetNoteById(noteId);

            var updated = new NoteEdit()
            {
                NoteId = detail.NoteId,
                Title = detail.Title,
                CategoryId = detail.CategoryId,
                Content = detail.Content,
                IsStarred = newState
            };

            return service.UpdateNote(updated);
        }

        [Route("{id}/Star")]
        [HttpPut]
        public bool ToggleStarOn(int id)
        {
            return SetStarState(id, true);
        }

        [Route("{id}/Star")]
        [HttpDelete]
        public bool ToggleStarOff(int id)
        {
            return SetStarState(id, false);
        }
    }
}
