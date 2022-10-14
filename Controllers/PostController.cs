using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCore.Models;
using NetCore.Data;
using NetCore.Models;
using System.Diagnostics;

namespace NetCore.Controllers
{

    [Authorize]
    public class PostController : Controller
    {
        public PostController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            using (BlogContext context = new BlogContext())
            {
                IQueryable<Post> posts = context.Posts;

                return View("Index", posts.ToList());
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            using (BlogContext context = new BlogContext())
            {
                // aggiungiamo l'include di Tags
                Post postFound = context.Posts.Where(post => post.Id == id).Include(post => post.Category).Include(m => m.Tags).FirstOrDefault();

                if (postFound == null)
                {
                    return NotFound($"Il post con id {id} non è stato trovato");
                }
                else
                {
                    return View("Details", postFound);
                }
            }
        }

     
        [HttpGet]
        public IActionResult Create()
        {
            using (BlogContext context = new BlogContext())
            {
                List<Category> categories = context.Categories.ToList();
                List<Tag> tags = context.Tags.ToList();

                PostCategories model = new PostCategories();
                model.Post = new Post();
                model.Categories = categories;

                List<SelectListItem> listTags = new List<SelectListItem>();

                foreach(Tag tag in tags)
                {
                    listTags.Add(new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString() });
                }

                model.Tags = listTags;

                return View("Create", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostCategories data)
        {
            if (!ModelState.IsValid)
            {
                using (BlogContext context = new BlogContext())
                {
                    // In caso di errore di validazione dobbiamo restituire ogni volta il model
                    // popolato con la lista delle categorie, perchè questi dati non vengono
                    // passati dal post e restituendolo direttamente la property Categories
                    // sarebbe null e la pagina darebbe errore
                    List<Category> categories = context.Categories.ToList();
                    List<Tag> tags = context.Tags.ToList();

                    data.Categories = categories;

                    List<SelectListItem> listTags = new List<SelectListItem>();

                    foreach (Tag tag in tags)
                    {
                        listTags.Add(new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString() });
                    }

                    data.Tags = listTags;

                    return View("Create", data);
                }
            }

            using (BlogContext context = new BlogContext())
            {
                Post postToCreate = new Post();
                postToCreate.Title = data.Post.Title;
                postToCreate.Description = data.Post.Description;
                postToCreate.Image = data.Post.Image;
                postToCreate.CategoryId = data.Post.CategoryId;

                postToCreate.Tags = new List<Tag>();

                // se non è stato selezionato nessun tag, la proprietà sarà null, e genererà errore
                if (data.SelectedTags != null)
                {
                    foreach (string selectedTagId in data.SelectedTags)
                    {
                        int selectedIntTagId = int.Parse(selectedTagId);

                        Tag tag = context.Tags.Where(m => m.Id == selectedIntTagId).FirstOrDefault();

                        postToCreate.Tags.Add(tag);
                    }
                }

                context.Posts.Add(postToCreate);

                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (BlogContext context = new BlogContext())
            {
                // aggiungiamo include perchè vogliamo caricare anche i Tag del Post
                Post postToEdit = context.Posts.Where(post => post.Id == id).Include(m=>m.Tags).FirstOrDefault();

                if (postToEdit == null)
                {
                    return NotFound();
                }
                else
                {
                    List<Category> categories = context.Categories.ToList();

                    PostCategories model = new PostCategories();
                    model.Post = postToEdit;
                    model.Categories = categories;

                    List<Tag> tags = context.Tags.ToList();

                    List<SelectListItem> listTags = new List<SelectListItem>();

                    foreach (Tag tag in tags)
                    {
                        listTags.Add(
                            new SelectListItem() { 
                                Text = tag.Title, 
                                Value = tag.Id.ToString(), 
                                // dobbiamo settare come selezionati i tag che sono presenti nel Post
                                Selected = postToEdit.Tags.Any(m=>m.Id == tag.Id) 
                            });
                    }

                    model.Tags = listTags;

                    return View(model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PostCategories data)
        {
            if (!ModelState.IsValid)
            {
                using (BlogContext context = new BlogContext())
                {
                    // In caso di errore di validazione dobbiamo restituire ogni volta il model
                    // popolato con la lista delle categorie, perchè questi dati non vengono
                    // passati dal post e restituendolo direttamente la property Categories
                    // sarebbe null e la pagina darebbe errore
                    List<Category> categories = context.Categories.ToList();
                    data.Categories = categories;

                    List<Tag> tags = context.Tags.ToList();

                    List<SelectListItem> listTags = new List<SelectListItem>();

                    foreach (Tag tag in tags)
                    {
                        listTags.Add(new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString() });
                    }

                    data.Tags = listTags;

                    return View("Update", data);
                }
            }

            using (BlogContext context = new BlogContext())
            {
                // dobbiamo caricare anche i Tag collegati al post
                Post postToEdit = context.Posts.Where(post => post.Id == id).Include(m => m.Tags).FirstOrDefault();

                if (postToEdit != null)
                {
                    // aggiorniamo i campi con i nuovi valori
                    postToEdit.Title = data.Post.Title;
                    postToEdit.Description = data.Post.Description;
                    postToEdit.Image = data.Post.Image;
                    postToEdit.CategoryId = data.Post.CategoryId;

                    // rimuoviamo i tag e inseriamo i nuovi
                    postToEdit.Tags.Clear();

                    // se non è stato selezionato nessun tag, la proprietà sarà null, e genererà errore
                    if (data.SelectedTags != null)
                    {
                        foreach (string selectedTagId in data.SelectedTags)
                        {
                            int selectedIntTagId = int.Parse(selectedTagId);

                            Tag tag = context.Tags.Where(m => m.Id == selectedIntTagId).FirstOrDefault();

                            postToEdit.Tags.Add(tag);
                        }
                    }

                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using (BlogContext context = new BlogContext())
            {
                Post postToDelete = context.Posts.Where(post => post.Id == id).FirstOrDefault();

                if (postToDelete != null)
                {
                    context.Posts.Remove(postToDelete);

                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}