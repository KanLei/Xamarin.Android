using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GeoQuiz
{
    public class TrueFalse
    {
        public int Question { get; set; }
        public bool IsTrueQuestion { get; set; }
        public bool IsCheater { get; set; }

        public TrueFalse(int question, bool trueQuestion, bool isCheater = false)
        {
            Question = question;
            IsTrueQuestion = trueQuestion;
            IsCheater = isCheater;
        }
    }
}