using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalAssignment_AverieScheil.Models;
using PersonalAssignment_AverieScheil.Data;

namespace PersonalAssignment_AverieScheil.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserRoleViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList(),
                });
            }

            return View(userList);
        }

        [HttpGet]
        public async Task<IActionResult> EditRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return View("Error");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                AvailableRoles = _roleManager.Roles
                    .Select(role => role.Name!)
                    .ToList(),
                SelectedRoles = userRoles.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(EditRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return View("Error");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = model.SelectedRoles ?? new List<string>();

            var rolesToRemove = currentRoles
                .Where(role => !selectedRoles.Contains(role))
                .ToList();

            var rolesToAdd = selectedRoles
                .Where(role => !currentRoles.Contains(role))
                .ToList();

            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (!removeResult.Succeeded)
            {
                return View("Error");
            }

            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);

            if (!addResult.Succeeded)
            {
                return View("Error");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return View("Error");
            }

            var currentUserId = _userManager.GetUserId(User);

            if(user.Id == currentUserId)
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
            {
                var admins = await _userManager.GetUsersInRoleAsync("Admin");

                if (admins.Count <= 1)
                {
                    TempData["Error"] = "You cannot delete the last administrator";
                    return RedirectToAction(nameof(Index));
                }
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "The user could not be deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        public class UserRoleViewModel
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public List<string> Roles { get; set; }
        }
    }
}
