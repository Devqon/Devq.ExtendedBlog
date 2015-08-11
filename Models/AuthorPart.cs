using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace Devq.ExtendedBlog.Models
{
    public class AuthorPart : ContentPart<AuthorPartRecord>
    {
        internal readonly LazyField<IEnumerable<string>> _tagsField = new LazyField<IEnumerable<string>>();
        internal readonly LazyField<IEnumerable<IContent>> _itemsField = new LazyField<IEnumerable<IContent>>(); 

        public IEnumerable<string> Tags
        {
            get { return _tagsField.Value; }
            set { _tagsField.Value = value; }
        }

        public IEnumerable<IContent> Items {
            get { return _itemsField.Value; }
            set { _itemsField.Value = value; }
        } 
    }
}