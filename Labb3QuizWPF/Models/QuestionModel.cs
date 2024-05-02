using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Labb3QuizWPF.Models;

public class QuestionModel : INotifyPropertyChanged
{
    public string Id { get; set; }

    private string _question;

    public string Question
    {
        get { return _question; }
        set
        {
            _question = value;
            OnPropertyChanged();
        }
    }

    private List<string> _choices;

    public List<string> Choices
    {
        get { return _choices; }
        set
        {
            _choices = value;
            OnPropertyChanged();
        }
    }

    private string _correctChoice;

    public string CorrectChoice
    {
        get { return _correctChoice; }
        set
        {
            _correctChoice = value;
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