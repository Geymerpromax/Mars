using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<SessionService>();

builder.Services.AddDistributedMemoryCache(); // Используется для хранения данных сессии
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сессии
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Обязательно для работы с GDPR
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapGet("/", () => Results.Redirect("/main"));

app.Run();


// SessionService.cs


public class SessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetUser(int userId, string userName)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session != null)
        {
            session.SetInt32("UserId", userId);
            session.SetString("UserName", userName);
        }
    }

    public int GetUserId()
    {
        return _httpContextAccessor.HttpContext?.Session.GetInt32("UserId") ?? -1;
    }

    public string GetUserName()
    {
        return _httpContextAccessor.HttpContext?.Session.GetString("UserName") ?? string.Empty;
    }

    public void ClearSession()
    {
        _httpContextAccessor.HttpContext?.Session.Clear();
    }
}
