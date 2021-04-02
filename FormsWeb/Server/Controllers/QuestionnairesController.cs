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
    [Authorize(Roles = "Administrator")]
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
        public List<Questionnaire> Get()
        {
            return _QuestionnaireDbContext.QuestionSets.ToList();
        }

        [HttpGet(nameof(Questionnaire))]
        public Questionnaire Questionnaire([FromQuery]int id)
        {
            var returnObj = _QuestionnaireDbContext.QuestionSets
                                .Include(nameof(FormsWeb.Shared.Questionnaire.Questions))//Learning: Only explicit include would fetch
                                                                                         //the related table data
                                .SingleOrDefault(s => s.Id == id);
            returnObj.Questions.ForEach(s => s.QuestionnaireRef = new Shared.Questionnaire() { QuestionnaireTitle = "SomeTitleToPassValidateion" });
            return returnObj;
        }

        [HttpPost(nameof(AddQuestionnaire))]
        public void AddQuestionnaire(Questionnaire questionnaire)
        {
            _QuestionnaireDbContext.QuestionSets.Add(questionnaire);
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(DeleteQuestionnaire))]
        public void DeleteQuestionnaire(Questionnaire questionnaire)
        {
            var questionnaireToDelete = _QuestionnaireDbContext.QuestionSets
                                            .Include(nameof(FormsWeb.Shared.Questionnaire.Questions))
                                            .Single(s => s.Id == questionnaire.Id);

            _QuestionnaireDbContext.QuestionSets.Remove(questionnaireToDelete);
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(UpdateQuestionnaireTitle))]
        public void UpdateQuestionnaireTitle(Questionnaire questionnaire)
        {
            var questionnaireToUpdate = _QuestionnaireDbContext.QuestionSets.Single(s => s.Id == questionnaire.Id);
            questionnaireToUpdate.QuestionnaireTitle = questionnaire.QuestionnaireTitle;
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(UpdateQuestionTitle))]
        public void UpdateQuestionTitle(Question question)
        {
            var questionToUpdate = _QuestionnaireDbContext.Questions.Single(s => s.Id == question.Id);
            questionToUpdate.Title = question.Title;
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(DeleteQuestion))]
        public void DeleteQuestion(Question question)
        {
            var questionToDelete = _QuestionnaireDbContext.Questions.Single(s => s.Id == question.Id);
            _QuestionnaireDbContext.Questions.Remove(questionToDelete);
            _QuestionnaireDbContext.SaveChanges();
        }

        [HttpPost(nameof(InsertQuestion))]
        public void InsertQuestion(Question question)
        {
            var questionnaireToAddTo = _QuestionnaireDbContext.QuestionSets.Include(nameof(FormsWeb.Shared.Questionnaire.Questions)).Single(s => s.Id == question.QuestionnaireRef.Id);
            questionnaireToAddTo.Questions.Add(question);
            _QuestionnaireDbContext.SaveChanges();
        }
    }
}
