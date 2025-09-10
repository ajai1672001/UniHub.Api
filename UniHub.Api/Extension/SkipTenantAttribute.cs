namespace UniHub.Api.Extension;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SkipTenantMiddlewareAttribute : Attribute
{
}