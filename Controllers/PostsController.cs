using System;
using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.Controllers;

public class PostsController : Controller
{
    private IPostRepository _postRepository;
    private ITagRepository _tagRepository;
    private ICommentRepository _commentRepository;
    private IUserRepository _userRepository;

    public PostsController(IPostRepository postRepository, ITagRepository tagRepository, ICommentRepository commentRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _tagRepository = tagRepository;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }



    public IActionResult Index(string? tag)
    {
        var posts = _postRepository.Posts;


        if (!string.IsNullOrEmpty(tag))
        {
            posts = posts.Where(p => p.PostTags.Any(t => t.TagUrl == tag));
        }




        var tags = _tagRepository.Tags.ToList();
        PostViewModel postViewModel = new PostViewModel
        {
            Posts = posts.Include(p => p.PostTags).Include(p => p.User).ToList(),
            Tags = tags
        };


        return View(postViewModel);
    }
    public async Task<IActionResult> Details(string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return NotFound();
        }
        var post = await _postRepository
            .Posts
            .Include(p => p.PostTags)
            .Include(p => p.PostComments)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(p => p.PostUrl == url);

        if (post == null)
        {
            return NotFound();
        }
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr != null && post.UserId == int.Parse(userIdStr))
        {
            ViewBag.IsOwner = true;
        }
        else
        {
            ViewBag.IsOwner = false;
        }

        return View(post);


    }

    [HttpPost]
    public async Task<JsonResult> AddComment(int PostId, [Bind("CommentText")] Comment comment)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        User? user = null;

        if (int.TryParse(userId, out int id))
        {
            user = await _userRepository.Users
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        if (user == null)
        {
            return Json(new { error = "Kullanıcı bulunamadı" });
        }
        comment.User = user;

        if (comment.CommentText != null)
        {
            comment.CommentDate = DateTime.UtcNow;
            comment.UserId = comment.User.UserId;
            await _commentRepository.AddCommentAsync(PostId, comment);
            return Json(new
            {
                comment.User.UserName,
                comment.CommentDate,
                comment.CommentText,
                comment.User.UserImage

            });
        }
        return Json(new { error = "Text Boş bulunamadı" });
    }


    public IActionResult Create()
    {
        ViewBag.Tags = _tagRepository.Tags.ToList();

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return View();
        }
        return RedirectToAction("Login", "User");
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateViewModel model)
    {
        ViewBag.Tags = _tagRepository.Tags.ToList();

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        if (int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
        {
            string? fileName = null;

            // Resim kaydetme
            if (model.PostImageFile != null && model.PostImageFile.Length > 0)
            {
                // wwwroot/img klasörünü hazırla
                string uploadPath = Path.Combine("wwwroot", "img");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Benzersiz isim
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.PostImageFile.FileName);
                string filePath = Path.Combine(uploadPath, fileName);
                model.PostImage = fileName;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PostImageFile.CopyToAsync(stream);
                }
            }



            await _postRepository.AddPostAsync(model, userId);
        }
        else
        {
            ModelState.AddModelError("", "UserId bulunamadı");
            return View(model);
        }
        return RedirectToAction("Index", "Posts");
    }

    public async Task<IActionResult> Edit(string url)
    {
        var post = await _postRepository.Posts
            .Include(p => p.PostTags)
            .FirstOrDefaultAsync(p => p.PostUrl == url);

        if (post == null)
        {
            return NotFound();
        }

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null || post.UserId != int.Parse(userIdStr))
        {
            return Forbid();
        }

        var model = new CreateViewModel
        {
            PostName = post.PostName,
            PostText = post.PostText,
            PostImage = post.PostImage,
            SelectedTagIds = post.PostTags.Select(t => t.TagId).ToList()
        };

        ViewBag.Tags = _tagRepository.Tags.ToList();

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(string url, CreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Tags = _tagRepository.Tags.ToList();
            return View(model);
        }
        var post = await _postRepository.Posts
            .Include(p => p.PostTags)
            .FirstOrDefaultAsync(p => p.PostUrl == url);
        if (post == null)
        {
            return NotFound();
        }
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null || post.UserId != int.Parse(userIdStr))
        {
            return Forbid();
        }
        if (model.PostImageFile != null && model.PostImageFile.Length > 0)
        {
            // wwwroot/img klasörünü hazırla
            string uploadPath = Path.Combine("wwwroot", "img");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Benzersiz isim
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.PostImageFile.FileName);
            string filePath = Path.Combine(uploadPath, fileName);
            model.PostImage = fileName; // Yeni dosya adını modele ata

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.PostImageFile.CopyToAsync(stream);
            }
        }



        await _postRepository.EditPostAsync(post, model);
        return RedirectToAction("Details", new { url = post.PostUrl });


    }



    public async Task<IActionResult> Delete(string url)
    {
        var post = await _postRepository.Posts
            .FirstOrDefaultAsync(p => p.PostUrl == url);

        if (post == null)
        {
            return NotFound();
        }

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null || post.UserId != int.Parse(userIdStr))
        {
            return Forbid();
        }

        return View(post);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed( int PostId)
    {
        var post = await _postRepository.Posts
            .FirstOrDefaultAsync(p => p.PostId == PostId);

        if (post == null)
        {
            return NotFound();
        }

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null || post.UserId != int.Parse(userIdStr))
        {
            return Forbid();
        }

        await _postRepository.DeletePostAsync(post);
        return RedirectToAction("Index", "Posts");
    }
}
