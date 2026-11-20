using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HomeHunt.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-policy")]
    [HtmlTargetElement(Attributes = "asp-not-policy")]
    public class PolicyTagHelper : TagHelper
    {
        private readonly IAuthorizationService _authorizationService;

        public PolicyTagHelper(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HtmlAttributeName("asp-policy")]
        public string Policy { get; set; } = string.Empty;

        [HtmlAttributeName("asp-not-policy")]
        public string NotPolicy { get; set; } = string.Empty;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            System.Security.Claims.ClaimsPrincipal user = ViewContext.HttpContext.User;

            if (!string.IsNullOrEmpty(Policy))
            {
                AuthorizationResult result = await _authorizationService.AuthorizeAsync(user, Policy);

                if (!result.Succeeded)
                {
                    output.SuppressOutput();
                    return;
                }
            }

            if (!string.IsNullOrEmpty(NotPolicy))
            {
                AuthorizationResult result = await _authorizationService.AuthorizeAsync(user, NotPolicy);

                if (result.Succeeded)
                {
                    output.SuppressOutput();
                }
            }
        }
    }
}
