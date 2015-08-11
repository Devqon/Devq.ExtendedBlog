using Devq.ExtendedBlog.Models;
using Orchard.ContentManagement.Drivers;

namespace Devq.ExtendedBlog.Drivers
{
    public class AuthorablePartDriver : ContentPartDriver<AuthorablePart> {
        // Empty driver to enable 'OnUpdated' event of the part
    }
}