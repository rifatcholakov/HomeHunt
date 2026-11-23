using HomeHunt.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HomeHunt.TagHelpers
{
    [HtmlTargetElement(Attributes = CanModifyPropertyAttributeName)]
    public class CanModifyPropertyTagHelper : TagHelper
    {
        private const string CanModifyPropertyAttributeName = "asp-can-modify-property";

        private readonly IAuthorizationService _authorizationService;

        public CanModifyPropertyTagHelper(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HtmlAttributeName(CanModifyPropertyAttributeName)]
        public string OwnerUserId { get; set; } = string.Empty;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(
                ViewContext.HttpContext.User, OwnerUserId, Policies.CanModifyProperty);

            if (!result.Succeeded)
            {
                output.SuppressOutput();
            }
        }
    }
}
