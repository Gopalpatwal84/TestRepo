namespace PowerBI.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Acom.Mvc.Cdn;
    using Models.Pages;

    public static class CustomHtmlHelpers
    {
        public static void GenerateMetaTags(this HtmlHelper helper, ViewMetadataModel model)
        {
            if (model.SocialModel != null)
            {
                var url = model.SocialModel.Url;
                var currentUri = helper.ViewContext.HttpContext.Request.Url;

                if (string.IsNullOrWhiteSpace(url))
                {
                    if (currentUri != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.SocialModel.Domain))
                        {
                            // If the Domain is specified, the URL is formed with the Domain and the AbsolutePath
                            url = model.SocialModel.Domain + currentUri.AbsolutePath.TrimStart('/');
                        }
                        else
                        {
                            // The URL is formed using the current request URL (removing query string and fragments)
                            url = currentUri.GetLeftPart(UriPartial.Path);
                        }
                    }
                }

                if (CultureHelper.GetDefaultCulture().Equals(CultureHelper.GetCurrentCulture(), StringComparison.OrdinalIgnoreCase))
                {
                    var metaOpenGraph = new Dictionary<string, string>();
                    var metaTwitter = new Dictionary<string, string>();
                    
                    metaOpenGraph.Add("og:type", "website");

                    if (!string.IsNullOrWhiteSpace(model.SocialModel.Title))
                    {
                        metaOpenGraph.Add("og:title", HttpUtility.HtmlEncode(model.SocialModel.Title));
                        metaTwitter.Add("twitter:title", HttpUtility.HtmlEncode(model.SocialModel.Title));
                    }
                    else if (!string.IsNullOrWhiteSpace(model.PageTitle))
                    {
                        metaOpenGraph.Add("og:title", HttpUtility.HtmlEncode(model.PageTitle));
                        metaTwitter.Add("twitter:title", HttpUtility.HtmlEncode(model.PageTitle));
                    }

                    metaTwitter.Add("twitter:card", "summary");

                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        metaOpenGraph.Add("og:url", HttpUtility.HtmlEncode(url));
                        metaTwitter.Add("twitter:url", HttpUtility.HtmlEncode(url));
                    }

                    if (!string.IsNullOrWhiteSpace(model.SocialModel.Description))
                    {
                        metaOpenGraph.Add("og:description", HttpUtility.HtmlEncode(model.SocialModel.Description));
                        metaTwitter.Add("twitter:description", HttpUtility.HtmlEncode(model.SocialModel.Description));
                    }
                    else if (!string.IsNullOrWhiteSpace(model.MetaDescription))
                    {
                        metaOpenGraph.Add("og:description", model.MetaDescription);
                        metaTwitter.Add("twitter:description", model.MetaDescription);
                    }

                    if (!string.IsNullOrWhiteSpace(model.SocialModel.ImageUrl))
                    {
                        metaOpenGraph.Add("og:image", HttpUtility.HtmlEncode(model.SocialModel.ImageUrl));
                        metaTwitter.Add("twitter:image", HttpUtility.HtmlEncode(model.SocialModel.ImageUrl));
                    }
                    else
                    {
                        var defaultImageUrl = string.IsNullOrWhiteSpace(model.SocialModel.Domain) ?
                            CdnHelper.GetCdnUrl("/" + model.SocialModel.DefaultImageRelativePath, true) :
                            model.SocialModel.Domain + model.SocialModel.DefaultImageRelativePath;

                        metaOpenGraph.Add("og:image", HttpUtility.HtmlEncode(defaultImageUrl));
                        metaTwitter.Add("twitter:image", HttpUtility.HtmlEncode(defaultImageUrl));
                    }

                    if (!string.IsNullOrWhiteSpace(model.SocialModel.VideoUrl))
                    {
                        var sslUri = string.Format("{0}?format=html5", model.SocialModel.VideoUrl.Replace("http://", "https://"));

                        metaTwitter["twitter:card"] = "player";
                        metaTwitter.Add("twitter:player", HttpUtility.HtmlEncode(sslUri));
                        metaTwitter.Add("twitter:player:width", "643");
                        metaTwitter.Add("twitter:player:height", "320");
                    }

                    foreach (var metaTag in metaOpenGraph)
                    {
                        if (!model.AdditionalMetaTags.ContainsKey(metaTag.Key))
                        {
                            model.AdditionalMetaTags.Add(metaTag.Key, metaTag.Value);
                        }
                    }

                    foreach (var metaTag in metaTwitter)
                    {
                        if (!model.AdditionalMetaTags.ContainsKey(metaTag.Key))
                        {
                            model.AdditionalMetaTags.Add(metaTag.Key, metaTag.Value);
                        }
                    }

                    model.SocialModel.Url = url;
                }
            }
        }

        public static IHtmlString RenderSocial(this HtmlHelper helper, ViewMetadataModel model)
        {
            if (model.SocialModel != null)
            {
                helper.GenerateMetaTags(model);

                return helper.Partial("controls/Social", model.SocialModel);
            }

            return MvcHtmlString.Empty;
        }
    }
}