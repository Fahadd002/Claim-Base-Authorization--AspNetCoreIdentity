using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AspNetCoreIdentity_Fahad.Models;
using System.Security.Claims;

namespace AspNetCoreIdentity_Fahad.Controllers
{
     
    public class AdministratorController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdministratorController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

       
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool roleExists = await _roleManager.RoleExistsAsync(model.RoleName);

                if (roleExists)
                {
                    ModelState.AddModelError(string.Empty, "Role Already Exists");
                }
                else
                {
                    ApplicationRole role = new ApplicationRole { Name = model.RoleName , Description=model.Description };

                    IdentityResult result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return View(model);
                    }
                }
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
       
        public async Task<IActionResult> ListRoles()
        {
            List<ApplicationRole> listRoles = await _roleManager.Roles.ToListAsync();
            return View(listRoles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return View("Error");
            }
            var model = new EditRoleViewModel { RoleId = role.Id, RoleName = role.Name ,Description=role.Description};
           
            model.Users = new List<string>();
            model.Claims = new List<string>();

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            model.Claims = roleClaims.Select(x => x.Value).ToList();

            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }
        [HttpPost]
        [Authorize(policy: "EditRolePolicy")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                if (role == null)
                {
                    ViewBag.ErrorMesg = $"Role with ID : {model.RoleId} can not be found";
                    return View("Error");
                }
                else
                {
                    role.Name = model.RoleName;
                    role.Description = model.Description;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View(model);
                }
            }
            return View(model);
        }

        //[HttpPost]

        [Authorize(policy: "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMesg = $"Role with ID : {roleId} can not be found";
                return View("Error");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("ListRoles",await _roleManager.Roles.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId= roleId;
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMsg = $"ROle with ID : {roleId} can not be found";
                return View("Error");
            }

            ViewBag.RollName  = role.Name;
            
            var model = new List<UserRoleModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userroleModel = new UserRoleModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userroleModel.IsSelected = true;
                }
                else { userroleModel.IsSelected = false; }

                model.Add(userroleModel);
            }
           return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleModel> model,string roleId)
        {
            
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMsg = $"ROle with ID : {roleId} can not be found";
                return View("Error");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result;
                if (model[i].IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if(!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result =await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else { continue; }

                if (result.Succeeded)
                {
                    if (i < (model.Count) - 1)
                    {
                        continue;
                    }
                    else return RedirectToAction("EditRole", new { roleId = roleId });
                }
            }

            return RedirectToAction("EditRole", new { roleId = roleId });
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMsg = $"User with ID :{userId} can not found";
                return View("Error");
            }
            var claimsList = await _userManager.GetClaimsAsync(user);
            var roleList = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Claims = claimsList.Select(c => c.Value).ToList(),
                Roles = roleList.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMsg = $"User with ID :{model.Id} can not found";
                return View("Error");
            }
            else { 
                 user.Email = model.Email;
                user.UserName=model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");

                }
                else {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
             
            return View(model);
        }

       // [HttpPost]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                ViewBag.ErrorMsg = $"User with ID :{UserId} can not found";
                return View("Error");
            }
            else { 
            var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View("ListUsers");
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string UserId)
        {
            var user =await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                ViewBag.ErrorMsg = $"User with ID :{UserId} can not found";
                return View("Error");
            }

            ViewBag.UserName = user.UserName;

            var model = new UserClaimViewModel { UserId = UserId };

            var ExistingUserClaims = await _userManager.GetClaimsAsync(user);

            foreach (Claim claims in ClaimStore.GetAllClaims())
            {
                UserClaim claim = new UserClaim() { ClaimType = claims.Type };

                if (ExistingUserClaims.Any(x => x.Type == claims.Type))
                {
                    claim.IsSeleted= true;
                }

                model.Claims.Add(claim);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimViewModel model)
        {
            var user = await _userManager.FindByIdAsync (model.UserId);
            if (user == null)
            {
                ViewBag.ErrorMsg = $"User with ID :{model.UserId} can not found";
                return View("Error");
            }
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user,claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can not remove Claims from User");
                return View(model);
            }

            var selectedClaims = model.Claims.Where(x => x.IsSeleted)
                .Select(c => new Claim(c.ClaimType, c.ClaimType))
                .ToList();

            if (selectedClaims.Any())
            {
                result = await _userManager.AddClaimsAsync(user, selectedClaims);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Can not remove Claims from User");
                    return View(model);
                }

            }

            return RedirectToAction("EditUser", new { userId = model.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageRoleClaims(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMsg = $"User with ID :{roleId} can not found";
                return View("Error");
            }

            ViewBag.RoleName = role.Name;

            var model = new RoleClaimViewModel { RoleId = roleId };

            var ExistingroleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (Claim claims in ClaimStore.GetAllClaims())
            {
                RoleClaim claim = new RoleClaim() { ClaimType = claims.Type };

                if (ExistingroleClaims.Any(x => x.Type == claims.Type))
                {
                    claim.IsSelected = true;
                }

                model.claims.Add(claim);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRoleClaims(RoleClaimViewModel model) 
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                ViewBag.ErrorMsg = $"User with ID :{model.RoleId} can not found";
                return View("Error");
            }
            var claims = await _roleManager.GetClaimsAsync(role);

            for (int i = 0; i < model.claims.Count; i++)
            {
                Claim claim = new Claim(model.claims[i].ClaimType, model.claims[i].ClaimType);
                IdentityResult? result;
                if (model.claims[i].IsSelected && !(claims.Any(c => c.Type == claim.Type)))
                { 
                    result = await _roleManager.AddClaimAsync(role, claim);
                }
                else if (!model.claims[i].IsSelected && claims.Any(c => c.Type == claim.Type))
                { 
                    result = await _roleManager.RemoveClaimAsync(role, claim);
                }
                else
                { 
                    continue;
                }
                
                if (result.Succeeded)
                {
                    if (i < (model.claims.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { roleId = model.RoleId });
                }
                else
                {
                    ModelState.AddModelError("", "Cannot add or removed selected claims to role");
                    return View(model);
                }
            }
            return RedirectToAction("EditRole", new { roleId = model.RoleId });
        }
    }
}
