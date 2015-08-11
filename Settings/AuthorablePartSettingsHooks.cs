using System.Collections.Generic;
using Devq.ExtendedBlog.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;

namespace Devq.ExtendedBlog.Settings
{
    public class AuthorablePartSettingsHooks : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition) {
            if (definition.PartDefinition.Name != typeof (AuthorablePart).Name) yield break;

            var model = definition.Settings.GetModel<AuthorablePartSettings>();

            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel) {
            if (builder.Name != typeof(AuthorablePart).Name) yield break;

            var model = new AuthorablePartSettings();
            updateModel.TryUpdateModel(model, typeof (AuthorablePartSettings).Name, null, null);
            builder.WithSetting(string.Format("{0}.{1}", typeof(AuthorablePartSettings).Name, model.AutomaticallyAssignEditorToAuthors.GetType().Name), model.AutomaticallyAssignEditorToAuthors.ToString());

            yield return DefinitionTemplate(model);
        }
    }
}