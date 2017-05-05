namespace PowerBI.Entities
{
    using System;
    using Acom.IO.Entities;

    /// <summary>
    /// The country.
    /// </summary>
    [Serializable]
    public class Country : IHaveSlug
    {
        /// <summary>
        /// Gets or sets the slug.
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets the localization key.
        /// </summary>
        public string LocKey { get; set; }

        /// <summary>
        /// Gets or sets the ISO 2 code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the ISO 3 code
        /// </summary>
        public string CodeAlpha3 { get; set; }

        /// <summary>
        /// Gets or sets the contact information for this country
        /// </summary>
        public ContactInfo ContactInfo { get; set; }
    }
}
