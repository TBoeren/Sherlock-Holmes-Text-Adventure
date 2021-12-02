using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class EndQuestions
    {
        private DBLayoutPanel FirstSeriesPanel;
        private DBLayoutPanel SecondSeriesPanel;
        private DBLayoutPanel FirstSeriesAnswersPanel;
        private DBLayoutPanel SecondSeriesAnswersPanel;

        private DataTable QuestionDatatable;
        private Dictionary<string, string> QuestionAnswerDictionary = new Dictionary<string, string>();

        public EndQuestions(DBLayoutPanel Series1, DBLayoutPanel Series2, DBLayoutPanel Answers1, DBLayoutPanel Answers2)
        {
            FirstSeriesPanel = Series1;
            SecondSeriesPanel = Series2;
            FirstSeriesAnswersPanel = Answers1;
            SecondSeriesAnswersPanel = Answers1;

            ExternalFileManager FileManager = new ExternalFileManager();
            QuestionDatatable = FileManager.ConvertCSVToDatatable("SherlockQuestions.csv");
        }

        public bool IsPlayerReady()
        {
            //Check if the player is ready to continue to the end questions
            DialogResult dialogResult = MessageBox.Show("If you continue, you are not allowed to go back to any other tab. Are you sure you want to proceed to questions? ", "End Questions", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //If they are, load the questions in
                var QuestionsAndPossibleAnswers = GetQuestionsAndPossibleAnswers();
                PrintQuestionsAndPossibleAnswers(QuestionsAndPossibleAnswers.Item1, FirstSeriesPanel);
                PrintQuestionsAndPossibleAnswers(QuestionsAndPossibleAnswers.Item2, SecondSeriesPanel);
                return true;
            }
            else if (dialogResult == DialogResult.No)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        (string[], string[]) GetQuestionsAndPossibleAnswers()
        {
            List<string> FirstSeriesQuestionList = new List<string>();
            List<string> SecondSeriesQuestionList = new List<string>();
            bool SecondSeries = false;

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = QuestionDatatable;

            foreach (var item in bindingSource)
            {
                DataRowView Row = (DataRowView)item;
                if (!SecondSeries)
                {
                    if (Row["question"].ToString() == "-" && Row["answer"].ToString() == "-")
                    {
                        SecondSeries = true;
                        continue;
                    }
                    FirstSeriesQuestionList.Add(Row["question"].ToString());
                    FirstSeriesQuestionList.Add(Row["possible answers"].ToString());
                }
                else
                {
                    SecondSeriesQuestionList.Add(Row["question"].ToString());
                    SecondSeriesQuestionList.Add(Row["possible answers"].ToString());
                }

                QuestionAnswerDictionary.Add(Row["question"].ToString(), Row["answer"].ToString());
            }

            return (FirstSeriesQuestionList.ToArray(), SecondSeriesQuestionList.ToArray());
        }

        void PrintQuestionsAndPossibleAnswers(string[] QuestionsAndAnswers, DBLayoutPanel SeriesPanel)
        {
            SeriesPanel.SuspendLayout();

            for (int i = 0; i < QuestionsAndAnswers.Length; i++)
            {
                if (i % 2 == 0)
                {
                    Label LocationLabel = new Label();
                    LocationLabel.SuspendLayout();
                    LocationLabel.Font = new Font("IM FELL English", 12);
                    LocationLabel.Margin = new Padding(0, 0, 0, 0);
                    LocationLabel.Dock = DockStyle.Top;
                    LocationLabel.Text = QuestionsAndAnswers[i];
                    SeriesPanel.Controls.Add(LocationLabel, i, SeriesPanel.RowCount);
                    LocationLabel.ResumeLayout(false);
                }
                else
                {
                    ComboBox PossibleAnswersBox = new ComboBox();
                    PossibleAnswersBox.SuspendLayout();
                    PossibleAnswersBox.Font = new Font("IM FELL English", 12);
                    PossibleAnswersBox.Margin = new Padding(0, 0, 0, 0);
                    PossibleAnswersBox.Dock = DockStyle.Top;
                    string[] PossibleAnswers = QuestionsAndAnswers[i].Split(new char[] { '?' });

                    foreach (var item in PossibleAnswers)
                    {
                        PossibleAnswersBox.Items.Add(item);
                    }

                    PossibleAnswersBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    SeriesPanel.Controls.Add(PossibleAnswersBox, i, SeriesPanel.RowCount);
                    PossibleAnswersBox.ResumeLayout(false);
                }
            }
            SeriesPanel.RowCount = SeriesPanel.Controls.Count;
            SeriesPanel.ResumeLayout(true);         
        }
    }
}

