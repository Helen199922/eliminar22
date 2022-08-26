using CarniceriaFinal.Autenticacion.DTOs;
using CarniceriaFinal.Autenticacion.Services.IServices;
using CarniceriaFinal.Core.JWTOKEN.DTOs;
using CarniceriaFinal.Core.JWTOKEN.Repository.IRepository;
using CarniceriaFinal.Core.JWTOKEN.Services.IServices;
using CarniceriaFinal.ModelsEF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.Security
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private int? idUserDetail;
        public IConfiguration Configuration { get; }
        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            Configuration = configuration;
        }

        public async Task Invoke(HttpContext context, 
            IJwtUtils JwtUtils, IJwtService JwtService, 
            ILogsServices iLogsServices, ILogsRepository ILogsRepository, 
            DBContext _Context)
        {
            this.idUserDetail = null;
            LogsEntity log = new LogsEntity();

            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }

            log.hostname = remoteIp.ToString();
            log.timestamp = DateTime.Now;

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var httpRequestFeature = context.Features[typeof(IHttpRequestFeature)] as IHttpRequestFeature;
            context.Request.EnableBuffering();
            var bodyAsText = await new System.IO.StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            Microsoft.AspNetCore.Http.Endpoint endpointBeingHit = context.Features.Get<IEndpointFeature>()?.Endpoint;
            ControllerActionDescriptor actionDescriptor = endpointBeingHit?.Metadata?.GetMetadata<ControllerActionDescriptor>();

            //log.

            var endPoint = actionDescriptor?.ActionName;
            string method = httpRequestFeature.Method;

            log.metodo = method;
            log.endpoint = endPoint;


            var logResponse = iLogsServices.mapperValues(log, actionDescriptor.ControllerName);


            //mensaje
            //estadoHTTP


            Boolean isPublic = await JwtService.IsPublicEndPoint(endPoint, method);
            if (isPublic)
            {
                using (var swapStream = new MemoryStream())
                {
                    var originalResponseBody = context.Response.Body;

                    context.Response.Body = swapStream;

                    context.Items["isPublic"] = true;
                    await _next(context);
                    

                    var statusCode = context.Response.StatusCode;

                    swapStream.Seek(0, SeekOrigin.Begin);
                    string responseBody = new StreamReader(swapStream).ReadToEnd();
                    swapStream.Seek(0, SeekOrigin.Begin);

                    await swapStream.CopyToAsync(originalResponseBody);

                    //try
                    //{
                    //    logResponse.mensaje = iLogsServices.getMessage(responseBody);
                    //    logResponse.estadoHTTP = statusCode ;
                    //    logResponse.idUser = null;

                    //    await ILogsRepository.SaveLogs(logResponse);
                    //}
                    //catch (Exception)
                    //{
                    //}
                    context.Response.Body = originalResponseBody;
                }
                
                return;
            }
            
            if (token != null)
            {
                await attachUserToContext(context, token, JwtUtils, endPoint, method, JwtService);

            }

            using (var swapStream = new MemoryStream())
            {
                var originalResponseBody = context.Response.Body;

                context.Response.Body = swapStream;

                await _next(context);

                var statusCode = context.Response.StatusCode;
                swapStream.Seek(0, SeekOrigin.Begin);
                string responseBody = new StreamReader(swapStream).ReadToEnd();
                swapStream.Seek(0, SeekOrigin.Begin);

                await swapStream.CopyToAsync(originalResponseBody);

                //try
                //{
                //    logResponse.mensaje = iLogsServices.getMessage(responseBody);
                //    logResponse.estadoHTTP = statusCode;
                //    logResponse.idUser = this.idUserDetail;

                //    await ILogsRepository.SaveLogs(logResponse);
                //}
                //catch (Exception)
                //{
                //}
                
                context.Response.Body = originalResponseBody;
            }
        }




        private async Task attachUserToContext(HttpContext context, string token, 
            IJwtUtils _JwtUtils, string endPoint, string method, IJwtService JwtService)
        {
            try
            {
                var userId = _JwtUtils.ValidateToken(token);
                this.idUserDetail = userId.Value;
                Boolean isOnlyForUser = await JwtService.IsOnlyForUSer(endPoint, method);
                if (isOnlyForUser)
                {
                    var idUserRequest = context.GetRouteData().Values["idUser"];
                    if(int.Parse((string)(idUserRequest ?? "0")) != userId)
                        return;
                }
                var user = await JwtService.GetUserById(userId.Value);

                var auth = await JwtService.FindOptionByIdRolAndMethodAndEndPoint(user.idRol.Value, endPoint, method);

                if (!auth) return;

                

                UserTokenEntity userEntity = new()
                {
                    idRol = user.idRol.Value,
                    idUser = userId.Value
                };
                // attach user to context on successful jwt validation
                context.Items["User"] = userEntity;
                
            }
            catch(Exception err)
            {
            }
            return;
        }
    }
}
