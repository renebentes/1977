using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithoutIdentity.Models;
using WithoutIdentity.Models.ManagerViewModels;

namespace WithoutIdentity.Controllers
{
    public class ManagerController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManagerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                throw new ApplicationException($"Não foi possível carregar o usuário com o Id '{_userManager.GetUserId(User)}'");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await _signInManager.SignInAsync(user, false);

            StatusMessage = "Sua senha foi alterada com sucesso.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                throw new ApplicationException($"Não foi possível carregar o usuário com o Id '{_userManager.GetUserId(User)}'");
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                throw new ApplicationException($"Não foi possível carregar o usuário com o Id '{_userManager.GetUserId(User)}'");
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                throw new ApplicationException($"Não foi possível carregar o usuário com o Id '{_userManager.GetUserId(User)}'");
            }

            if (user.Email != model.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);

                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Não foi possível atribuir o e-mail ao usuário com o Id '{_userManager.GetUserId(User)}'");
                }
            }

            if (user.PhoneNumber != model.PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.Email);

                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Não foi possível atribuir o telefone ao usuário com o Id '{_userManager.GetUserId(User)}'");
                }
            }

            StatusMessage = "Seu perfil foi atualizado.";

            return RedirectToAction(nameof(Index));
        }
    }
}