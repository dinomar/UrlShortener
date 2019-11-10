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

        public IActionResult Index()
        {
            return View();
        }

        // GET: Links
        public async Task<IActionResult> List()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(_linkRepo.GetAllForUser(user.Id));
        }

        // GET: Link/Details/5
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

            return View(model);
        }

        // POST: Link/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Original")] LinkModel model)
        {
            if (ModelState.IsValid)
            {

                IdentityUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return View(model);
                }

                model.OwnerId = user.Id;

                _linkRepo.Add(_random, model);
                return RedirectToAction(nameof(Details), new { id = model.ID });
            }
            return View(nameof(Index));
        }


        // GET: Link/Delete/5
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

        // POST: Link/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            LinkModel model = _linkRepo.GetOne(id);
            _linkRepo.Delete(model);

            return RedirectToAction(nameof(Index));
        }


        /* -------------------------------------------------------------------------------------------------- */

        //[Authorize]
        public async Task<IActionResult> PrivacyAsync()
        {

            IdentityUser user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            string uid = user.Id;
            //_userManager.GetUserId(context)
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
