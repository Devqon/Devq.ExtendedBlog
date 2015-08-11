using System.Collections.Generic;
using System.Globalization;
using Devq.ExtendedBlog.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;

namespace Devq.ExtendedBlog.Settings
{
    public class AuthorPartSettingsHooks : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition) {
            if (definition.PartDefinition.Name != typeof (AuthorPart).Name) yield break;

            var model = definition.Settings.GetModel<AuthorPartSettings>();

            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel) {
            if (builder.Name != typeof (AuthorPart).Name) yield break;

            var settings = new AuthorPartSettings();
            if (updateModel.TryUpdateModel(settings, typeof (AuthorPartSettings).Name, null, null)) {

                builder
                    .WithSetting(string.Format("{0}.{1}", typeof (AuthorPartSettings).Name, "MaxShownTags"), settings.MaxShownTags.ToString(CultureInfo.InvariantCulture))
                    .WithSetting(string.Format("{0}.{1}", typeof (AuthorPartSettings).Name, "ItemsDisplayType"), settings.ItemsDisplayType);
            }

            yield return DefinitionTemplate(settings);
        }
    }
}