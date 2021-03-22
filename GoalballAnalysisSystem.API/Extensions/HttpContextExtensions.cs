using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }
            return httpContext.User.Claims.Single(c => c.Type == "id").Value;
        }

        public static string GetUserRole(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }
            return httpContext.User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;
        }
    }
}
