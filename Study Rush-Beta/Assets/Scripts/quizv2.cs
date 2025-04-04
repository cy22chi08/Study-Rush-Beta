using System.Collections;
using UnityEngine;
using TMPro;

public class quizV2 : MonoBehaviour
{
    public GameObject[] Quiz_Number;
    int currentLevel;

    public TextMeshProUGUI scoreText;
    int scorePercentage;

    public TextMeshProUGUI countdownText; // Reference to a UI Text element
    public float countdownTime = 10f;

    public GameObject resultPanel; // The GameObject to show when time runs out or quiz ends
    public TextMeshProUGUI resultScoreText; // Reference to display result score in the result panel

    private bool timerStarted = false; // To track if the timer has started



    public float speedChase;



    void Awake()
{
    DontDestroyOnLoad(gameObject);
}
    void Start()
    {
        scorePercentage = 0; // Initialize score percentage to 0
        UpdateScoreText(); // Update the UI with the initial score
        
        resultPanel.SetActive(false); // Hide the result panel at the start
        countdownText.text = countdownTime.ToString(); // Display the initial countdown time
    }

    public void StartTimer()
    {
        timerStarted = true; // Set the timer to start
    }

    public void CorrectAnswer()
    {
        StartCoroutine(HandleAnswer(true)); // Start the coroutine for the correct answer
    }

    public void WrongAnswer()
    {
        StartCoroutine(HandleAnswer(false)); // Start the coroutine for the wrong answer
    }

    private IEnumerator HandleAnswer(bool isCorrect)
    {
        // Wait for the delay time before continuing
        yield return new WaitForSeconds(0.25f);

        // Process the answer after the delay
        if (isCorrect)
        {
            if (currentLevel + 1 < Quiz_Number.Length)
            {
                Quiz_Number[currentLevel].SetActive(false);
                currentLevel++;
                Quiz_Number[currentLevel].SetActive(true);

                // Add 10% to score for correct answer
                scorePercentage += 10;
                if (scorePercentage > 99) scorePercentage = 99; // Ensure score doesn't exceed 100%

                UpdateScoreText(); // Update the displayed score
            }
        }
        else
        {
            if (currentLevel + 1 < Quiz_Number.Length)
            {
                Quiz_Number[currentLevel].SetActive(false);
                currentLevel++;
                Quiz_Number[currentLevel].SetActive(true);

                countdownTime -= 10f;
                if (countdownTime < 0f) countdownTime = 0f;
            }
        }

        // If it's the last question, show the result panel
        if (currentLevel == Quiz_Number.Length - 1)
        {
            ShowResult();
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + scorePercentage + "%"; // Update the score text UI element
    }

    void Update()
    {
        if (timerStarted)
        {
            countdownText.text = Mathf.CeilToInt(countdownTime).ToString(); // Update text

            if (countdownTime <= 0f)
            {
                Debug.Log("Time's up!");
                countdownTime = 0f; // Ensure it doesn't go negative
                ShowResult(); // Show the result panel when time's up
            }
            else
            {
                countdownTime -= Time.deltaTime; // Reduce time
            }
        }

        Chase();
    }

    // Simple function to show the result panel and deactivate quiz questions
    void ShowResult()
    {
        int scorePercentageFinal;
        scorePercentageFinal = scorePercentage; 

        resultPanel.SetActive(true); // Set the result panel to active

        // Deactivate all quiz questions
        foreach (GameObject questions in Quiz_Number)
        {
            Destroy(questions);
        }

        // Destroy the score and countdown texts instantly
        Color color = scoreText.color;
        color.a = 0;
        scoreText.color = color;
        Destroy(countdownText.gameObject);

        // Display the final score in the result panel
        resultScoreText.text = scorePercentageFinal + "% / 30%";
    }


    public void Chase()
    {


        if(scorePercentage <= 30)
        {
            speedChase = 3f;
        }
        else{
            speedChase = 0f;
        }

    }

}
