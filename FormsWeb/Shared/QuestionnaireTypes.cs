using FormsWeb.Shared.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsWeb.Shared
{
    public class Questionnaire
    {
        public int Id { get; init; }

        public string QuestionnaireOwnerEmailId { get; set; }

        [Required]
        public string QuestionnaireTitle { get; set; }
        [ValidateComplexType]
        [EnsureMinimumCountOfElements(1, ErrorMessage = "At least one question required")]
        public List<Question> Questions { get; set; }//Learning: For one-to-many relationships, it is suggested to have both List<'T> property
                                                                    //and also in 'T class to have a property with foreignKey field with Required attribute
                                                                    //in order to have not null constraint of ForeignKey
                                                                    //Without Not null constraint of FK, delete operation will not cascade
    }

    public class Question
    {
        public int Id { get; init; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }

        public ResponseTypeAllowed ResponseTypeAllowed { get; set; }

        [ValidateComplexType]
        public MultipleChoiceSet MultipleChoiceSet { get; set; }

        public virtual List<Response> Responses { get; set; }
    }

    public class MultipleChoiceSet
    {
        public int Id { get; init; }

        [EnsureMinimumCountOfElements(2, ErrorMessage = "At least two options required")]
        public List<MultipleChoice> MultipleChoices { get; set; }

        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }

    public class MultipleChoice
    {
        public int Id { get; init; }

        [Required]
        public string Title { get; set; }


        [Required]
        public int MultipleChoiceSetId { get; set; }
        public MultipleChoiceSet MultipleChoiceSet { get; set; }
    }

    public enum ResponseTypeAllowed
    {
        Text, Checkbox, Radiobutton
    }

    public class ResponseSet
    {
        public int Id { get; init; }

        [Required]
        public int QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }

        public string ResponseSetOwnerEmailId { get; set; }

        [ValidateComplexType]
        public List<Response> Responses { get; set; }
    }

    public class Response
    {
        public int Id { get; init; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string ResponseText { get; set; }

        [EnsureMinimumCountOfElements(1, ErrorMessage = "At least one option to be selected")]
        public List<ResponseMultipleChoiceSelectionMap> MultipleChoicesSelected { get; set; }

        [Required]
        public int ResponseSetId { get; set; }
        public ResponseSet ResponseSet { get; set; }
    }

    public class ResponseMultipleChoiceSelectionMap
    {
        public int Id { get; init; }

        [Required]
        public int ResponseId { get; set; }
        public Response Response { get; set; }

        [Required]
        public int MultipleChoiceId { get; set; }
        public MultipleChoice MultipleChoice { get; set; }
    }

    //Cascade delete cases
        //1. Questionnaire -> all Questions ( -> all Response)
        //2. ReponseSet -> all Response
        //3. Questionnaire -> all ResponseSet ( -> all Response)
        //4. Question -> all Response
}
