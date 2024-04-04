using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebShop.Permission;
using WebShopCore;
using WebShopCore.Interfaces;
using WebShopCore.Repositories;
using WebShopCore.Services.FileUploadService;
using WebShopCore.Services.MailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

IronPdf.License.LicenseKey = builder.Configuration["IronPdf:LicenseKey"];


builder.Services.AddScoped<LoginRequired>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// cấu hình cho việc xử lý form
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;          // đặt giá trị ko giới hạn
    x.MultipartBodyLengthLimit = int.MaxValue;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // lưu cookie phụ thuộc vào chính sách của trình duyệt
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
});

// cloudinary
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("Cloudinary"));

builder.Services.AddScoped<IFileUploadService, CloudinaryUploadService>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<ISendMailService, SendMailService>();



string connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<WebShopDbContext>(options => options
    .UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        options => options.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: System.TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    )
);





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
