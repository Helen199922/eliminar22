using CarniceriaFinal.Autenticacion.DTOs;
using CarniceriaFinal.Autenticacion.Services.IServices;
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
        public IConfiguration Configuration { get; }
        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            Configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IJwtUtils JwtUtils, IJwtService JwtService, DBContext _Context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var httpRequestFeature = context.Features[typeof(IHttpRequestFeature)] as IHttpRequestFeature;

            Microsoft.AspNetCore.Http.Endpoint endpointBeingHit = context.Features.Get<IEndpointFeature>()?.Endpoint;
            ControllerActionDescriptor actionDescriptor = endpointBeingHit?.Metadata?.GetMetadata<ControllerActionDescriptor>();

            var endPoint = actionDescriptor?.ActionName;
            string method = httpRequestFeature.Method;

            Boolean isPublic = await JwtService.IsPublicEndPoint(endPoint, method);
            if (isPublic)
            {
                context.Items["isPublic"] = true;
                await _next(context);
                return;
            }
            
            if (token != null)
            {
                await attachUserToContext(context, token, JwtUtils, endPoint, method, JwtService);

            }
            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, string token, 
            IJwtUtils _JwtUtils, string endPoint, string method, IJwtService JwtService)
        {
            try
            {
                var userId = _JwtUtils.ValidateToken(token);
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
