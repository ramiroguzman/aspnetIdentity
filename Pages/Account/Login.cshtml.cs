using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using aspnetIdentity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential{get;set;} = new Credential();
        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid){
                if(Credential.UserName == "admin" && Credential.Password == "password"){
                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, Credential.UserName),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(ClaimTypes.Email, "admin@website.com"),
                        new Claim("Department", "HR"),
                        new Claim(ClaimTypes.Role, "Manager"),
                        new Claim("EmploymentDate", DateTime.Now.AddMonths(-5).ToString())
                    };
                    var identity = new ClaimsIdentity(claims, AppDefaults.APP_COOKIE_SCHEME);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    var authproperties = new AuthenticationProperties {
                        IsPersistent = Credential.RememberMe,
                        //ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
                    };
                    await HttpContext.SignInAsync(AppDefaults.APP_COOKIE_SCHEME,principal, authproperties);
                    return RedirectToPage("/Index"); // or "Home
            
                }else{
                    ModelState.AddModelError("Credential.UserName", "Invalid User Name or Password");
                }
             
            }
               return Page();
        }
    }
    public class Credential{
        [Required]
        [Display(Description="User Name")]
        public string UserName{get;set;} = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password{get;set;} = string.Empty;

        [Display(Name="Remember Me")]
        public bool RememberMe { get; set; }
    }
}
