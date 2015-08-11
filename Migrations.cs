using Devq.ExtendedBlog.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Devq.ExtendedBlog
{
    public class Migrations : DataMigrationImpl
    {
        public int Create() {

            // Extend profile
            ContentDefinitionManager.AlterPartDefinition("ExtendedProfilePart", part => part

                .WithField("DisplayName", field => field
                    .OfType("TextField"))

                .WithField("Facebook", field => field
                    .OfType("TextField"))

                .WithField("Twitter", field => field
                    .OfType("TextField"))

                .WithField("Google", field => field
                    .OfType("TextField"))

                .WithField("Github", field => field
                    .OfType("TextField"))

                .WithField("Image", field => field
                    .OfType("MediaLibraryPickerField")));

            // Attach to user
            ContentDefinitionManager.AlterTypeDefinition("User", type => type
                
                .WithPart("ProfilePart")
                .WithPart("BodyPart")
                .WithPart("ExtendedProfilePart")

                .WithPart(typeof(AuthorPart).Name));

            SchemaBuilder.CreateTable(typeof(AuthorPartRecord).Name, table => table
                .ContentPartRecord());

            // Authors field
            ContentDefinitionManager.AlterPartDefinition(typeof(AuthorablePart).Name, part => part

                .Attachable()
                .WithDescription("Turns your content type into authorable items through a content picker field, which are displayed with the author.")

                .WithField(Constants.FieldName, field => field
                    .OfType("ContentPickerField")
                    .WithSetting("ContentPickerFieldSettings.Multiple", "true")
                    .WithSetting("ContentPickerFieldSettings.DisplayedContentTypes", "User")));

            return 3;
        }

        public int UpdateFrom2() {

            // Attach to user
            ContentDefinitionManager.AlterTypeDefinition("User", type => type

                .WithPart("ProfilePart")
                .WithPart("BodyPart")
                .WithPart("ExtendedProfilePart")

                .WithPart(typeof(AuthorPart).Name));

            SchemaBuilder.CreateTable(typeof(AuthorPartRecord).Name, table => table
                .ContentPartRecord());

            // Authors field
            ContentDefinitionManager.AlterPartDefinition(typeof(AuthorablePart).Name, part => part

                .Attachable()
                .WithDescription("Turns your content type into authorable items through a content picker field, which are displayed with the author.")

                .WithField(Constants.FieldName, field => field
                    .OfType("ContentPickerField")
                    .WithSetting("ContentPickerFieldSettings.Multiple", "true")
                    .WithSetting("ContentPickerFieldSettings.DisplayedContentTypes", "User")));

            return 3;
        }
    }
}