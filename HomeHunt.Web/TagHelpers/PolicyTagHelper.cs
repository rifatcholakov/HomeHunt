using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HomeHunt.TagHelpers
{
    [HtmlTargetElement(Attributes = PolicyAttributeName)]
    public class PolicyTagHelper : TagHelper
    {
        private const string PolicyAttributeName = "asp-policy";

        private readonly IAuthorizationService _authorizationService;

        public PolicyTagHelper(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HtmlAttributeName(PolicyAttributeName)]
        public string Policy { get; set; } = string.Empty;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(ViewContext.HttpContext.User, Policy);

            if (!result.Succeeded)
            {
                output.SuppressOutput();
            }
        }
    }
}
