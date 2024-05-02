using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Labb3QuizWPF.Models;

public class QuizModel : INotifyPropertyChanged
{

    public string Id { get; set; }

    private string _name;

    public string Name
    {
        get { return _name; }

        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _description;

    public string Description
    {
        get { return _description; }
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    private List<QuestionModel> _questionList;

    public List<QuestionModel> QuestionList
    {
        get { return _questionList; }
        set
        {
            _questionList = value;
            OnPropertyChanged();
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}