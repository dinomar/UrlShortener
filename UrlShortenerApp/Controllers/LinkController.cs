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
    public class LinkController : Controller
    {
        private readonly static Random _random = new Random();
        private readonly ILinkRepo _linkRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public LinkController(ILinkRepo repo, UserManager<IdentityUser> userManager)
        {
            _linkRepo = repo;
            _userManager = userManager;
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

        // GET: Link/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Link/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Link/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Url,Original")] LinkModel model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _linkRepo.Update(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_linkRepo.GetOne(id) != null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
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
    }
}
