using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;

namespace RiskDashBoard.AccessRights
{
    public class SessionValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(context.HttpContext.Session.GetString(SessionVariables.SessionEnum.SessionKeyUserName.ToString()))){
                context.Result = new RedirectResult("~/Access/Login");
            }

            base.OnActionExecuting(context);
        }
    }
}
