using Market.API.Swagger;
using Market.Application;
using Market.Application.Filters;
using Market.Application.Middlewares;
using Market.Core;
using Market.Infrastructure;
using Market.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//config appsettings
var config = new AppSettingsConfiguration();
builder.Configuration.Bind(config);
//services.AddSingleton(config);

//call Dependency Injection Methods from other layers
builder.Services.AddAuthInfrastructureDependencyInjection(config);

builder.Services.AddAuthApplicationDependencyInjection(config);


//services.AddControllers();
builder.Services.AddControllers(options =>
{
    //add filter by instance
    options.Filters.Add(new InputValidationActionFilter());
    //add filter By the type 
    options.Filters.Add(typeof(InputValidationActionFilter));
});

//disable default model validation, because we handle this in InputValidationActionFilter and LoggingMiddleware.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = config.JWT.Audience,
        ValidIssuer = config.JWT.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JWT.SecretKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/notify")))
            {
                            // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to apply global headers for all requests
    swagger.OperationFilter<HeaderOperationFilter>();

    //This is to export enums to front
    swagger.SchemaFilter<EnumSchemaFilter>();

    //This is to generate the Default UI of Swagger Documentation  
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Market.API",
        Description = "Market.API"
    });

    swagger.CustomOperationIds(
            d => (d.ActionDescriptor as ControllerActionDescriptor)?.ControllerName + (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName
        );

    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});


var app = builder.Build();
app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials
var culture = CultureInfo.CreateSpecificCulture("ar-EG");



var supportedCultures = new[]
{
    culture
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
if (config.EnvironmentSettings.EnableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Market.API v1"));
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// global error handler
app.UseMiddleware<LoggingMiddleware>();
app.MapControllers();

using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    scope.ServiceProvider.GetRequiredService<MarketContext>().Database.Migrate();
}
app.Run();