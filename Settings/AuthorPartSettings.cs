namespace Devq.ExtendedBlog.Settings
{
    public class AuthorPartSettings
    {
        public AuthorPartSettings() {
            MaxShownTags = 10;
        }

        /// <summary>
        /// Maximum number of tags shown, defaults to 10
        /// </summary>
        public int MaxShownTags { get; set; }

        /// <summary>
        /// The display type of the children, defaults to Summary
        /// </summary>
        public string ItemsDisplayType { get; set; }
    }
}