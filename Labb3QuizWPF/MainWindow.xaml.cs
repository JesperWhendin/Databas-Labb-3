using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DataAccess.Entities;
using DataAccess.Services;
using Labb3QuizWPF.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Labb3QuizWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly QuestionRepository _questionrepo;
        private readonly QuizRepository _quizrepo;
        public ObservableCollection<QuizEntity> Quizzes { get; set; }
        public ObservableCollection<QuestionEntity> Questions { get; set; }
        public QuestionModel? SelectedQuestion { get; set; } = new();
        public QuizModel? SelectedQuiz { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _questionrepo = new QuestionRepository();
            _quizrepo = new QuizRepository();
            Quizzes = new ObservableCollection<QuizEntity>(_quizrepo.GetAllQuizzes());
            QuizComboBox.ItemsSource = Quizzes;
            var allQ = _questionrepo.GetAllQuestions();
            foreach (var q in allQ)
            {
                AllQuestionsListBox.Items.Add(q);
            }
        }

        private void QuizComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshQuizQuestions();
        }

        private void RefreshQuizQuestions()
        {
            QuizQuestionsListBox.Items.Clear();
            if (QuizComboBox.SelectedItem is QuizEntity selectedQuiz)
            {
                foreach (var question in selectedQuiz.Questions)
                {
                    QuizQuestionsListBox.Items.Add(question);
                }
            }
        }

        private void RemoveQuestionFromQuizBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (QuizComboBox.SelectedItem is null ||
                QuizQuestionsListBox.SelectedItem is null)
            {
                return;
            }
            if (QuizComboBox.SelectedItem is QuizEntity quiz &&
                QuizQuestionsListBox.SelectedItem is QuestionEntity question)
            {
                _quizrepo.RemoveQuestionFromQuiz(quiz, question);
            }
            RefreshQuizQuestions();
        }

        private void UpdateQuestionBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (AllQuestionsListBox.SelectedItem is null)
            {
                MessageBox.Show("Make sure you select a question in the list box to the right.");
                return;
            }
            if (QuestionTextBox is null || ChoicesTextBox is null || CorrectChoiceTextBox is null)
            {
                MessageBox.Show("Make sure text boxes are properly filled in.");
                return;
            }
            if (AllQuestionsListBox.SelectedItem is QuestionEntity question)
            {
                QuestionEntity newQuestion = new QuestionEntity();
                newQuestion.Id = question.Id;
                newQuestion.Question = QuestionTextBox.Text;
                newQuestion.Choices = ChoicesTextBox.Text.Split(',').Select(choice => choice.Trim()).ToList();
                newQuestion.CorrectChoice = CorrectChoiceTextBox.Text;
                _questionrepo.UpdateQuestion(newQuestion);
                AllQuestionsListBox.SelectedItem = null;
                AllQuestionsListBox.Items.Clear();
                var allQ = _questionrepo.GetAllQuestions();
                foreach (var q in allQ)
                {
                    AllQuestionsListBox.Items.Add(q);
                }
            }
        }

        private void AllQuestionsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllQuestionsListBox.SelectedItem is null)
            {
                QuestionTextBox.Clear();
                ChoicesTextBox.Clear();
                CorrectChoiceTextBox.Clear();
            }
            if (AllQuestionsListBox.SelectedItem is QuestionEntity selectedQuestion)
            {
                QuestionTextBox.Text = selectedQuestion.Question;
                ChoicesTextBox.Text = selectedQuestion.Choices.Aggregate((a, b) => a + ", " + b);
                CorrectChoiceTextBox.Text = selectedQuestion.CorrectChoice;
            }
        }

        private void AddQuestionToQuizBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (QuizComboBox.SelectedItem is null)
            {
                MessageBox.Show("Select a quiz in the combobox.");
                return;
            }
            if (AllQuestionsListBox.SelectedItem is null)
            {
                MessageBox.Show("Select a question in the listbox to the right.");
            }
            if (AllQuestionsListBox.SelectedItem is QuestionEntity question &&
                QuizComboBox.SelectedItem is QuizEntity quiz)
            {
                _quizrepo.AddQuestionToQuiz(question, quiz);
            }
            RefreshQuizQuestions();
        } 
    }
}