using DocsService.Data;
using DocsService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // ��������� ������� � ����� �������
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.UseCors("AllowAll");
app.UseRouting();
app.UseCors("AllowAll");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Views/form/")),
    RequestPath = ""
});

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    db.Database.EnsureCreated(); // ������� �� � �������, ���� �� ���

//    if (!db.Employees.Any())
//    {
//        db.Employees.AddRange(
//            new Employees
//            {
//                ID = 1,
//                LastName = "������",
//                FirstName = "����",
//                MiddleName = "��������",
//                BirthDate = new DateTime(1985, 5, 15),
//                Position = "�������"
//            },
//            new Employees
//            {
//                ID = 2,
//                LastName = "������",
//                FirstName = "����",
//                MiddleName = "��������",
//                BirthDate = new DateTime(1990, 8, 22),
//                Position = "�������"
//            }
//        );
//        db.SaveChanges();
//    }
//}

app.Run();