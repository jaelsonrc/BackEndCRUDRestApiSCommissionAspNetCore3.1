using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServiceCommission.Controllers
{
    public abstract class BaseController : Controller
    {
        protected Guid UserId { get; set; } = Guid.Empty;


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var strUserId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value.ToString();
            if (strUserId == null) return;

            UserId = Guid.Parse(strUserId);
        }
    }
}
