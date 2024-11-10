using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PharmEtrade_ApiGateway.Extensions;
using PharmEtrade_ApiGateway.Repository.Interface;
using PharmEtrade_ApiGateway.Repository.Helper;
using BAL.BusinessLogic.Interface;
using BAL.BusinessLogic.Helper;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using BAL.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
var keyString = jwtSettings["Key"];

 if (string.IsNullOrEmpty(keyString))
 {
    throw new InvalidOperationException("JWT Key is not configured. Please ensure 'JwtSettings:Key' is set in appsettings.json.");
 }

var key = Encoding.UTF8.GetBytes(keyString);

// Add services to the container
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Configure JWT authentication
    //var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    //var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // lowercase
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
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
            new string[] { }
        }
    };
    c.AddSecurityRequirement(securityRequirement);
});

//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("VendorPolicy", policy => policy.RequireRole("Vendor"));
    options.AddPolicy("PharmacyPolicy", policy => policy.RequireRole("Pharmacy"));
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));
});
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<SmtpSettings>>().Value);
builder.Services.AddSingleton<JwtAuthenticationExtensions>();
builder.Services.AddSingleton<ICustomerRepo, CustomerRepository>();
builder.Services.AddTransient<ICustomerHelper, CustomerHelper>();
builder.Services.AddSingleton<IsqlDataHelper, SqlDataHelper>();
builder.Services.AddSingleton<IProductsRepo, ProductRepository>();
builder.Services.AddTransient<IProductHelper, ProductHelper>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<IProductFilter, ProductFilterHelper>();
builder.Services.AddSingleton<IProductFilterRepo, ProductFilterRepository>();
builder.Services.AddSingleton<IOrdersRepository, OrdersRepository>();
builder.Services.AddSingleton<IOrders, OrdersHelper>();
builder.Services.AddSingleton<IMenuRepository, MenuRepository>();
builder.Services.AddSingleton<IMenuHelper, MenuHelper>();
builder.Services.AddSingleton<ICartRepository, CartRepository>();
builder.Services.AddSingleton<ICartHelper, CartHelper>();
builder.Services.AddSingleton<IWishListHelper, WishListHelper>();
builder.Services.AddSingleton<IWishListRepository, WishListRepository>();
builder.Services.AddSingleton<IBannerRepository, BannerRepository>();
builder.Services.AddSingleton<IBannerHelper, BannerHelper>();
builder.Services.AddSingleton<IMastersRepository, MastersRepository>();
builder.Services.AddSingleton<IMastersHelper, MastersHelper>();
builder.Services.AddSingleton<IBidHealper, BidHealper>();
builder.Services.AddSingleton<IBidRepository, BidRepository>();
builder.Services.AddSingleton<IPaymentinfoRepository, PaymentInfoRepository>();
builder.Services.AddSingleton<IPaymentInfo, PaymentInfoHelper>();
builder.Services.AddSingleton<IDashboardHelper, DashboardHelper>();
builder.Services.AddSingleton<IDashboardRepository, DashboardRepository>();
builder.Services.AddSingleton<IEmailHelper, EmailHelper>();
builder.Services.AddSingleton<IAdminRepository, AdminRepository>();
builder.Services.AddSingleton<IAdminHelper, AdminHelper>();
builder.Services.AddSingleton<ITaxRepo, TaxRepository>();
builder.Services.AddSingleton<ITaxHelper, TaxHelper>();
builder.Services.AddSingleton<IJwtHelper, JwtHelper>();
builder.Services.AddSingleton<IFedExRepository, FedExRepository>();
builder.Services.AddSingleton<IFedExHelper, FedExHelper>();
builder.Services.AddSingleton<ISquareupHelper, SquareupHelper>();
builder.Services.AddSingleton<IReportsHelper, ReportsHelper>();
builder.Services.AddSingleton<IReportsRepository, ReportsRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
