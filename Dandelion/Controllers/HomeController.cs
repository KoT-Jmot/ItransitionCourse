using Dandelion.Data;
using Dandelion.Models;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Dandelion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var testDBContext = new TestDBContext();

            var a = testDBContext.Collections.ToList();
            var b = testDBContext.Items.ToList();
            if (a.Count >= 10)
                ViewData["Collect"] = a.GetRange(0, 10);
            else
                ViewData["Collect"] = a.ToList();

            if (b.Count >= 5)
                ViewData["Item"] = b.GetRange(0, 5);
            else
                ViewData["Item"] = b;
            ViewData["UserLogin"] = null;
            ViewData["UserStatus"] = null;
            ViewData["UserName"] = null;
            return View("Index");
        }
        [HttpPost]
        public IActionResult Index(string login)
        {
            var testDBContext = new TestDBContext();
            if (testDBContext.Collections.ToList().Count >= 10)
                ViewData["Collect"] = testDBContext.Collections.ToList().GetRange(0, 10);
            else
                ViewData["Collect"] = testDBContext.Collections.ToList();

            if (testDBContext.Items.ToList().Count >= 5)
                ViewData["Item"] = testDBContext.Items.ToList().GetRange(0, 5);
            else
                ViewData["Item"] = testDBContext.Items.ToList();
            Data.User person = testDBContext.Users.FirstOrDefault(a => a.Login == login);
            if (login != null)
            {
                ViewData["UserName"] = person.Name;
                ViewData["UserLogin"] = person.Login;
                ViewData["UserStatus"] = person.Status;
            }
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CreateCollection(IFormFile uploadFile, Collection model, string login)
        {
            if (IsUnBlock(login))
            {
                var testDBContext = new TestDBContext();
                Data.User person = testDBContext.Users.FirstOrDefault(a => a.Login == login);
                ViewData["UserName"] = person.Name;
                ViewData["UserLogin"] = person.Login;
                ViewData["UserStatus"] = person.Status;
                ViewData["Collect"] = testDBContext.Collections.ToList();
                if (model.Login != null && model.Name != null)
                {
                    int x = 1;
                    while (testDBContext.Collections.FirstOrDefault(a => a.Id == x) != null)
                        x++;
                    model.Id = x;
                    if (uploadFile != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            uploadFile.CopyTo(ms);
                            model.Pic = ms.ToArray();
                        }
                    }
                    testDBContext.Collections.Add(model);
                    testDBContext.SaveChanges();
                    if (person.Status == "Admin")
                    {
                        ViewData["Collect"] = testDBContext.Collections.ToList();
                        return View("Profile");
                    }
                    else
                    {
                        List<Collection> Collect = new List<Collection>();
                        try
                        {
                            foreach (var col in testDBContext.Collections)
                                if (col.Login == login)
                                    Collect.Add(new Collection(col));

                        }
                        catch
                        {
                            Collect = null;
                        }
                        ViewData["Collect"] = Collect;
                        return View("Profile");
                    }
                    return View("Profile");
                }
                else
                {
                    ModelState.AddModelError("Name", "Enter correct data!");
                    return View("Profile");
                }
            }
            else
            {
                ModelState.AddModelError("Name", "Enter correct data!");
                return View("LogIn");
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Data.User model)
        {
            if (model.Name != null && model.Password != null && model.Login != null && model.Password.Length > 0 && isValid(model.Login))
            {
                model.Status = "UnBlock";
                var testDBContext = new TestDBContext();
                if (testDBContext.Users.FirstOrDefault(a => a.Login == model.Login) == null)
                {
                    int x = 1;
                    while (testDBContext.Users.FirstOrDefault(a => a.Id == x) != null)
                        x++;
                    model.Id = x;
                    testDBContext.Users.Add(model);
                    testDBContext.SaveChanges();
                    ViewData["UserName"] = model.Name;
                    ViewData["UserLogin"] = model.Login;
                    ViewData["UserStatus"] = model.Status;
                    ViewData["Collect"] = testDBContext.Collections.ToList();
                    ViewData["Item"] = testDBContext.Items.ToList();
                    return View("Index");
                }
                else
                    ModelState.AddModelError("Name", "This account already exists!");
            }
            else
                ModelState.AddModelError("Name", "Enter correct data!");
            return View();
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(Data.User model)
        {

            if (model.Login != null && model.Password != null)
            {
                if (IsUnBlock(model.Login))
                {
                    var testDBContext = new TestDBContext();
                    var person = testDBContext.Users.FirstOrDefault(a => a.Login == model.Login && a.Password == model.Password);
                    if (person != null)
                    {
                        testDBContext.SaveChanges();
                        ViewData["UserName"] = person.Name;
                        ViewData["UserLogin"] = person.Login;
                        ViewData["UserStatus"] = person.Status;
                        ViewData["Collect"] = testDBContext.Collections.ToList();
                        ViewData["Item"] = testDBContext.Items.ToList();
                        return View("Index");
                    }
                }
            }
            ModelState.AddModelError("Name", "Enter correct data!");
            return View();
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            var testDBContext = new TestDBContext();
            ViewData["Collect"] = testDBContext.Collections.ToList();
            ViewData["Item"] = testDBContext.Items.ToList();
            ViewData["UserName"] = null;
            ViewData["UserLogin"] = null;
            ViewData["UserStatus"] = null;
            return View("Index");
        }

        [HttpPost]
        public IActionResult Profile(string login)
        {
            if (IsUnBlock(login))
            {
                var testDBContext = new TestDBContext();
                Data.User person = testDBContext.Users.FirstOrDefault(a => a.Login == login);
                ViewData["UserName"] = person.Name;
                ViewData["UserLogin"] = person.Login;
                ViewData["UserStatus"] = person.Status;
                if (person.Status == "Admin")
                {
                    ViewData["Collect"] = testDBContext.Collections.ToList();
                    return View("Profile");
                }
                else
                {
                    List<Collection> Collect = new List<Collection>();
                    try
                    {
                        foreach (var col in testDBContext.Collections)
                            if (col.Login == login)
                                Collect.Add(new Collection(col));
                    }
                    catch
                    {
                        Collect = null;
                    }
                    ViewData["Collect"] = Collect;
                    return View("Profile");
                }

            }
            else
            {
                ModelState.AddModelError("Name", "Enter correct data!");
                return View("LogIn");
            }
        }
        [HttpPost]
        public IActionResult AccountManager(string login)
        {
            if (IsUnBlock(login))
            {
                var testDBContext = new TestDBContext();
                Data.User person = testDBContext.Users.FirstOrDefault(a => a.Login == login);
                ViewData["UserName"] = person.Name;
                ViewData["UserLogin"] = person.Login;
                ViewData["UserStatus"] = person.Status;
                return View(testDBContext.Users.ToList());
            }
            else
            {
                ModelState.AddModelError("Name", "Enter correct data!");
                return View("LogIn");
            }
        }
        [HttpPost]
        public IActionResult ActionWithAccaunt(int[] selectedUsers, string login, string actions)
        {
            if (IsUnBlock(login))
            {
                var testDBContext = new TestDBContext();
                if (selectedUsers != null)
                {

                    for (int i = 0; i < selectedUsers.Length; i++)
                    {
                        if (testDBContext.Users.FirstOrDefault(a => a.Id == selectedUsers[i]) != null)
                        {
                            switch (actions)
                            {
                                case "Delete":
                                    testDBContext.Users.Remove(testDBContext.Users.FirstOrDefault(a => a.Id == selectedUsers[i]));
                                    break;
                                default:
                                    testDBContext.Users.FirstOrDefault(a => a.Id == selectedUsers[i]).Status = actions;
                                    break;
                            }

                        }
                    }
                    testDBContext.SaveChanges();
                }
                Data.User person = testDBContext.Users.FirstOrDefault(a => a.Login == login);

                if (person.Status == "Admin")
                {
                    ViewData["UserName"] = person.Name;
                    ViewData["UserLogin"] = person.Login;
                    ViewData["UserStatus"] = person.Status;
                    return View("AccountManager", testDBContext.Users.ToList());
                }
                else
                {
                    ModelState.AddModelError("Name", "Enter correct data!");
                    return View("LogIn");
                }
            }
            else
            {
                ModelState.AddModelError("Name", "Enter correct data!");
                return View("LogIn");
            }
        }

        [HttpPost]
        public IActionResult Items(int a, string login)
        {
            var testDBContext = new TestDBContext();
            Data.User person = testDBContext.Users.FirstOrDefault(b => b.Login == login);
            if (person != null)
            {
                ViewData["UserName"] = person.Name;
                ViewData["UserLogin"] = person.Login;
                ViewData["UserStatus"] = person.Status;
            }
            List<Item> Items = new List<Item>();
            try
            {
                foreach (var col in testDBContext.Items)
                    if (col.CollectionId == a)
                        Items.Add(new Item(col));
            }
            catch
            {
                Items = null;
            }
            ViewData["Item"] = Items;
            ViewData["Coll"] = testDBContext.Collections.FirstOrDefault(b => b.Id == a);
            return View("Items");
        }

        [HttpPost]
        public IActionResult CreateItem(int CollId, int[] chackboxes, Item model, string login, IFormFile uploadFile)
        {
            if (IsUnBlock(login))
            {
                var testDBContext = new TestDBContext();

                if (model.Name != null && model.Tegs != null)
                {
                    int x = 1;
                    while (testDBContext.Items.FirstOrDefault(a => a.Id == x) != null)
                        x++;
                    model.Id = x;
                    model.CollectionId = CollId;
                    if (uploadFile != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            uploadFile.CopyTo(ms);
                            model.Pic = ms.ToArray();
                        }
                    }
                    model.Checkbox1 = false;
                    model.Checkbox2 = false;
                    model.Checkbox3 = false;
                    model.CreateDate = DateTime.Now;
                    for (int i = 0; i < chackboxes.Length; i++)
                    {
                        switch (chackboxes[i])
                        {
                            case 1:
                                model.Checkbox1 = true;
                                break;
                            case 2:
                                model.Checkbox2 = true;
                                break;
                            case 3:
                                model.Checkbox3 = true;
                                break;
                        }
                    }
                    testDBContext.Items.Add(model);
                    testDBContext.SaveChanges();

                }
                else
                    ModelState.AddModelError("Name", "Enter correct data!");
                Data.User person = testDBContext.Users.FirstOrDefault(a => a.Login == login);
                ViewData["UserName"] = person.Name;
                ViewData["UserLogin"] = person.Login;
                ViewData["UserStatus"] = person.Status;
                ViewData["Coll"] = testDBContext.Collections.FirstOrDefault(a => a.Id == CollId);

                List<Item> Items = new List<Item>();
                try
                {
                    foreach (var col in testDBContext.Items)
                        if (col.CollectionId == CollId)
                            Items.Add(new Item(col));
                }
                catch
                {
                    Items = null;
                }
                ViewData["Item"] = Items;

                return View("Items");

            }
            else
            {
                ModelState.AddModelError("Name", "Enter correct data!");
                return View("LogIn");
            }
        }

        [HttpPost]
        public IActionResult ItemInfo(string login, int a)
        {
            var testDBContext = new TestDBContext();
            Data.User person = testDBContext.Users.FirstOrDefault(b => b.Login == login);
            if (person != null)
            {
                ViewData["UserName"] = person.Name;
                ViewData["UserLogin"] = person.Login;
                ViewData["UserStatus"] = person.Status;
            }
            var i = testDBContext.Items.FirstOrDefault(b => b.Id == a);
            ViewData["Item"] = i;
            ViewData["Coll"] = testDBContext.Collections.FirstOrDefault(b => b.Id == i.CollectionId);
            List<Comment> Comments = new List<Comment>();
            try
            {
                foreach (var col in testDBContext.Comments)
                    if (col.ItemId == a)
                        Comments.Add(col);
            }
            catch
            {
                Comments = null;
            }
            ViewData["Comments"] = Comments;
            return View("ItemInfo");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        bool isValid(string email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }
        public bool IsUnBlock(string login)
        {
            try
            {
                var testDBContext = new TestDBContext();
                if (testDBContext.Users.FirstOrDefault(a => a.Login == login && a.Status != "Block") != null)
                {
                    ViewData["User"] = login;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }
        }

        [HttpPost]
        public IActionResult SaveCollInfo(string Names, string Description, string Tag, int CollId, string login)
        {
            if (IsUnBlock(login))
            {
                var testDBContext = new TestDBContext();
                Data.User person = testDBContext.Users.FirstOrDefault(b => b.Login == login);
                ViewData["UserName"] = person.Name;
                ViewData["UserLogin"] = person.Login;
                ViewData["UserStatus"] = person.Status;

                Collection itm = testDBContext.Collections.FirstOrDefault(b => b.Id == CollId);
                {
                    itm.Name = Names;
                    itm.Description = Description;
                    itm.Tag = Tag;
                }
                testDBContext.SaveChanges();
                List<Item> Items = new List<Item>();
                try
                {
                    foreach (var col in testDBContext.Items)
                        if (col.CollectionId == CollId)
                            Items.Add(new Item(col));
                }
                catch
                {
                    Items = null;
                }
                ViewData["Item"] = Items;
                ViewData["Coll"] = testDBContext.Collections.FirstOrDefault(b => b.Id == CollId);
                return View("Items");

            }
            else
            {
                ModelState.AddModelError("Name", "Enter correct data!");
                return View("LogIn");
            }
        }

        [HttpPost]
        public IActionResult DeleteCollInfo(string login, int CollId)
        {
            if (IsUnBlock(login))
            {
                var testDBContext = new TestDBContext();
                Collection person = testDBContext.Collections.FirstOrDefault(b => b.Id == CollId);
                testDBContext.Collections.Remove(person);
                foreach (var a in testDBContext.Items)
                    if (a.CollectionId == CollId)
                    {
                        try
                        {
                            foreach (var b in testDBContext.Comments)////////
                                if (b.ItemId == a.Id)
                                    testDBContext.Comments.Remove(b);
                        }
                        catch { }
                        testDBContext.Items.Remove(a);
                    }
                
                testDBContext.SaveChanges();

                return Index(login);

            }
            else
            {
                ModelState.AddModelError("Name", "Enter correct data!");
                return View("LogIn");
            }
        } /////////////////////////////

        [HttpPost]
        public IActionResult AddMessage(string CommentName, string CommentText, int ItemId, string login)
        {
            if (IsUnBlock(login))
            {
                var testDBContext = new TestDBContext();
                if (CommentText != null)
                {
                    Comment com = new Comment();
                    com.Name = CommentName;
                    com.Text = CommentText;
                    com.ItemId = ItemId;
                    try
                    {
                        int x = 1;
                        while (testDBContext.Comments.FirstOrDefault(a => a.Id == x) != null)
                            x++;
                        com.Id = x;
                    }
                    catch
                    {
                        com.Id = 1;
                    }
                    testDBContext.Comments.Add(com);
                    testDBContext.SaveChanges();
                }
                List<Comment> Comments = new List<Comment>();
                try
                {
                    foreach (var col in testDBContext.Comments)
                        if (col.ItemId == ItemId)
                            Comments.Add(col);
                }
                catch
                {
                    Comments = null;
                }
                Data.User person = testDBContext.Users.FirstOrDefault(b => b.Login == login);
                ViewData["UserName"] = person.Name;
                ViewData["UserLogin"] = person.Login;
                ViewData["UserStatus"] = person.Status;
                var i = testDBContext.Items.FirstOrDefault(b => b.Id == ItemId);
                ViewData["Item"] = i;
                ViewData["Coll"] = testDBContext.Collections.FirstOrDefault(b => b.Id == i.CollectionId);
                ViewData["Comments"] = Comments;
                return View("ItemInfo");

            }
            else
            {
                ModelState.AddModelError("Name", "Enter correct data!");
                return View("LogIn");
            }
        }
    }
}
