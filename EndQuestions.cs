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
        private Notes CaseNotes;
        private Label PointsEarnedLabel;

        private DataTable QuestionDatatable;
        private List<string> AllAnswers = new List<string>();
        private string OutroText;
        private string LeadsFollowedText;
        private int LeadsFollowed;
        private List<ComboBox> AllComboBoxes = new List<ComboBox>();
        private List<Label> AllQuestions = new List<Label>();
        private List<int> PossiblePoints = new List<int>();

        public EndQuestions(DBLayoutPanel Series1, DBLayoutPanel Series2, DBLayoutPanel Answers1, DBLayoutPanel Answers2, Notes EndNotes, Label PointsLabel)
        {
            FirstSeriesPanel = Series1;
            SecondSeriesPanel = Series2;
            FirstSeriesAnswersPanel = Answers1;
            SecondSeriesAnswersPanel = Answers2;
            CaseNotes = EndNotes;
            PointsEarnedLabel = PointsLabel;

            ExternalFileManager FileManager = new ExternalFileManager();
            QuestionDatatable = FileManager.ConvertCSVToDatatable("SherlockQuestions.csv");
            SetLeadsFollowed(FileManager.ReadTextFile("Outro.txt"));
        }

        void SetLeadsFollowed(string AllText)
        {
            string[] OutroAndLeads = AllText.Split('@');
            OutroText = OutroAndLeads[0];
            LeadsFollowedText = OutroAndLeads[1];
            LeadsFollowed = int.Parse(OutroAndLeads[2]);
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
                    PossiblePoints.Add(int.Parse(Row["points"].ToString()));
                }
                else
                {
                    SecondSeriesQuestionList.Add(Row["question"].ToString());
                    SecondSeriesQuestionList.Add(Row["possible answers"].ToString());
                    PossiblePoints.Add(int.Parse(Row["points"].ToString()));
                }

                AllAnswers.Add(Row["answer"].ToString());
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
                    AllQuestions.Add(LocationLabel);
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
                    AllComboBoxes.Add(PossibleAnswersBox);
                }
            }
            SeriesPanel.RowCount = SeriesPanel.Controls.Count;
            SeriesPanel.ResumeLayout(true);
        }

        public void SubmitAnswers()
        {
            TravelLocation Outro = new TravelLocation(OutroText, LeadsFollowedText);
            Outro.Show();
            Outro.FormClosed += ShowAnswers;
            Outro.FormClosed += CalculateScore;
        }

        private void CalculateScore(object sender, FormClosedEventArgs e)
        {
            int TotalPointsEarned = 0;

            for (int i = 0; i < PossiblePoints.Count; i++)
            {
                if (AllAnswers[i] == AllComboBoxes[i].Text)
                {
                    TotalPointsEarned += PossiblePoints[i];
                    AllQuestions[i].ForeColor = Color.Green;
                }
                else
                {
                    AllQuestions[i].ForeColor = Color.Red;
                }
            }

            foreach (var item in AllComboBoxes)
            {
                item.Enabled = false;
            }

            if(CaseNotes.TotalLeadsFollowed > LeadsFollowed)
            {
                int x = CaseNotes.TotalLeadsFollowed - LeadsFollowed;
                x = x * 5;
                TotalPointsEarned -= x;
            }

            PointsEarnedLabel.Text = "You earned: " + TotalPointsEarned + "points!";
        }

        private void ShowAnswers(object sender, FormClosedEventArgs e)
        {
            FirstSeriesAnswersPanel.SuspendLayout();
            SecondSeriesAnswersPanel.SuspendLayout();

            //Print first series answers
            for (int i = 0; i < (FirstSeriesPanel.RowCount / 2); i++)
            {
                Label AnswerLabel = new Label();
                AnswerLabel.SuspendLayout();
                AnswerLabel.Font = new Font("IM FELL English", 12);
                AnswerLabel.Margin = new Padding(0, 3, 0, 3);
                AnswerLabel.Dock = DockStyle.Bottom;
                AnswerLabel.Text = AllAnswers[i];
                FirstSeriesAnswersPanel.Controls.Add(AnswerLabel, 0, i);
                AnswerLabel.ResumeLayout(false);
            }

            for (int i = 0; i < (SecondSeriesPanel.RowCount / 2); i++)
            {
                Label AnswerLabel = new Label();
                AnswerLabel.SuspendLayout();
                AnswerLabel.Font = new Font("IM FELL English", 12);
                AnswerLabel.Margin = new Padding(0, 3, 0, 3);
                AnswerLabel.Dock = DockStyle.Bottom;
                AnswerLabel.Text = AllAnswers[(FirstSeriesPanel.RowCount / 2) + i];
                SecondSeriesAnswersPanel.Controls.Add(AnswerLabel, 0, i);
                AnswerLabel.ResumeLayout(false);
            }

            FirstSeriesAnswersPanel.RowCount = FirstSeriesAnswersPanel.Controls.Count;
            SecondSeriesAnswersPanel.RowCount = SecondSeriesAnswersPanel.Controls.Count;
            FirstSeriesAnswersPanel.ResumeLayout(true);
            SecondSeriesAnswersPanel.ResumeLayout(true);
        }
    }
}

