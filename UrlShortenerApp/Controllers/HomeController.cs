using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UrlShortenerApp.Models;
using UrlShortenerDAL.Models;
using UrlShortenerDAL.Repos;

namespace UrlShortenerApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly static Random _random = new Random();
        private readonly ILinkRepo _linkRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILinkRepo repo, UserManager<IdentityUser> userManager)
        {
            _linkRepo = repo;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index(string url)
        {
            if (url == null || url.Length != 6)
            {
                return RedirectToAction(nameof(Create));
            }

            LinkModel link = _linkRepo.GetLinkByUrl(url);
            if (link == null)
            {
                return View("NotFound");
            }

            // Inc Visitors
            link.Visitors++;
            _linkRepo.Update(link);

            return Redirect(link.Original);
        }

        // GET: Home/List
        public async Task<IActionResult> List()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            List<LinkModel> links = _linkRepo.GetAllForUser(user.Id);

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            foreach (LinkModel link in links)
            {
                link.Url = baseUrl + link.Url;
            }

            return View(links);
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LinkModel model = _linkRepo.GetOne(id);
            if (model == null)
            {
                return NotFound();
            }

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            model.Url = baseUrl + model.Url;
            return View(model);
        }

        // GET: Home/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Original")] LinkModel model)
        {
            if (ModelState.IsValid)
            {
                // User is logged in.
                IdentityUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return View(model);
                }

                // Check if user already created this url.
                LinkModel link = _linkRepo.GetLinkByOriginalForUser(model.Original, user.Id);
                if (link != null)
                {
                    ModelState.AddModelError(String.Empty, "You have already created a Short link for this url.");
                    return View(model);
                }

                model.OwnerId = user.Id;

                _linkRepo.Add(_random, model);
                return RedirectToAction(nameof(Details), new { id = model.ID });
            }
            return View();
        }


        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LinkModel model = _linkRepo.GetOne(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            LinkModel model = _linkRepo.GetOne(id);
            _linkRepo.Delete(model);

            return RedirectToAction(nameof(List));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
