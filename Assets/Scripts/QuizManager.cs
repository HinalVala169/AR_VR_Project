using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Required for TextMesh Pro

public class QuizManager : MonoBehaviour
{
    [Header("Quiz Configuration")]
    public QuestionsData questionsData;    // Reference to the ScriptableObject
    public TMP_Text questionText;           // TMP_Text element to display the question
    public TMP_Text feedbackText;           // TMP_Text element to show feedback
    public TMP_Text scoreText;              // TMP_Text element to display score

    private List<QuestionData> questions;   // List of questions
    private QuestionData currentQuestion;    // Current question
    private int score;                       // Player's score

    private void Start()
    {
        if (questionsData != null)
        {
            questions = questionsData.questions;
            score = 0;
            UpdateScoreText();
            GetRandomQuestion();
        }
        else
        {
            Debug.LogError("QuestionsData is not assigned in the QuizManager.");

            questionText.text = "QuestionsData is not assigned in the QuizManager.";
        }
    }

    // Method to get a random question from the questions list
    public void GetRandomQuestion()
    {
        if (questions == null || questions.Count == 0)
        {
            Debug.LogError("No questions available.");
            return;
        }

        int randomIndex = Random.Range(0, questions.Count);
        currentQuestion = questions[randomIndex];
        questionText.text = currentQuestion.question; // Update TMP text
      //  feedbackText.text = ""; // Clear previous feedback
        Debug.Log("Current Question: " + currentQuestion.question);
    }

    // Method to check if the answer is true
    public void AnswerTrue()
    {
        CheckAnswer(true);
    }

    // Method to check if the answer is false
    public void AnswerFalse()
    {
        CheckAnswer(false);
    }

    // Method to check the answer and update score
    private void CheckAnswer(bool answer)
    {
        if (currentQuestion != null)
        {
            bool isCorrect = currentQuestion.answer == answer;
            PrintResult(isCorrect);
            UpdateScore(isCorrect);
            GetRandomQuestion(); // Get a new question after answering
        }
        else
        {
            Debug.LogWarning("No question has been retrieved. Please call GetRandomQuestion first.");
        }
    }

    // Method to update and display the score
    private void UpdateScore(bool isCorrect)
    {
        if (isCorrect)
        {
            score++;
        }
        UpdateScoreText();
    }

    // Method to update the score text in the UI
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Method to print whether the answer was correct or not
    private void PrintResult(bool isCorrect)
    {
        feedbackText.text = isCorrect ? "Well Done!" : "Try Again";
    }
}
