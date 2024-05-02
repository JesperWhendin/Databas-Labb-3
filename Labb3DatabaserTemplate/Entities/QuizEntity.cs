using MongoDB.Bson;

namespace DataAccess.Entities;

public class QuizEntity
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<QuestionEntity> Questions { get; set; } = new ();
}