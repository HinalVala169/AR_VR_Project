using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionsData", menuName = "ScriptableObjects/QuestionsData", order = 1)]
public class QuestionsData : ScriptableObject
{

    public TextAsset Questions;

    // Expose the list directly in the inspector
    public List<QuestionData> questions = new List<QuestionData>();

    // This method will be called when the object is modified in the Inspector
    private void OnValidate()
    {
        if (Questions != null)
        {
            LoadFromJson(Questions.text);
        }
    }

    // Method to deserialize JSON data
    public void LoadFromJson(string json)
    {
        // Load the questions from the JSON string
        QuestionList temp = JsonUtility.FromJson<QuestionList>(json);
        questions = temp.questions;
    }
}

[System.Serializable]
public class QuestionData
{
    public string question;
    public bool answer;
}


[System.Serializable]
public class QuestionList
{
    public List<QuestionData> questions;
}
