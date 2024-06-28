var builder = WebApplication.CreateBuilder(args);

// Add CORS policy to services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Adjust to match your Angular app's origin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
   
});

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

var app = builder.Build();

await app.BootUmbracoAsync();

// Apply the CORS middleware
app.UseCors("AllowSpecificOrigin");

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Music}/{action=GetHomeNodeName}/{id?}");

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();