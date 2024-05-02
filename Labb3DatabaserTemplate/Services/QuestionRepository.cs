using DataAccess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Services;

public class QuestionRepository
{
    private readonly IMongoCollection<QuestionEntity> _questions;

    public QuestionRepository()
    {
        var hostName = "localhost";
        var port = "27017";
        var dbName = "JwQuizDb";
        var client = new MongoClient($"mongodb://{hostName}:{port}");
        var db = client.GetDatabase(dbName);
        var collection = "Questions";
        _questions = db.GetCollection<QuestionEntity>(collection, new MongoCollectionSettings(){AssignIdOnInsert = true});
        CreateQuestions_OnStartUp();
    }

    public void CreateQuestions_OnStartUp()
    {
        if (_questions.Find(_ => true).CountDocuments() >= 1)
        {
            return;
        }
        var questions = new List<QuestionEntity>()
        {
            new()
            {
                Question = "How many planets are in our solar system?",
                Choices = new List<string>() { "8", "9", "10" },
                CorrectChoice = "8"
            },
            new()
            {
                Question = "How many moons does Mars have?",
                Choices = new List<string>() { "0", "1", "2" },
                CorrectChoice = "2"
            },
            new()
            {
                Question = "How old is Earth? Choices are in billion years.",
                Choices = new List<string>() { "3,9", "4,2", "4,5" },
                CorrectChoice = "4,5"
            },
            new()
            {
                Question = "Which year did humans first set foot on the moon?",
                Choices = new List<string>() { "1969", "1971", "1972" },
                CorrectChoice = "1969"
            },
            new()
            {
                Question = "Who was the Prime Minister of the United Kingdom during world war 2?",
                Choices = new List < string >() { "Nevile Chamberlain", "Winston Churchill", "Clement Attlee" },
                CorrectChoice = "Winston Churchill"
            },
            new()
            {
                Question = "In which year did the Titanic sink after hitting an iceberg?",
                Choices = new List < string >() { "1912", "1914", "1918" },
                CorrectChoice = "1912"
            },
            new()
            {
                Question = "How many elements are there in the periodic table?",
                Choices = new List<string>()  { "103", "108", "118" },
                CorrectChoice = "118"
            },
            new()
            {
                Question = "What is the capital city of Australia?",
                Choices = new List < string >() { "Sydney", "Brisbane", "Canberra" },
                CorrectChoice = "Canberra"
            },
            new()
            {
                Question = "In which mountain range is Mount Everest located?",
                Choices = new List < string >() { "Rocky Mountains", "Himalayas", "Andes Mountains" },
                CorrectChoice = "Himalayas"
            },
            new()
            {
                Question = "What is the largest desert in the world?",
                Choices = new List < string >() { "Gobi Desert", "Sahara Desert", "Antarctic Desert" },
                CorrectChoice = "Antarctic Desert"
            }
        };
        foreach (var question in questions)
        {
            _questions.InsertOne(question);
        }
    }

    public IEnumerable<QuestionEntity> GetAllQuestions()
    {
        var filter = Builders<QuestionEntity>.Filter.Empty;
        var allQuestions =
            _questions.Find(filter).ToList();
        return allQuestions;
    }

    public void UpdateQuestion(QuestionEntity newQuestion)
    {
        var filter = Builders<QuestionEntity>.Filter.Eq("_id", newQuestion.Id);
        var update = Builders<QuestionEntity>.Update
            .Set(question => question.Question, newQuestion.Question)
            .Set(question => question.Choices, newQuestion.Choices)
            .Set(question => question.CorrectChoice, newQuestion.CorrectChoice);
        _questions.UpdateOne(filter, update);
    }
}