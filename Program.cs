using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BlogContextConnection") ?? throw new InvalidOperationException("Connection string 'BlogContextConnection' not found.");

builder.Services.AddDbContext<BlogContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BlogContext>();


//Ottiene o imposta un valore che determina se il filtro (configurato da [ApiController]
//che restituisce un BadRequestObjectResult valore quando ModelState non è valido viene eliminato. .
//Quindi qunado mettiamo SuppressModelStateInvalidFilter = true DOBBIAMO gestire manualmente if(!ModelState.IsValid) in tutte le action!
builder.Services.Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = true; });


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Contact}/{id?}");

app.MapRazorPages();

app.Run();
