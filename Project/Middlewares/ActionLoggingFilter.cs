using Microsoft.AspNetCore.Mvc.Filters;

namespace Project.Middlewares
{
    public class ActionLoggingFilter : IActionFilter
    {
        private readonly ILogger<ActionLoggingFilter> _logger;
        public ActionLoggingFilter(ILogger<ActionLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            _logger.LogInformation("Executing action: {ActionName}", actionName);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            if (context.Exception != null)
            {
                _logger.LogError(context.Exception, "Action {ActionName} threw an exception", actionName);
            }
            else
            {
                _logger.LogInformation("Executed action: {ActionName}", actionName);
            }
        }

    }
}
