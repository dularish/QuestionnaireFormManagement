using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormsWeb.Client
{
    public class CustomValidator : ComponentBase
    {
        private ValidationMessageStore messageStore;

        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if(CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{nameof(CustomValidator)} requires a cascading parameter " +
                    $"of type {nameof(EditContext)}, like when using it inside an {nameof(EditForm)}");
            }

            messageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += (s, e) => messageStore.Clear();

            CurrentEditContext.OnFieldChanged += (s, e) => messageStore.Clear(e.FieldIdentifier);
        }

        public void DisplayErrors(Dictionary<string, List<string>> errors)
        {
            foreach (var error in errors)
            {
                messageStore.Add(CurrentEditContext.Field(error.Key), error.Value);
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }

        public void AddError(string field, List<string> errors)
        {
            messageStore.Add(CurrentEditContext.Field(field), errors);

            CurrentEditContext.NotifyValidationStateChanged();
        }

        public void ClearErrors()
        {
            messageStore.Clear();
            CurrentEditContext.NotifyValidationStateChanged();
        }
    }
}
