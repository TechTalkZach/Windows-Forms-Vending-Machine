using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetFinal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjetFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuizExamenContext context;

        public HomeController(QuizExamenContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewQuizz()
        {
            return View();
        }

        public IActionResult SubmitNewQuizz(string UserName, string Email, int easyQuestions, int mediumQuestions,
            int hardQuestions)
        {
            if (UserName != null && Email != null) //si le UserName et Email ne sont pas null
            {
                if (easyQuestions + mediumQuestions + hardQuestions > 0) //si le quizz contiendra plus de 0 
                {
                    ViewBag.questionsInsuffissantes = "";
                    try
                    {
                        //creation d'une nouvelle entrée dans la table Quizz
                        context.Quiz.Add(new Quiz() {UserName = UserName, Email = Email});
                        context.SaveChanges();
                        /////***************************questions faciles************************//////
                        //indice des questions faciles
                        List<int> indiceQuestionsFaciles = new List<int>();

                        //liste des questions faciles 
                        List<Question> qListEasy =
                            context.Question.Where(question => question.CategoryId == 1).ToList();
                        if (easyQuestions >
                            qListEasy.Count()) //si l'utilisateur a demandé plus de questions faciles qu'il y en a
                        {
                            easyQuestions = qListEasy.Count();
                            Debug.WriteLine(easyQuestions);
                        }

                        Random rnd = new Random();
                        while (indiceQuestionsFaciles.Count() <
                               easyQuestions) //pendant que la liste des indices n'est pas totalement remplie
                        {
                            int randomInt = rnd.Next(0, qListEasy.Count());
                            if (!indiceQuestionsFaciles.Contains(randomInt))
                            {
                                indiceQuestionsFaciles.Add(randomInt);
                            }
                        }

                        //cherchons le dernier Id du quiz qu'on vient d'insérer
                        int lastQuizId = context.Quiz.ToList().Last().QuizId;

                        indiceQuestionsFaciles.ForEach(element =>
                        {
                            context.QuestionQuiz.Add(new QuestionQuiz()
                                {QuestionId = qListEasy[element].QuestionId, QuizId = lastQuizId});
                        });

                        /////***************************questions medium************************//////
                        //indice des questions medium
                        List<int> indiceQuestionsMedium = new List<int>();

                        //liste des questions faciles 
                        List<Question> qListMedium =
                            context.Question.Where(question => question.CategoryId == 2).ToList();
                        if (mediumQuestions >
                            qListMedium.Count()) //si l'utilisateur a demandé plus de questions faciles qu'il y en a
                        {
                            mediumQuestions = qListMedium.Count();
                        }

                        while (indiceQuestionsMedium.Count() <
                               mediumQuestions) //pendant que la liste des indices n'est pas totalement remplie
                        {
                            int randomInt = rnd.Next(0, qListMedium.Count());
                            if (!indiceQuestionsMedium.Contains(randomInt))
                            {
                                indiceQuestionsMedium.Add(randomInt);
                            }
                        }

                        indiceQuestionsMedium.ForEach(element =>
                        {
                            context.QuestionQuiz.Add(new QuestionQuiz()
                                {QuestionId = qListMedium[element].QuestionId, QuizId = lastQuizId});
                        });

                        /////***************************questions hard************************//////
                        //indice des questions hard
                        List<int> indiceQuestionsHard = new List<int>();

                        //liste des questions faciles 
                        List<Question> qListHard =
                            context.Question.Where(question => question.CategoryId == 3).ToList();
                        if (hardQuestions >
                            qListHard.Count()) //si l'utilisateur a demandé plus de questions faciles qu'il y en a
                        {
                            hardQuestions = qListHard.Count();
                        }

                        while (indiceQuestionsHard.Count() <
                               hardQuestions) //pendant que la liste des indices n'est pas totalement remplie
                        {
                            int randomInt = rnd.Next(0, qListHard.Count());
                            if (!indiceQuestionsHard.Contains(randomInt))
                            {
                                indiceQuestionsHard.Add(randomInt);
                            }
                        }

                        indiceQuestionsHard.ForEach(element =>
                        {
                            context.QuestionQuiz.Add(new QuestionQuiz()
                                {QuestionId = qListHard[element].QuestionId, QuizId = lastQuizId});
                        });
                        ViewBag.quizzSaved = "Quizz généré!!";
                        context.SaveChanges();
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", "Error, try again");
                    }
                }
                else
                {
                    ViewBag.questionsInsuffissantes = "Veuillez entrer une quantité de questions pour le quizz!!!";
                }
            }

            return View("NewQuizz");
        }

        public IActionResult passQuiz(int QuizId, string UserName, string Email)
        {
            ViewBag.quizzezFound = false;
            if (UserName != null && Email != null)
            {
                var listQuizUser = context.Quiz.Where(quiz => quiz.UserName == UserName && quiz.Email == Email);
                if (listQuizUser.Count() > 0)
                {
                    ViewBag.quizzezFound = true;
                    ViewBag.noUserFound = "";
                    ViewBag.listeQuizes = listQuizUser;
                    //var user1 = new Quiz() { QuizId = listQuizUser.First().QuizId, UserName = listQuizUser.First().UserName, Email = listQuizUser.First().Email };
                }
                else
                {
                    ViewBag.noUserFound = "Aucun quiz a été trouvé pour cet utilisateur";
                    return View();
                }
            }

            return View();
        }

        public IActionResult ReviserQuiz(string userName, string email)
        {
            ViewBag.reviserQuizFound = false;

            if (userName == null || email == null) return View();

            var listPasserQuizUser = context.Quiz.Where(quiz => quiz.UserName == userName && quiz.Email == email);

            var listFinishedQuiz = new List<Quiz>();
            foreach (var passerQuizUser in listPasserQuizUser)
            {
                var passedQuizId = passerQuizUser.QuizId;

                var sumQuestions = context.QuestionQuiz.Count(questionQuiz => questionQuiz.QuizId == passedQuizId);
                var sumAnswers = context.Answer.Count(answer => answer.QuizId == passedQuizId);

                if (sumAnswers != 0 && sumAnswers == sumQuestions)
                {
                    listFinishedQuiz.Add(passerQuizUser);
                }
            }

            if (listFinishedQuiz.Any())
            {
                ViewBag.reviserQuizFound = true;
                ViewBag.noReviserQuizFound = "";
                ViewBag.listeRevizerQuizes = listFinishedQuiz;
            }
            else
            {
                ViewBag.noreviserUserFound = "Aucun quiz a été trouvé pour cet utilisateur";
                return View();
            }

            return View();
        }

        public IActionResult AnswersToQuiz(string UserName, int QuizId)
        {
            //Ces deux ViewBag sont pour l'affichage 
            ViewBag.UserName = UserName;
            ViewBag.QuizId = QuizId;

            //Pour récupérer le quiz
            var questionsQuiz = context.QuestionQuiz.Where(q => q.QuizId == QuizId);
            var answerQuiz = context.Answer.Where(c => c.QuizId == QuizId);

            //Pour récupérer les questions qui sont dans le quiz sélectioné
            var questions = questionsQuiz.Select(q => q.Question); //Va nous donner un liste de type Question

            ViewBag.Questions = questions;

            ViewBag.Options = context.ItemOption.ToList(); //Pour chercher les reponses
            ViewBag.Answers = answerQuiz;

            return View();
        }

        public IActionResult ReviewAnswersToQuiz(string userName, string email, int quizId)
        {
            //Ces deux ViewBag sont pour l'affichage 
            ViewBag.UserName = userName;
            ViewBag.Email = email;
            ViewBag.QuizId = quizId;

            var listPasserQuizUser = context.Quiz.Where(quiz => quiz.UserName == userName && quiz.Email == email);
            var sumFinishQuestion = 0;
            var sumRightAnswers = 0;
            foreach (var passerQuizUser in listPasserQuizUser)
            {
                var passedQuizId = passerQuizUser.QuizId;
                var sumQuestions = context.QuestionQuiz.Count(questionQuiz => questionQuiz.QuizId == passedQuizId);
                var sumAnswers = context.Answer.Count(answer => answer.QuizId == passedQuizId);

                if (sumAnswers != 0 && sumAnswers == sumQuestions)
                {
                    sumFinishQuestion = sumQuestions;
                    var listAnswers = context.Answer.Where(answer => answer.QuizId == passedQuizId);
                    foreach (var answer in listAnswers)
                    {
                        var optionId = answer.OptionId;
                        sumRightAnswers += context.ItemOption.Count(itemOption =>
                            (itemOption.OptionId == optionId) && (itemOption.IsRight));
                    }
                }
            }

            //Pour récupérer le quiz
            var questionsQuiz = context.QuestionQuiz.Where(q => q.QuizId == quizId);
            var answerQuiz = context.Answer.Where(c => c.QuizId == quizId);

            //Pour récupérer les questions qui sont dans le quiz sélectioné
            var questions = questionsQuiz.Select(q => q.Question); //Va nous donner un liste de type Question

            ViewBag.SumFinishQuestions = sumFinishQuestion;
            ViewBag.SumRightAnswers = sumRightAnswers;
            ViewBag.Questions = questions;

            ViewBag.Options = context.ItemOption.ToList(); //Pour chercher les reponses
            ViewBag.Answers = answerQuiz;

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}