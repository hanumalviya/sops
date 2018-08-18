using NHMembership.Models;
using SOPS.WebUI.Areas.Administration.ViewModels.Account;
using System;
using System.Linq;
using System.Web.Mvc;
using SOPS.WebUI.Resources;
using NHMembership.Services;
using SOPS.Services.Employees;
using SOPS.Services.Mail;

namespace SOPS.WebUI.Areas.Administration.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmployeesProvider _employeesProvider;
        private readonly IEmployeeUpdater _employeeUpdater;
        private readonly IMailService _mailService;        

        public AccountController(            
            IAuthenticationService authenticationService,
            IEmployeesProvider employeesProvider,
            IEmployeeUpdater employeeUpdater,
            IMailService mailService)
        {
            _authenticationService = authenticationService;
            _employeesProvider = employeesProvider;
            _employeeUpdater = employeeUpdater;
            _mailService = mailService;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                bool result = _authenticationService.Login(login.UserName, login.Password, login.Remember);

                if (result == true)
                {
                    return RedirectToAction("Index", "Students");
                }

                ModelState.AddModelError("SomethingGoesWrong", "Niepoprawna nazwa użytkownika lub hasło");
            }
            else
            {
                ModelState.AddModelError("SomethingGoesWrong", LocalizedStrings.AccountController_Login_SomethingGoesWrong);
            }

            return View(login);
        }

        public ActionResult Logout()
        {
            _authenticationService.Logout();

            return  RedirectToAction("Login");
        }

        public ActionResult Manage(int? userId)
        {
            int currentUserId = _authenticationService.MembershipService.GetUserProfileByName(User.Identity.Name).Id;
            ViewBag.IsCurrentUser = userId.HasValue == false || userId.Value == currentUserId;

            return View(userId ?? currentUserId);
        }

        public ActionResult UpdateProfile(int? userId)
        {
            UserProfile user = null;

            if (userId.HasValue)
                user = _authenticationService.MembershipService.GetUserProfileByKey(userId.Value);
            else
                user = _authenticationService.MembershipService.GetUserProfileByName(User.Identity.Name);

            var updateProfileViewModel = new UpdateProfileViewModel()
            {
                UserId = user.Id,
                Email = user.Membership.Email,
                IsLockedOut = user.Membership.IsLockedOut,
                UserName = user.UserName
            };

            return PartialView(updateProfileViewModel);
        }

        [HttpPost]
        public ActionResult UpdateProfile(UpdateProfileViewModel profile)
        {
            if (ModelState.IsValid)
            {
                var user = _authenticationService.MembershipService.GetUserProfileByKey(profile.UserId);
                var employee = _employeesProvider.GetEmployee(profile.UserId);
                employee.Email = profile.Email;

                _authenticationService.MembershipService.UpdateUser(profile.UserName, profile.Email, true);
                _employeeUpdater.Update(employee);

                if (profile.IsLockedOut == false && user.Membership.IsLockedOut == true)
                {
                    _authenticationService.MembershipService.UnLockUser(user.UserName, DateTime.Now);
                }
                else if (profile.IsLockedOut == true && user.Membership.IsLockedOut == false)
                {
                    _authenticationService.MembershipService.LockOutUser(user.UserName, DateTime.Now);
                }              
            }

            return RedirectToAction("Manage", new {userId = profile.UserId});
        }

        public ActionResult ChangePassword(int? userId)
        {
            UserProfile user = null;

            if (userId.HasValue)
                user = _authenticationService.MembershipService.GetUserProfileByKey(userId.Value);
            else
                user = _authenticationService.MembershipService.GetUserProfileByName(User.Identity.Name);

            var changePasswordViewModel = new ChangePasswordViewModel()
                {
                    UserId = user.Id,
                    CurrentPassword = "***********",
                    NewPassword = string.Empty,
                    PasswordAnswer = string.Empty,
                    PasswordAnswerConfirm = string.Empty,
                    PasswordConfirm = string.Empty,
                    PasswordQuestion = user.Membership.PasswordQuestion
                };

            return PartialView(changePasswordViewModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                _authenticationService.ChangePassword(model.UserId, model.CurrentPassword, model.NewPassword, model.PasswordQuestion, model.PasswordAnswer);
            }

            return RedirectToAction("Manage", new { userId = model.UserId });
        }

        public ActionResult CheckUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckUser(string userName)
        {
            if (_authenticationService.MembershipService.AllUsers().Any(n => n.UserName == userName))
            {
                int userId = _authenticationService.MembershipService.GetUserProfileByName(userName).Id;

                return RedirectToAction("ResetPassword", new { userId = userId });
            }

            ModelState.AddModelError("userNotExist", "Użytkownik nie istnieje");

            return View("CheckUser");
        }

        public ActionResult ResetPassword(int? userId)
        {
            if (userId.HasValue == false)
                return HttpNotFound();

            var user = _authenticationService.MembershipService.GetUserProfileByKey(userId.Value);

            var model = new ResetPasswordViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Membership.Email,
                PasswordQuestion = user.Membership.PasswordQuestion
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string generatedPassword = _authenticationService.ResetPassword(model.UserId, model.PasswordAnswer);
                    string meessage = string.Format("Nowe hasło systemu sops zostało wygenerowane. \n Nazwa użytkownika: {0} \n Hasło: {1}", model.UserName, generatedPassword);
                    _mailService.SendMail("SOPS - Nowe hasło zostało wygenerowane", meessage, model.Email);
                }
                catch
                {
                    ModelState.AddModelError("wrong answer", "Błędna odpowiedź");
                }
            }

            return View(model);
        }
    }
}
