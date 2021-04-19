using FormsWeb.Server.Data;
using FormsWeb.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormsWeb.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuestionnairesController : ControllerBase
    {
        private QuestionnaireDbContext _QuestionnaireDbContext;

        public QuestionnairesController(QuestionnaireDbContext questionnaireDbContext)
        {
            _QuestionnaireDbContext = questionnaireDbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,User")]
        public List<Questionnaire> Get()
        {
            return _QuestionnaireDbContext.QuestionSets.ToList();
        }

        [HttpGet(nameof(Questionnaire))]
        [Authorize(Roles = "Administrator,User")]
        public Questionnaire Questionnaire([FromQuery]int id)
        {
            var questionnaire = _QuestionnaireDbContext.QuestionSets
                                .Include(s => s.Questions)//Learning: Only explicit include would fetch
                                                                                         //the related table data
                                .ThenInclude(s => s.MultipleChoiceSet).ThenInclude(s => s.MultipleChoices)
                                .SingleOrDefault(s => s.Id == id);
            questionnaire.Questions.ForEach(s => s.Questionnaire = null);//Learning: Prevent circular referencing when giving out
            questionnaire.Questions.ForEach(s =>
            {
                if(s.MultipleChoiceSet != null)
                {
                    s.MultipleChoiceSet.Question = null;
                    s.MultipleChoiceSet.MultipleChoices.ForEach(y => y.MultipleChoiceSet = null);
                }
            });
            return questionnaire;
        }

        [HttpGet(nameof(ResponseSets))]
        [Authorize(Roles = "Administrator")]
        public List<ResponseSet> ResponseSets([FromQuery] int questionnaireid)
        {
            return _QuestionnaireDbContext.ResponseSets
                .Include(nameof(FormsWeb.Shared.ResponseSet.Questionnaire))
                .Where(s => s.QuestionnaireId == questionnaireid).ToList();
        }

        [HttpGet(nameof(ResponsesForResponseSetId))]
        [Authorize(Roles = "Administrator")]
        public List<Response> ResponsesForResponseSetId([FromQuery] int responsesetid)
        {
            List<Response> responses = _QuestionnaireDbContext.Responses
                                .Include(s => s.Question)
                                .ThenInclude(s => s.MultipleChoiceSet).ThenInclude(s => s.MultipleChoices)
                                .Include(s => s.MultipleChoicesSelected).ThenInclude(s => s.MultipleChoice)
                                .Where(s => s.ResponseSetId == responsesetid).ToList();
            responses.ForEach(s =>
            {
                s.Question.Responses = null;
                if (s.Question.MultipleChoiceSet != null)
                {
                    s.Question.MultipleChoiceSet.Question = null;
                    s.Question.MultipleChoiceSet.MultipleChoices.ForEach(y => y.MultipleChoiceSet = null);
                }
                s.MultipleChoicesSelected?.ForEach(y =>
                {
                    y.MultipleChoice.MultipleChoiceSet = null;
                    y.Response = null;
                });
            });
            return responses;
        }

        [HttpPost(nameof(AddQuestionnaire))]
        [Authorize(Roles = "Administrator")]
        public void AddQuestionnaire(Questionnaire questionnaire)
        {
            _QuestionnaireDbContext.QuestionSets.Add(questionnaire);
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(DeleteQuestionnaire))]
        [Authorize(Roles = "Administrator")]
        public async Task DeleteQuestionnaire(Questionnaire questionnaire)
        {
            var questionnaireToDelete = _QuestionnaireDbContext.QuestionSets
                                            .Include(nameof(FormsWeb.Shared.Questionnaire.Questions))
                                            .Single(s => s.Id == questionnaire.Id);

            await _QuestionnaireDbContext.Questions.ForEachAsync(DeleteQuestionWithoutCommit);//Learning: You have to be explicit to remove children relationship
                                                                                              //dependent entities inorder to remove grandchildren entities

            _QuestionnaireDbContext.QuestionSets.Remove(questionnaireToDelete);
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(DeleteResponseSet))]
        [Authorize(Roles = "Administrator")]
        public void DeleteResponseSet(ResponseSet responseSet)
        {
            var responseSetToDelete = _QuestionnaireDbContext.ResponseSets
                                            .Include(s => s.Responses)
                                            .ThenInclude(s => s.MultipleChoicesSelected)
                                            .Single(s => s.Id == responseSet.Id);

            _QuestionnaireDbContext.ResponseSets.Remove(responseSetToDelete);
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(UpdateQuestionnaireTitle))]
        [Authorize(Roles = "Administrator")]
        public void UpdateQuestionnaireTitle(Questionnaire questionnaire)
        {
            var questionnaireToUpdate = _QuestionnaireDbContext.QuestionSets.Single(s => s.Id == questionnaire.Id);
            questionnaireToUpdate.QuestionnaireTitle = questionnaire.QuestionnaireTitle;
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(UpdateQuestionTitle))]
        [Authorize(Roles = "Administrator")]
        public void UpdateQuestionTitle(Question question)
        {
            var questionToUpdate = _QuestionnaireDbContext.Questions.Single(s => s.Id == question.Id);
            questionToUpdate.Title = question.Title;
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(DeleteQuestion))]
        [Authorize(Roles = "Administrator")]
        public void DeleteQuestion(Question question)
        {
            DeleteQuestionWithoutCommit(question);
            _QuestionnaireDbContext.SaveChanges();
        }

        private void DeleteQuestionWithoutCommit(Question question)
        {
            var questionToDelete = _QuestionnaireDbContext.Questions
                                                .Include(s => s.Responses)
                                                .Include(s => s.MultipleChoiceSet).ThenInclude(s => s.MultipleChoices)
                                                .Single(s => s.Id == question.Id);
            _QuestionnaireDbContext.Questions.Remove(questionToDelete);
        }

        [HttpPost(nameof(InsertQuestion))]
        [Authorize(Roles = "Administrator")]
        public void InsertQuestion(Question question)
        {
            var questionnaireToAddTo = _QuestionnaireDbContext.QuestionSets.Include(nameof(FormsWeb.Shared.Questionnaire.Questions)).Single(s => s.Id == question.Questionnaire.Id);
            questionnaireToAddTo.Questions.Add(question);
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(SubmitResponse))]
        [Authorize(Roles = "User")]
        public void SubmitResponse(ResponseSet responseSet)
        {
            responseSet.Questionnaire = null;//Learning: When adding an object which has already existing referenced objects,
                                             //do not have any object reference, have just foreign key value

            responseSet.Responses.ForEach(response =>
            {
                if(response.Question.ResponseTypeAllowed != ResponseTypeAllowed.Text)
                {
                    var choices = _QuestionnaireDbContext.MultipleChoiceSets.Include(s => s.MultipleChoices).Single(s => s.Id == response.Question.MultipleChoiceSet.Id).MultipleChoices;
                    var filteredChoices = choices.Where(s => response.MultipleChoicesSelected.Any(y => y.MultipleChoiceId == s.Id)).ToList();
                    var responseMultipleChoiceMap = filteredChoices.Select(s => new ResponseMultipleChoiceSelectionMap() { MultipleChoiceId = s.Id, Response = response }).ToList();
                    response.MultipleChoicesSelected = responseMultipleChoiceMap;//Learning: When adding an object which has already existing one-to-many existing
                                                                       //referenced objects, take those objects from DbContext again, so that EF understands
                                                                       //that they are not objects to be created
                }
                
                response.Question = null;
            });
            
            _QuestionnaireDbContext.ResponseSets.Add(responseSet);
            _QuestionnaireDbContext.SaveChanges();
        }
    }
}
