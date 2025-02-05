using CartoonCaps.Referral.Api.Repositories;
using CartoonCaps.Referral.Api.Services;
using CartoonCaps.Referral.Api.Utilities;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi("v1");

builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddTransient<IReferralService, ReferralService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IReferralCodeGenerator, ReferralCodeGenerator>();
builder.Services.AddTransient<IReferralRepository, ReferralRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
