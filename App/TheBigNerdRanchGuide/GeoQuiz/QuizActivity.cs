using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Annotation;

namespace GeoQuiz
{
    [Activity(Label = "GeoQuiz", MainLauncher = true, Icon = "@drawable/icon")]
    public class QuizActivity : Activity
    {
        private const string KEY_INDEX = "index";

        private Button trueButton;
        private Button falseButton;
        private Button cheatButton;
        private ImageButton previousButton;
        private ImageButton nextButton;
        private TextView questionTextView;

        private static TrueFalse[] questionBank = new TrueFalse[]{
            new TrueFalse(Resource.String.question_oceans, true),
            new TrueFalse(Resource.String.question_mideast,false),
            new TrueFalse(Resource.String.question_africa,false),
            new TrueFalse(Resource.String.question_americas,true),
            new TrueFalse(Resource.String.question_asia,true)
        };

        private int currentIndex = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_quiz);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
                ActionBar.Subtitle = "guess a question...";


            questionTextView = FindViewById<TextView>(Resource.Id.txtQuestion);


            trueButton = FindViewById<Button>(Resource.Id.true_button);
            trueButton.Click += (s, e) =>
            {
                CheckAnswer(true);
            };

            falseButton = FindViewById<Button>(Resource.Id.false_button);
            falseButton.Click += (s, e) =>
            {
                CheckAnswer(false);
            };

            nextButton = FindViewById<ImageButton>(Resource.Id.next_button);
            nextButton.Click += (s, e) =>
            {
                currentIndex = (currentIndex + 1) % questionBank.Length;
                UpdateQuestion();
            };

            previousButton = FindViewById<ImageButton>(Resource.Id.previous_button);
            previousButton.Click += (s, e) =>
            {
                currentIndex = (currentIndex + (questionBank.Length - 1)) % questionBank.Length;
                UpdateQuestion();
            };

            cheatButton = FindViewById<Button>(Resource.Id.cheat_button);
            cheatButton.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(CheatActivity));
                intent.PutExtra(CheatActivity.EXTRA_ANSWER_IS_TRUE, questionBank[currentIndex].IsTrueQuestion);
                //StartActivity(intent);
                StartActivityForResult(intent, 0);
            };

            if (bundle != null)
            {
                currentIndex = bundle.GetInt(KEY_INDEX);
            }

            UpdateQuestion();
        }

        private void UpdateQuestion()
        {
            questionTextView.SetText(questionBank[currentIndex].Question);
        }

        private void CheckAnswer(bool answer)
        {
            int messageId;
            if (questionBank[currentIndex].IsCheater)
                messageId = Resource.String.judgment_toast;
            else
                messageId = questionBank[currentIndex].IsTrueQuestion == answer ? Resource.String.correct_toast : Resource.String.incorrect_toast;
            Toast.MakeText(this, messageId, ToastLength.Short).Show();
        }

        // called by system before OnPause()
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt(KEY_INDEX, currentIndex);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 0 && resultCode == Result.Ok && data != null)
            {
                bool isCheater = data.GetBooleanExtra(CheatActivity.EXTRA_ANSWER_SHOWN, false);
                questionBank[currentIndex].IsCheater = isCheater;
            }
        }

    }
}

