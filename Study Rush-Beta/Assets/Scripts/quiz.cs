using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MathQuiz : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_Text[] answerTexts; // Store text inside buttons
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public float timeLimit = 30f;

    private int correctAnswer;
    private float timer;
    private int score = 0;
    private List<int> remainingQuestions = new List<int>();

    private string[] questions = {
        "5 + 3 = ?", "10 - 7 = ?", "4 × 6 = ?", "12 ÷ 3 = ?", "15 + 9 = ?"
    };

    private int[] correctAnswers = { 8, 3, 24, 4, 24 };

    private int[,] answerChoices = {
        { 8, 5, 10, 7 },   // Choices for question 1
        { 2, 3, 5, 6 },    // Choices for question 2
        { 20, 24, 26, 18 },// Choices for question 3
        { 3, 4, 5, 6 },    // Choices for question 4
        { 23, 24, 22, 21 } // Choices for question 5
    };

    void Start()
    {
        timer = timeLimit;
        
        for (int i = 0; i < questions.Length; i++)
            remainingQuestions.Add(i);
        
        ShuffleQuestions();
        GenerateQuestion();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

        if (timer <= 0)
            EndQuiz();
    }

    void GenerateQuestion()
    {
        if (remainingQuestions.Count == 0)
        {
            EndQuiz();
            return;
        }

        int questionIndex = remainingQuestions[0];
        remainingQuestions.RemoveAt(0);

        correctAnswer = correctAnswers[questionIndex];
        questionText.text = questions[questionIndex];

        for (int i = 0; i < answerTexts.Length; i++)
            answerTexts[i].text = answerChoices[questionIndex, i].ToString();
    }

    public void CheckAnswer(int index)
    {
        int selectedAnswer = int.Parse(answerTexts[index].text);

        if (selectedAnswer == correctAnswer)
        {
            score += 10;
            scoreText.text = "Score: " + score;
        }
        else
        {
            timer -= 2f;
        }

        GenerateQuestion();
    }

    void EndQuiz()
    {
        Debug.Log("Quiz Over! Final Score: " + score);
    }

    void ShuffleQuestions()
    {
        for (int i = 0; i < remainingQuestions.Count; i++)
        {
            int randomIndex = Random.Range(0, remainingQuestions.Count);
            int temp = remainingQuestions[i];
            remainingQuestions[i] = remainingQuestions[randomIndex];
            remainingQuestions[randomIndex] = temp;
        }
    }
}