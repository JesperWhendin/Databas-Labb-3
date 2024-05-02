using MongoDB.Bson;

namespace DataAccess.Entities;

public class QuestionEntity
{
    public ObjectId Id { get; set; }
    public string Question { get; set; }
    public List<string> Choices { get; set; }
    public string CorrectChoice { get; set; }
    public override string ToString()
    {
        var text = Question;
        return text;
    }
}