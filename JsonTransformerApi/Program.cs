using AutoMapper;
using JsonTransformerApi;
using JsonTransformerApi.DataAccess;
using JsonTransformerApi.Mappings;
using JsonTransformerApi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<MongoDatabaseOptions>(builder.Configuration.GetSection("Mongo"));

builder.Services.AddSingleton<IPeopleManager, PeopleManager>();
builder.Services.AddSingleton<IPersonHierarchyBuilder, PersonHierarchyBuilder>();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddSingleton<IPeopleRepository, PeopleRepository>();

builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(builder.Configuration["Mongo:ConnectionString"]));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new PersonProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = JwtBearerDefaults.AuthenticationScheme
                              }
                          },
                         new string[] {}
                    }
                });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateLifetime = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
