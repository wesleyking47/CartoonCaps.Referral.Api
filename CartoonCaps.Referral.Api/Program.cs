using CartoonCaps.Referral.Application.Services;
using CartoonCaps.Referral.Domain.Infra.Interfaces;
using CartoonCaps.Referral.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Servers.Clear();
        document.Servers.Add(new OpenApiServer { Url = "http://localhost:8080" });

        return Task.CompletedTask;
    });
});

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
builder.Services.AddTransient<IReferralRepository, ReferralRepository>();
builder.Services.AddDbContext<ReferralContext>();

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