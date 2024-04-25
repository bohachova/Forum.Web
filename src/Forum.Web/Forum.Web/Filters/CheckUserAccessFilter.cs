using Forum.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Forum.Web.Filters
{
    public class CheckUserAccessFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                if(user.IsBanned())
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
