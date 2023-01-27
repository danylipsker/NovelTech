using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NovelTech.libraries.ui.popups
{
    /// <summary>
    /// Interaction logic for Popup_question.xaml
    /// </summary>
    public partial class Popup_question : Window
    {
        public event EventHandler<QuestionHandler> OnConfirmed;
        public string answer;
        public bool confirmed = false;
        public Popup_question(string title, string question)
        {
            InitializeComponent();
            this.Title = title;
            l_quest.Content = question;

        }

        private void b_ok_Click(object sender, RoutedEventArgs e)
        {
            answer = tb_answer.Text;
            confirmed = true;
            OnConfirmed(this, new QuestionHandler() { answer = tb_answer.Text });
            this.Close();
        }

        private void b_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public class QuestionHandler : EventArgs
        {
            public string answer;
        }
    }
}
