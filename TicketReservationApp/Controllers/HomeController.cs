using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TicketReservationApp.Models;
using TicketReservation.App.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using TicketReservation.App.Models;
using TicketReservation.App.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TicketReservationDataAccess.Data.Dtos;

namespace TicketReservationApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AccountService accountService;
       
        public HomeController(ILogger<HomeController> logger, AccountService accountService)
        {
            this.accountService = accountService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
       

        public async Task<IActionResult> AdminPage()
        {
            return View();
        }
        public IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginViewModel loginViewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginViewModel.Username) || string.IsNullOrWhiteSpace(loginViewModel.Password))
                {
                    TempData["ErrorMessage"] = "Username and password are required";
                    return RedirectToAction("AdminLogin", "Home");
                }
                LoginViewModel result = await accountService.LoginAsync(loginViewModel);
                if (result.Password == null)
                {
                    TempData["ErrorMessage"] = "Invalid Credentials";
                    return RedirectToAction("AdminLogin", "Home");
                }
                int id = result.Id;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()), 
                };

                if (result.UserRole)
                {
               
                    TempData["SuccessMessage"] = "Please Login With UserLogin! You are Trying To Login with Admin's Login";
                    return RedirectToAction("UserLogin", "Home");
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                    var claimsIdentity = new ClaimsIdentity(claims, "AdminCookies"); 
                    await HttpContext.SignInAsync("AdminCookies", new ClaimsPrincipal(claimsIdentity));
                    TempData["SuccessMessage"] = "Successfully Login";
                    return RedirectToAction("Index", "Adminstrator");
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error Occured");
                return View("AdminLogin");

            }
        }

      
       
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        public async Task<IActionResult> AdminLogout()
        {
            await HttpContext.SignOutAsync("AdminCookies");
            return RedirectToAction("AdminPage","Home");
        }

        public IActionResult UserPage()
        {
            return View();
        }
        public IActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserLogin(LoginViewModel loginViewModel)
        {

            try
            {
                
                LoginViewModel result = await accountService.LoginAsync(loginViewModel);
                if (result.Password == null)
                {
                    TempData["ErrorMessage"] = "Invalid Credentials";
                    return RedirectToAction("UserLogin", "Home");
                }
                int id = result.Id;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()), 
                };

                if (result.UserRole)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                    var claimsIdentity = new ClaimsIdentity(claims, "UserCookies");
                    await HttpContext.SignInAsync("UserCookies", new ClaimsPrincipal(claimsIdentity));
                    TempData["SuccessMessage"] = "Successfully Login";
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    TempData["SuccessMessage"] = "Please Login With AdminLogin! You are Trying To Login with Users's Login";
                    return RedirectToAction("AdminLogin", "Home");
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred during login");
                return View("UserLogin");

            }
        }

        public IActionResult ForgetPasswordView(int id, string userName, string email, string phone)
        {
            try
            {
                ResetPasswordModel resetPassword;
                if (!string.IsNullOrEmpty(userName))
                {

                    resetPassword = new ResetPasswordModel
                    {
                        Id = id,
                        UserName = userName,
                        Email = email,
                        Phone = phone,

                        IsValidated = true
                    };
                }
                else
                {
                    resetPassword = new ResetPasswordModel();
                }
                return View(resetPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured");
                return RedirectToAction("ForgetPasswordView", new ResetPasswordModel());
            }
        }

        public async Task<IActionResult> VerifyAccount(ResetPasswordModel model)
        {
            try
            {
                UserModel user = await accountService.VerifyAccount(model);
                if (user.Id == 0 || user.Username == null)
                {
                    TempData["ErrorMessage"] = "Account Does not Exist!";
                }
                else
                {
                    TempData["SuccessMessage"] = "Account Verified!";

                    return RedirectToAction("ForgetPasswordView", new
                    {
                        Id = user.Id,
                        UserName = user.Username,
                        Email = model.Email,
                        Phone = model.Phone,
                    });
                }
                return RedirectToAction("ForgetPasswordView");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured");
                return RedirectToAction("ForgetPasswordView", new ResetPasswordModel());
            }
        }

        public async Task<IActionResult> ResetPassword(string UserName, string Password)
        {
            bool IsPasswordReset=await accountService.ResetPassword(UserName, Password);
            if(IsPasswordReset)
            {
                TempData["SuccessMessage"] = "Password Changed!,You May Login";
                 return View("UserLogin");
                
            }
            TempData["ErrorMessage"] = "Unable To change Password!";
            return View("ResetPasswordView");

        }

        public async Task<IActionResult> SignUpView()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel user)
        {
            try
            {
                ErrorMessageModel model = await accountService.SignUpAsync(user);
                if (model.Success)
                {
                    TempData["SuccessMessage"] = "Sucessful Registration, You may Login";
                    return RedirectToAction("UserLogin", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, model.ErrorMessage);
                    return View("SignUpView", user);
                }
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "An unexpected error occurred during signup.";
                return View("SignUpView", user);
            }
        }

       
        [Authorize(AuthenticationSchemes = "UserCookies")]
        public async Task<IActionResult> UserLogout()
        {
            await HttpContext.SignOutAsync("UserCookies");
            return RedirectToAction("UserPage", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}