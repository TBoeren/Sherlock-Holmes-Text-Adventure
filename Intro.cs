using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sherlock_Holmes_Text_Adventure
{
    internal class Intro
    {
        public Intro(Label IntroLabel)
        {
            string IntroText = System.IO.File.ReadAllText("D:/Thijs Huiswerk/Sherlock Holmes Text Adventures/Sherlock Holmes Text Adventure/Resources/Intro.txt", Encoding.Default);
            IntroLabel.Text = IntroText;
        }
    }
}
