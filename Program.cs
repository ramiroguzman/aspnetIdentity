using System.Security.Claims;
using aspnetIdentity;
using aspnetIdentity.Authorization;
using Microsoft.AspNetCore.Authorization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(AppDefaults.APP_COOKIE_SCHEME).AddCookie(AppDefaults.APP_COOKIE_SCHEME, options =>
{
    options.Cookie.Name = AppDefaults.APP_COOKIE_SCHEME;
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBelongToHR", policy => policy.RequireClaim("Department", "HR"));
    options.AddPolicy("Admin", policy => policy
        .RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("HRManager", policy => policy
        .RequireClaim("Department", "HR")
        .RequireClaim(ClaimTypes.Role, "Manager")
        .Requirements.Add(new HRManagerProbationRequirement(3)));    
});
builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
