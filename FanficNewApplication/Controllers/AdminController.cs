using FanficNewApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FanficNewApplication.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(ILogger<HomeController> logger, IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _config = config;
            _userManager = userManager;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public IActionResult Index()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> AssignAdmin()
        {
            var model = new List<AssignAdminViewModel>();
            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new AssignAdminViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email
                };

                if (await _userManager.IsInRoleAsync(user, "Admin")) userRoleViewModel.IsSelected = true; 
                else userRoleViewModel.IsSelected = false; 

                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignAdmin(List<AssignAdminViewModel> model)
        {
            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, "Admin")))
                {
                    await _userManager.RemoveFromRoleAsync(user, "User");
                    result = await _userManager.AddToRoleAsync(user, "Admin");
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
                    result = await _userManager.AddToRoleAsync(user, "User");
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("AssignAdmin");
                }
            }
            return RedirectToAction("AssignAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBlockUnblockUsers(string IdList, string action)
        {
            string[] idList = IdList.Split(',');
            if (action == "Delete")
            {
                await DeleteUsers(idList);
            }
            else if (action == "Block")
            {
                await BlockeUsers(idList);
            }
            else if (action == "Unblock")
            {
                await UnblockUsers(idList);
            }
            return Redirect("~/Admin/Index");
        }

        public async Task<ActionResult> DeleteUsers(string[] idList)
        {
            foreach (string id in idList)
            {
                var user = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                    throw new Exception();
            }
            return Redirect("~/Admin/Index");
        }

        public async Task<ActionResult> BlockeUsers(string[] idList)
        {
            foreach (string id in idList)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return NotFound();

                user.LockoutEnd = DateTime.Now.AddYears(1000);
                await _userManager.UpdateAsync(user);
            }
            return Redirect("~/Admin/Index");
        }

        public async Task<ActionResult> UnblockUsers(string[] idList)
        {
            foreach (string id in idList)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return NotFound();

                user.LockoutEnd = DateTime.Now.Subtract(new TimeSpan(0, 0, 0, 1));
                await _userManager.UpdateAsync(user);
            }
            return Redirect("~/Admin/Index");
        }
    }
}
