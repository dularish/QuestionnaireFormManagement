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
        public virtual List<Question> Questions { get; set; }//Learning: For one-to-many relationships, it is suggested to have both List<'T> property
                                                                    //and also in 'T class to have a virtual property with Required attribute
                                                                    //in order to have not null constraint of ForeignKey
                                                                    //Without Not null constraint of FK, delete operation will not cascade

    }

    public class Question
    {
        public int Id { get; init; }

        [Required]
        public string Title { get; set; }

        [Required]
        public virtual Questionnaire QuestionnaireRef { get; set; }
    }

    public class ResponseSet
    {
        public int Id { get; init; }

        public Questionnaire Questionnaire { get; set; }

        public string ResponseSetOwnerEmailId { get; set; }

        public List<Response> Responses { get; set; }
    }

    public class Response
    {
        public int Id { get; init; }

        public Question Question { get; set; }

        public string ResponseText { get; set; }
    }
}
