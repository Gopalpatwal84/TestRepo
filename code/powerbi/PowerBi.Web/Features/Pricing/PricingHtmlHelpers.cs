using System.Web;
using System.Web.Mvc;
using Acom.Mvc.Helpers;
using System.Globalization;

namespace PowerBI.Web.Features.Pricing
{
    public static class PricingHtmlHelpers
    {
        public static IHtmlString Price(this HtmlHelper helper, double amount, int? decimals = null)
        {
            return helper.Price(new PriceViewModel
            {
                Amount = amount,
                RateFormat = "{0}",
                Decimals = decimals
            });
        }

        public static IHtmlString Price(this HtmlHelper helper, PriceViewModel model)
        {
            var digitsAttribute = string.Empty;
            if (model.Decimals.HasValue)
            {
                // Since it would be very odd to display prices with only one decimal digit, if the "Decimals" value is 1, it is changed to 2
                digitsAttribute = string.Format(
                    CultureInfo.InvariantCulture,
                    " data-decimals=\"{0}\"",
                    model.Decimals.Value == 1 ? 2 : model.Decimals.Value);
            }

            var span = helper.RawFormatInvariant(
                "<span class=\"price-data\" data-amount='{0}'{1}>${0}</span>",
                model.Amount,
                digitsAttribute);

            return helper.RawFormatInvariant(model.RateFormat, span);
        }
    }
}