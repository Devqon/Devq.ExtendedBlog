using System;
using System.Collections.Generic;
using System.Linq;
using Devq.ExtendedBlog.Models;
using Devq.ExtendedBlog.Settings;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentPicker.Fields;
using Orchard.ContentPicker.Settings;
using Orchard.Security;

namespace Devq.ExtendedBlog.Handlers
{
    public class AuthorablePartHandler : ContentHandler
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthorablePartHandler(IAuthenticationService authenticationService) {
            _authenticationService = authenticationService;

            OnUpdated<AuthorablePart>((ctx, part) => UpdateAuthors(part));
        }

        private void UpdateAuthors(AuthorablePart part)
        {

            var settings = part.Settings.GetModel<AuthorablePartSettings>();
            // No need to go further if the setting is set to false
            if (!settings.AutomaticallyAssignEditorToAuthors)
                return;

            // Search for the authors field
            var field = part
                .Fields
                .OfType<ContentPickerField>()
                .FirstOrDefault(f => f.Name == Constants.FieldName);

            // No such field attached
            if (field == null)
                return;

            // Check if the 'User' content type is in the content picker
            var contentTypeSettings = field
                .PartFieldDefinition
                .Settings
                .GetModel<ContentPickerFieldSettings>()
                .DisplayedContentTypes
                .Split(new[] { Constants.ContentPickerFieldContentTypesSeparator, ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // If no user, then the authenticated user has no use
            if (!contentTypeSettings.Contains("User"))
                return;

            var currentUser = _authenticationService.GetAuthenticatedUser();
            if (currentUser == null)
                return;

            // Add current user to field values
            if (!field.Ids.Contains(currentUser.Id))
            {
                field.Ids = (new List<int>(field.Ids) { currentUser.Id }).ToArray();
            }
        }
    }
}