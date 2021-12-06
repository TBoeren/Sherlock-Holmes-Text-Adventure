using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class Intro
    {
        public Intro(Label IntroLabel, Label CaseTitle)
        {
            //Get the intro text from the resources folder and print it on the introtab
            ExternalFileManager FileManager = new ExternalFileManager();
            IntroLabel.Text = FileManager.ReadTextFile("Intro.txt");
            CaseTitle.Text = FileManager.ReadTextFile("CaseTitle.txt");
        }
    }
}