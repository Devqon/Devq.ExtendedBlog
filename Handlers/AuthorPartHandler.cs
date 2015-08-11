using System.Collections.Generic;
using System.Linq;
using Devq.ExtendedBlog.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Projections.Models;
using Orchard.Tags.Models;

namespace Devq.ExtendedBlog.Handlers
{
    public class AuthorPartHandler : ContentHandler {
        private readonly IContentManager _contentManager;

        public AuthorPartHandler(IRepository<AuthorPartRecord> repository,
            IContentManager contentManager) {

            _contentManager = contentManager;

            Filters.Add(StorageFilter.For(repository));
            OnLoaded<AuthorPart>((ctx, part) => SetupAuthorPart(part));
        }

        public void SetupAuthorPart(AuthorPart part) {

            // {2} because a formatted {22} also returns true if searching just for '2'
            var authorId = "{" + part.ContentItem.Id + "}";

            // Lazy field items loader
            part._itemsField.Loader(prt => {

                // Magic HqlQuery to query the field
                var items = _contentManager
                    .HqlQuery<AuthorablePart>()
                    .Where(e => e
                        .ContentPartRecord<FieldIndexPartRecord>()
                        .Property("StringFieldIndexRecords", Constants.FieldName),
                    f => f.And(
                        checkName => checkName
                            .Eq("PropertyName", string.Format("{0}.{1}.", typeof(AuthorablePart).Name, Constants.FieldName)), // PartName.FieldName.
                        checkValue => checkValue
                            .Like("Value", authorId, HqlMatchMode.Anywhere)
                    ));

                return items.List().Select(i => i.ContentItem);
            });

            // Lazy field tags loader
            part._tagsField.Loader(prt => {

                var tagNames = new List<string>();

                if (part._itemsField.Value != null) {
                    tagNames.AddRange(part.Items.SelectMany(i => i.ContentItem.As<TagsPart>().CurrentTags));
                }

                return tagNames;
            });
        }
    }
}