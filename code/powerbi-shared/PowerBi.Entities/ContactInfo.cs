using System;

namespace PowerBI.Entities
{
    /// <summary>
    /// The contact information for a country.
    /// </summary>
    [Serializable]
    public class ContactInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether requires contact consent.
        /// </summary>
        public bool RequiresContactConsent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether contact me default.
        /// </summary>
        public bool ContactMeDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enables contact phone.
        /// </summary>
        public bool EnablesContactPhone { get; set; }
    }
}
