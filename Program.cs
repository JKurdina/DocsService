using DocsService.Data;
using DocsService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

// ���������� �������� � ���������
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


//�� � ��������������
//var people = new List<Users>
//{
//    new Users("test1", "123456"),
//    new Users("test2", "567")

//}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // ��������� ������� � ����� �������
              .AllowAnyMethod()  // ��������� ��� HTTP-������ (GET, POST � �.�.)
              .AllowAnyHeader()  // ��������� ��� ���������
              .WithExposedHeaders("*");  // ��������� ��� ��������� ��������� � ������
    });
});

// �������� ������ ����������� �� ����� ������������
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));

// ��������� Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// ��������� ��� ��������������
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

//��������� �������������� � ������
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken"; // ��� ��������� ��� CSRF-������
    options.Cookie.Name = "CSRF-TOKEN"; // ��� ����
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // ������ HTTPS
    options.FormFieldName = "__RequestVerificationToken"; // ��� ���� � �����
});

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();


//app.UseCors("AllowAll");
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "Views/form/")),
//    RequestPath = "/form"
//});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "Views/authorization/")),
//    RequestPath = "/authorization"
//});

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/Account/Login"));
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();