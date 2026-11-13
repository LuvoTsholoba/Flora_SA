using Microsoft.AspNetCore.Localization;
using System.Globalization;
using OpenAI.Chat;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Mscc.GenerativeAI.Types;
using Mscc.GenerativeAI;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//builder.Services.AddSingleton(new ChatClient(model: "gpt-4o", apiKey: apiKey);
builder.Services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization();
builder.Services.AddSingleton(sp =>
 {
 string apiKey = builder.Configuration["Gemini:ApiKey"] ??
              throw new InvalidOperationException("Gemini keys are missing !");

 var googleAI = new GoogleAI(apiKey);
 return googleAI.GenerativeModel(model: "gemini-3.8-flash");

});
var app = builder.Build();

//Defining all 12 official languages in South Africa


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
