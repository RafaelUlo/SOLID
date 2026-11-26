using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace IPL.SOLID.API.Web.CustomAttributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ResourceTypeAttribute(string resourceType)
    : Attribute, IActionConstraint
{
    public int Order => 0;

    public bool Accept(ActionConstraintContext context)
    {
        var httpContext = context.RouteContext.HttpContext;

        if (!httpContext.Items.TryGetValue(
                "ResourceType",
                out var value))
        {
            return false;
        }

        return value is string requestedResourceType &&
               string.Equals(
                   requestedResourceType,
                   resourceType,
                   StringComparison.OrdinalIgnoreCase);
    }
}