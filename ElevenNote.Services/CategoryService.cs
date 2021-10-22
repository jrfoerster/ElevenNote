using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class CategoryService
    {
        private readonly Guid _userId;

        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateCategory(CategoryCreate model)
        {
            var category = new Category()
            {
                OwnerId = _userId,
                CategoryName = model.CategoryName
            };

            using (var context = ApplicationDbContext.Create())
            {
                context.Categories.Add(category);
                return context.SaveChanges() == 1;
            }
        }

        public List<CategoryListItem> GetCategories()
        {
            using (var context = ApplicationDbContext.Create())
            {
                var query = context.Categories
                    .Include(c => c.Notes)
                    .Where(c => c.OwnerId == _userId)
                    .Select(c => new CategoryListItem()
                    {
                        CategoryId = c.Id,
                        CategoryName = c.CategoryName,
                        NoteCount = c.Notes.Count
                    });

                return query.ToList();
            }
        }

        public CategoryDetail GetCategoryById(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var category = context.Categories
                    .Include(c => c.Notes)
                    .Single(c => c.Id == id && c.OwnerId == _userId);

                var detail = new CategoryDetail()
                {
                    CategoryId = category.Id,
                    CategoryName = category.CategoryName,
                };

                detail.Notes = category.Notes.Select(n => new NoteDetail()
                {
                    NoteId = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    IsStarred = n.IsStarred,
                    CreatedUtc = n.CreatedUtc,
                    ModifiedUtc = n.ModifiedUtc
                }).ToList();

                return detail;
            }
        }

        public bool UpdateCategory(CategoryEdit model)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var category = context.Categories.Single(c => c.Id == model.CategoryId && c.OwnerId == _userId);
                category.CategoryName = model.CategoryName;
                return context.SaveChanges() == 1;
            }
        }

        public bool DeleteCategory(int id)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var category = context.Categories.Single(n => n.Id == id && n.OwnerId == _userId);
                context.Categories.Remove(category);
                return context.SaveChanges() == 1;
            }
        }
    }
}
