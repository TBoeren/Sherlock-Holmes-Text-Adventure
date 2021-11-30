﻿using System;
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
            //Get the intro text from the resources folder and print it on the introtab
            ExternalFileManager FileManager = new ExternalFileManager();
            IntroLabel.Text = FileManager.ReadTextFile("Intro.txt");
        }
    }
}