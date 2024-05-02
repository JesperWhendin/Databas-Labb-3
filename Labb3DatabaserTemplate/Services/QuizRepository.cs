using DataAccess.Entities;
using MongoDB.Driver;

namespace DataAccess.Services;

public class QuizRepository
{
    private readonly IMongoCollection<QuizEntity> _quizzes;
    public QuizRepository()
    {
        var hostName = "localhost";
        var port = "27017";
        var dbName = "JwQuizDb";
        var client = new MongoClient($"mongodb://{hostName}:{port}");
        var db = client.GetDatabase(dbName);
        var collection = "Quizzes";
        _quizzes = db.GetCollection<QuizEntity>(collection, new MongoCollectionSettings() { AssignIdOnInsert = true });
        CreateQuizzez_OnStartUp();
    }

    public void CreateQuizzez_OnStartUp()
    {
        if (_quizzes.Find(_ => true).CountDocuments() >= 1)
        {
            return;
        }
        var quizzes = new List<QuizEntity>()
        {
            new()
            {
                Name = "Trivia",
                Description = "A quiz about general knowledge."
            },
            new()
            {
                Name = "GeoQuiz",
                Description = "A quiz about geography."
            }
        };
        foreach (var quiz in quizzes)
        {
            _quizzes.InsertOne(quiz);
        }
    }

    public IEnumerable<QuizEntity> GetAllQuizzes()
    {
        var filter = Builders<QuizEntity>.Filter.Empty;
        var allQuizzes =
            _quizzes.Find(filter).ToList();
        return allQuizzes;
    }

    public void AddQuestionToQuiz(QuestionEntity question, QuizEntity quiz)
    {
        var filterQuiz = Builders<QuizEntity>.Filter.Eq("_id", quiz.Id);
        var update = Builders<QuizEntity>.Update.Push(q => q.Questions, question);
        _quizzes.UpdateOne(filterQuiz, update);
        quiz.Questions.Add(question);
    }

    public void RemoveQuestionFromQuiz(QuizEntity quiz, QuestionEntity question)
    {
        var filterQuiz = Builders<QuizEntity>.Filter.Eq("_id", quiz.Id);
        var filterQuestion = Builders<QuestionEntity>.Filter.Eq("_id", question.Id);
        var update = Builders<QuizEntity>.Update.PullFilter(q => q.Questions, filterQuestion);
        _quizzes.UpdateOne(filterQuiz, update);
        quiz.Questions.Remove(question);
    }
}