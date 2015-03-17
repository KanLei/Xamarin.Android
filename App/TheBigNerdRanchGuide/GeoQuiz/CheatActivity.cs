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
    [Activity(Label = "CheatActivity")]
    public class CheatActivity : Activity
    {
        public const string EXTRA_ANSWER_IS_TRUE = "GeoQuiz.answer_is_true";
        public const string EXTRA_ANSWER_SHOWN = "GeoQuiz.answer_shown";
        private const string KEY_SHOW_ANSWER = "show_answer";

        private Button showAnswerButton;
        private TextView answerTextView;
        private TextView apiLevelTextView;

        private bool answerIsTrue;
        private bool isShowAnswer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_cheat);

            answerIsTrue = Intent.GetBooleanExtra(EXTRA_ANSWER_IS_TRUE, false);

            answerTextView = FindViewById<TextView>(Resource.Id.answerTextView);

            showAnswerButton = FindViewById<Button>(Resource.Id.showAnswerButton);
            showAnswerButton.Click += (s, e) =>
            {
                isShowAnswer = true;
                ShowAnswer();
            };

            apiLevelTextView = FindViewById<TextView>(Resource.Id.api_level_textview);
            apiLevelTextView.Text = "API " + Build.VERSION.Sdk;

            if (bundle != null)
            {
                isShowAnswer = bundle.GetBoolean(KEY_SHOW_ANSWER, false);
                if (isShowAnswer)
                {
                    ShowAnswer();
                }
            }
        }

        private void ShowAnswer()
        {
            answerTextView.SetText(answerIsTrue ? Resource.String.true_button : Resource.String.false_button);

            var intent = new Intent();
            intent.PutExtra(EXTRA_ANSWER_SHOWN, true);
            SetResult(Result.Ok, intent);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutBoolean(KEY_SHOW_ANSWER, isShowAnswer);
        }
    }
}