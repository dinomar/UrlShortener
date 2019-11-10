using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UrlShortenerDAL.EF;
using UrlShortenerDAL.Models;
using UrlShortenerDAL.Repos;

namespace UrlShortenerApp.Controllers
{
    public class LinksController : Controller
    {
        private readonly static Random _random = new Random();
        private readonly ILinkRepo _linkRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public LinksController(ILinkRepo repo, UserManager<IdentityUser> userManager)
        {
            _linkRepo = repo;
            _userManager = userManager;
        }

        

        

        // GET: Links/Create
        public IActionResult Create()
        {
            return RedirectToAction("Index", "Home");
        }

        
    }
}
