using EmployeeManagement.Model;
using EmployeeManagement.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminstrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;

        public AdminstrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }



        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role With ID {id} Can't Be Found";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName,
                City = user.City,
                Claims = userClaims.Select(x => x.Type + ":" + x.Value).ToList(),
                Roles = userRoles

            };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role With ID {model.Id} Can't Be Found";
                return View("NotFound");
            }
            else
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.City = model.City;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role With ID {id} Can't Be Found";
                return View("NotFound");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers", "Account");

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("ListUsers");
        }


        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var identityRole = new IdentityRole { Name = model.RoleName };
                var Result = await _roleManager.CreateAsync(identityRole);
                if (Result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Adminstration");
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public IActionResult ListRoles()
        {
            var Roles = _roleManager.Roles;
            return View(Roles);
        }

        [HttpGet]

        public async Task<IActionResult> EditRole(string id)
        {
            var Role = await _roleManager.FindByIdAsync(id);
            if (Role == null)
            {
                ViewBag.ErrorMessage = $"Role With ID {id} Can't Be Found";
                return View("NotFound");
            }
            var model = new EditRoleViewModel
            {
                ID = Role.Id,
                RoleName = Role.Name,

            };
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, Role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.ID);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.ID} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var userRoleViewMode = new UserRoleViewModel
                {
                    UserName = user.UserName,
                    UserID = user.Id
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewMode.IsSelected = true;
                }
                else
                {
                    userRoleViewMode.IsSelected = false;
                }
                model.Add(userRoleViewMode);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserID);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);

                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {

                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);

                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count) - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            try
            {

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListRoles");
            }
            catch (DbUpdateException ex)
            {
                ViewBag.ErrorTitle = $"Role {role.Name} is in use";
                ViewBag.ErrorMessage = $"Can't Delete {role.Name} bacause it has many users, if you want to delete it, first delete all the users that are attatched with it";
                return View("Error");
            }


        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                ViewBag.ErrorMessage = $"User With ID {userId} can't be found";
                return View("NotFound");
            }
            var model = new List<UserInRoleViewModel>();
            UserInRoleViewModel userInRoleViewModel;
            foreach (var role in _roleManager.Roles)
            {
                userInRoleViewModel = new UserInRoleViewModel
                {
                    RoleName = role.Name,
                    RoleId = role.Id
                };
                model.Add(userInRoleViewModel);
            }
            foreach (var item in model)
            {

                if (await _userManager.IsInRoleAsync(user, item.RoleName))
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;

                }

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserInRoleViewModel> model, string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user is null)
            {
                ViewBag.ErrorMessage = $"User With ID {userID} can't be found";
                return View("NotFound");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can't Remove User Existing Roles");
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can't Add Selected Roles");
            }

            return RedirectToAction("EditUser", new { id = userID });

        }
        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]

        public async Task<IActionResult> ManageUserClaims(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user is null)
            {
                ViewBag.ErrorMessage = $"User With ID {userID} can't be found";
                return View("NotFound");
            }
            var existingUserClaims = await _userManager.GetClaimsAsync(user);
            var model = new UserClaimsViewModel
            {
                userId = userID
            };
            foreach (var claim in ClaimsStroe.AllClaims)
            {
                var usserClaims = new UserClaims
                {
                    ClaimType = claim.Type
                };

                if(existingUserClaims.Any(x=> x.Type == claim.Type && x.Value=="true"))
                {
                    usserClaims.IsSelected = true;
                }
                else
                {
                    usserClaims.IsSelected = false;
                }
                model.Claims.Add(usserClaims);
            }
            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]

        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.userId);
            if (user is null)
            {
                ViewBag.ErrorMessage = $"User With ID {model.userId} can't be found";
                return View("NotFound");
            }
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can't Remove User Existing claims");
            }
            result = await _userManager.AddClaimsAsync(user,
                model.Claims.Select(y => new Claim(y.ClaimType,y.IsSelected?"true":"false")));
           
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can't Add Selected Claims");
            }

            return RedirectToAction("EditUser", new { id = model.userId });

        }
    }
}
