using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VOD.Admin.TagHelpers
{
    [HtmlTargetElement("btn")]
    public class BtnTagHelper : AnchorTagHelper
    {
        public string Icon { get; set; } = string.Empty;

        const string btnPrimary = "btn-primary";
        const string btnDanger = "btn-danger";
        const string btnDefault = "btn-default";
        const string btnInfo = "btn-info";
        const string btnSuccess = "btn-success";
        const string btnWarning = "btn-warning";
        //Google's Materials Icons provider name
        const string iconProvider = "material-icons";

        public BtnTagHelper(IHtmlGenerator generator) : base(generator) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            output.TagName = "a";

            #region Bootstrap Button
            var aspPageAttribute = context.AllAttributes.SingleOrDefault(
                p => p.Name.ToLower().Equals("asp-page"));

            var classAttribute = context.AllAttributes.SingleOrDefault(
                p => p.Name.ToLower().Equals("class"));

            var buttonStyle = btnDefault;

            if (aspPageAttribute != null)
            {
                var pageValue = aspPageAttribute.Value.ToString().ToLower();

                switch (pageValue)
                {
                    case "create": buttonStyle = btnPrimary; break;
                    case "delete": buttonStyle = btnDanger; break;
                    case "edit": buttonStyle = btnSuccess; break;
                    case "index": buttonStyle = btnPrimary; break;
                    case "details": buttonStyle = btnInfo; break;
                    case "/index": buttonStyle = btnWarning; break;
                    case "error": buttonStyle = btnDanger; break;
                    default: buttonStyle = btnDefault; break;
                }
            }
            var bootstrapClasses = $"btn-sm {buttonStyle}";

            if (classAttribute != null)
            {
                var css = classAttribute.Value.ToString();
                if (!css.ToLower().Contains("btn-"))
                {
                    output.Attributes.Remove(classAttribute);
                    classAttribute = new TagHelperAttribute("class", $"{css} {bootstrapClasses}");
                    output.Attributes.Add(classAttribute);
                }
            }
            else
            {
                output.Attributes.Add("class", bootstrapClasses);
            }
            #endregion

            #region Icon
            if (!Icon.Equals(String.Empty))
            {
                var childContent = output.GetChildContentAsync().Result;
                var content = childContent.GetContent().Trim();
                if (content.Length > 0) content = $"&nbsp{content}";

                output.Content.SetHtmlContent($@"<i class='{iconProvider}'
                    style='display: inline-flex; vertical-align: top;
                    line-height: inherit;font-size: medium;'>{Icon}</i>
                    <span style='font-size: medium;'>{content}</span>");
            }
            #endregion

            #region StyleAttribute
            var style = context.AllAttributes.SingleOrDefault(
                s => s.Name.ToLower().Equals("style"));

            var styleValue = style?.Value ?? "";
            var newStyle = new TagHelperAttribute("style",
                $"{styleValue} display: inline-flex; text-decoration: none; border-radius: 0px;");

            if (style != null) output.Attributes.Remove(style);
            output.Attributes.Add(newStyle);
            #endregion

            base.Process(context, output);
        }
    }
}
