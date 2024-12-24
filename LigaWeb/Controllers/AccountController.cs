using LigaWeb.Data;
using LigaWeb.Data.Entities;
using LigaWeb.Helpers;
using LigaWeb.Helpers.Impl;
using LigaWeb.Helpers.Interfaces;
using LigaWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LigaWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AccountController(
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IConfiguration configuration,
            DataContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            this._userHelper = userHelper;
            this._mailHelper = mailHelper;
            this._configuration = configuration;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }
                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login!");
            return View(model);
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.Firstname,
                        LastName = model.Lastname,
                        Email = model.Username,
                        UserName = model.Username
                    };

                    if (photo != null)
                    {
                        // Salvar a foto no diretório de uploads
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Certifique-se de que o diretório existe
                        Directory.CreateDirectory(uploadsFolder);

                        // Salvar o arquivo
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }

                        // Atualizar o caminho no modelo
                        user.PhotoPath = $"/uploads/{uniqueFileName}";
                    }

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn´t be created.");
                        return View(model);
                    }

                    // Associar o usuário à Role "Anonimous"
                    await _userHelper.AddUserToRoleAsync(user, "Anonimous");

                    // Gerar token de confirmação de email
                    var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, token },
                        protocol: HttpContext.Request.Scheme);

                    // Enviar email de confirmação
                    var emailBody = $"Please confirm your account by clicking <a href='{confirmationLink}'>here</a>.";
                    var emailResponse = _mailHelper.SendEmail(user.Email, "Confirm your email", emailBody);

                    if (!emailResponse.IsSuccess)
                    {
                        ModelState.AddModelError(string.Empty, "The confirmation email could not be sent.");
                        return View(model);
                    }

                    TempData["Message"] = "Registration successful! Please check your email to confirm your account.";
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "This email is already in use.");
            }

            return View(model);

            
        }


        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
            if (user != null)
            {
                model.Firstname = user.FirstName;
                model.Lastname = user.LastName;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    user.FirstName = model.Firstname;
                    user.LastName = model.Lastname;
                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return this.View(model);
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        
        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn´t correspond to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Shop Password Reset", $"<h1>Shop Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href=\"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                }
                return this.View();

            }
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return View();
                }
                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }
            this.ViewBag.Message = "User not found.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Error", "Home", new { message = "Invalid email confirmation request." });
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { message = "User not found." });
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmailSuccess");
            }

            return View("ConfirmEmailFailure");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            var clubs = await _context.Clubs.ToListAsync();
            var roles = await _userHelper.GetAllRolesAsync();

            var model = new AddUserViewModel
            {
                Clubs = clubs.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                Roles = roles.Select(r => new SelectListItem
                {
                    Value = r,
                    Text = r
                }).ToList()
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model, IFormFile? photo)
        {
            if (!ModelState.IsValid)
            {
                // Popula os Clubs e Roles novamente para retornar a View em caso de erro.
                var clubs = await _context.Clubs.ToListAsync();
                var roles = await _userHelper.GetAllRolesAsync();

                model.Clubs = clubs.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                model.Roles = roles.Select(r => new SelectListItem
                {
                    Value = r,
                    Text = r
                }).ToList();

                return View(model);
            }

            // Processa a foto se fornecida.
            if (photo != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                model.PhotoPath = $"/uploads/{uniqueFileName}";
            }

            // Cria o usuário.
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                ClubId = model.ClubId,
                PhotoPath = model.PhotoPath
            };

            var result = await _userHelper.AddUserAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            // Adiciona o usuário ao Role selecionado.
            if (!string.IsNullOrEmpty(model.RoleId))
            {
                var role = await _roleManager.FindByNameAsync(model.RoleId);
                if (role == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid role selected.");
                    return View(model);
                }

                await _userHelper.AddUserToRoleAsync(user, role.Name);
            }

            // Gera o token de confirmação de email.
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, token = emailToken },
                Request.Scheme
            );

            // Gera o token para redefinição de senha.
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordLink = Url.Action(
                "ResetPassword",
                "Account",
                new { token = resetToken, email = user.Email },
                Request.Scheme
            );

            // Corpo do email.
            var subject = "Account Confirmation and Password Reset";
            var body = $@"
                <h1>Welcome to Our Application</h1>
                <p>Please confirm your email by clicking the link below:</p>
                <a href='{confirmationLink}'>Confirm Email</a>
                <p>Additionally, you can set your password using the link below:</p>
                <a href='{resetPasswordLink}'>Reset Password</a>";

            // Envia o email.
            var emailResponse = _mailHelper.SendEmail(user.Email, subject, body);

            if (!emailResponse.IsSuccess)
            {
                TempData["Error"] = "Failed to send email. Please check your email configuration.";
            }

            TempData["Message"] = "User successfully created and confirmation email sent.";
            return RedirectToAction("Index", "Home");
        }


    }
}
