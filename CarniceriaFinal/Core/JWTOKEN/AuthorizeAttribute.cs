
using CarniceriaFinal.Autenticacion.DTOs;
using CarniceriaFinal.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Items["isPublic"] != null) return;

        var user = (UserTokenEntity)context.HttpContext.Items["User"];
        if (user == null)
        {
            RSEntity<UserAuthResponseEntity> rsEntity = new();

            context.Result = new JsonResult(rsEntity.Fail("Unauthorized"))
                { StatusCode = StatusCodes.Status401Unauthorized 
            
            };
        }
    }
}