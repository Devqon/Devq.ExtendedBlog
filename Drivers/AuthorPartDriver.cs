using System.Collections.Generic;
using System.Linq;
using Devq.ExtendedBlog.Models;
using Devq.ExtendedBlog.Settings;
using Devq.ExtendedBlog.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;

namespace Devq.ExtendedBlog.Drivers
{
    public class AuthorPartDriver : ContentPartDriver<AuthorPart> {

        private readonly IContentManager _contentManager;
        public AuthorPartDriver(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        protected override DriverResult Display(AuthorPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Author_Tags",
                    () => shapeHelper.Parts_Author_Tags(Tags: GetViewModels(part))),
                ContentShape("Parts_Author_Items",
                    () => {

                        // Retrieve settings for display mode of the items
                        var settings = part.Settings.GetModel<AuthorPartSettings>();
                        var displayMode = string.IsNullOrEmpty(settings.ItemsDisplayType) ? "Summary" : settings.ItemsDisplayType;

                        // Create list shape
                        var list = shapeHelper.List();
                        list.AddRange(part.Items.OrderByDescending(item => item.As<CommonPart>().PublishedUtc).Select(item => _contentManager.BuildDisplay(item, displayMode)));

                        return shapeHelper.Parts_Author_Items(ContentItems: list);
                    })
                );
        }

        private IEnumerable<WeightedTagViewModel> GetViewModels(AuthorPart part) {
            var list = new List<WeightedTagViewModel>();
            var maximum = part.Settings.GetModel<AuthorPartSettings>().MaxShownTags;

            foreach (var tag in part.Tags) {
                if (list.All(l => l.TagName != tag)) {
                    list.Add(new WeightedTagViewModel {TagName = tag, Weight = 1});
                }
                else {
                    list.Single(l => l.TagName == tag).Weight++;
                }
            }

            var ordered = list.OrderByDescending(l => l.Weight);

            return maximum == 0 ? ordered : ordered.Take(maximum);
        } 
    }
}