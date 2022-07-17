using CarniceriaFinal.Core.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Azure.Storage.Blobs;
using CarniceriaFinal.Autenticacion.Repository;
using CarniceriaFinal.Autenticacion.Repository.IRepository;
using CarniceriaFinal.Autenticacion.Services;
using CarniceriaFinal.Autenticacion.Services.IServices;
using CarniceriaFinal.Cliente.Repository;
using CarniceriaFinal.Core;
using CarniceriaFinal.Core.BlobStorage.Services;
using CarniceriaFinal.Core.BlobStorage.Services.IServices;
using CarniceriaFinal.Core.Email.DTOs;
using CarniceriaFinal.Core.Email.Services;
using CarniceriaFinal.Core.Email.Services.IServices;
using CarniceriaFinal.Core.JWTOKEN.Repository;
using CarniceriaFinal.Core.JWTOKEN.Repository.IRepository;
using CarniceriaFinal.Core.JWTOKEN.Services;
using CarniceriaFinal.Core.JWTOKEN.Services.IServices;
using CarniceriaFinal.Core.SaleStateHosted;
using CarniceriaFinal.Core.SaleStateHosted.Interface;
using CarniceriaFinal.Core.Security;
using CarniceriaFinal.Marketing.Interfaces.IRepository;
using CarniceriaFinal.Marketing.Interfaces.IService;
using CarniceriaFinal.Marketing.Repository;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.Marketing.Services;
using CarniceriaFinal.Marketing.Services.IService;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos;
using CarniceriaFinal.Productos.IRepository;
using CarniceriaFinal.Productos.IServicios;
using CarniceriaFinal.Productos.Repository;
using CarniceriaFinal.Productos.Services;
using CarniceriaFinal.Productos.Servicios;
using CarniceriaFinal.Roles.Repository;
using CarniceriaFinal.Roles.Repository.IRepository;
using CarniceriaFinal.Roles.Services;
using CarniceriaFinal.Roles.Services.IServices;
using CarniceriaFinal.Sales.IRepository;
using CarniceriaFinal.Sales.IServices;
using CarniceriaFinal.Sales.Repository;
using CarniceriaFinal.Sales.Repository.IRepository;
using CarniceriaFinal.Sales.Services;
using CarniceriaFinal.Sales.Services.IServices;
using CarniceriaFinal.Security.IRepository;
using CarniceriaFinal.Security.Repository;
using CarniceriaFinal.Security.Services.IServices;
using CarniceriaFinal.User.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers()
    .AddJsonOptions(configure =>
    {
        configure.JsonSerializerOptions.AllowTrailingCommas = true;
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarniceriaFinalFinal", Version = "v1" });
});
var connetionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(
        options =>
        options.
        UseMySql(connetionString, ServerVersion.AutoDetect(connetionString))
    );

builder.Services.AddTransient<IHostedService, SaleStateManagement>();

var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new MappingProfile());
});


builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: "CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton(x => new BlobServiceClient(
    builder.Configuration.GetValue<string>("ConnectionStrings:AzureBlobStorageConnectionString"))
);
builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));


builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPromocionRepository, PromocionRepository>();
builder.Services.AddScoped<IUnidadMedidaRepository, UnidadMedidaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ISubCategoriaRepository, SubCategoriaRepository>();
builder.Services.AddScoped<ISubInCategoriaRepository, SubInCategoriaRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<ISaleManagementHelper, SaleManagementHelper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommunicationRepository, CommunicationRepository>();
builder.Services.AddScoped<ICommunicationService, CommunicationService>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IJWTRepository, JWTRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IMeasureUnitService, MeasureUnitService>();
builder.Services.AddScoped<ICitiesRepository, CitiesRepository>();
builder.Services.AddScoped<ICitiesServices, CitiesServices>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IBankInfoRepository, BankInfoRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("CorsPolicy");



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
