using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.Web.Models.Accounts;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Runtime.Internal.Transform;

namespace WebAdvert.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _userPool;

        public AccountsController(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool userPool)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._userPool = userPool;
        }

        public UserManager<CognitoUser> UserManager { get; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup()
        {
            var model = new SignupModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            if(ModelState.IsValid)
            {
                var user = this._userPool.GetUser(model.Email);
                if(user.Status != null)
                {
                    ModelState.AddModelError("UserExists", "User with this email already exists");
                    return View(model);
                }
                else
                {
                    //user doesn't exist so create it
                    user.Attributes.Add(CognitoAttribute.Name.ToString(), model.Email);
                    var createdUser = await this._userManager.CreateAsync(user, model.Password);
                    if (createdUser.Succeeded)
                    {
                        return RedirectToAction("Confirm");
                    }
                }
            }
            return View();
        }

        public IActionResult Confirm(ConfirmModel model)
        {
            //var model = new ConfirmModel();
            return View(model);
        }

/*        public IActionResult Confirm()
        {
            var model = new ConfirmModel();
            return View(model);
        }
*/
        [HttpPost]
        [ActionName("Confirm")]
        public async Task<IActionResult> ConfirmPost(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await this._userManager.FindByEmailAsync(model.Email);//.ConfigureAwait(false);
                if (user == null)
                {
                    ModelState.AddModelError("UserExists", "User with this email not found");
                    return View(model);
                }
                else
                {

                    //var result = await _userManager.ConfirmEmailAsync(user, model.Code);
                    var result = await (_userManager as CognitoUserManager<CognitoUser>).ConfirmSignUpAsync(user, model.Code, true);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    {
                        foreach(var item in result.Errors)
                        {
                            ModelState.AddModelError(item.Code, item.Description);
                        }
                        return View(model);
                    }
                }
            }
            return View();
        }

        public IActionResult Login(LoginModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await this._signInManager.PasswordSignInAsync(model.Email,
                    model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                        ModelState.AddModelError("Login error", "Email and password no not match");
                }


            }
            return View("Login", model);
        }

    }
}
