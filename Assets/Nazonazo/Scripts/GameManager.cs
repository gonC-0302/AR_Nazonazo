using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private enum AnswerObjectName
    {
        footwear,
        laptop,
        chair
    }

    public int CurrentQuizID => _currentQuizID;
    [SerializeField] private QuizUIManager _uiManager;
    [SerializeField] Button _btnStartSearch;
    [SerializeField] private ObjectDetectionSample _objDetection;
    [SerializeField] private SceneTransitioner _sceneTransitioner;
    private int _currentQuizID = 0;
    private int _maxQuizNum = 3;
   
    private void Start()
    {
        _btnStartSearch.onClick.AddListener(OnClickStartButton);
        _uiManager.OpenQuestionPanel();
    }

    /// <summary>
    /// 探索開始
    /// </summary>
    public void OnClickStartButton()
    {
        var targetName = Enum.GetName(typeof(AnswerObjectName), _currentQuizID);
        _objDetection.StartSearch(targetName.ToString());
        _uiManager.CloseQuestionPanel();
    }

    /// <summary>
    /// 全てのクイズが終わったらタイトルへ戻る
    /// </summary>
    public void GoNextQuiz()
    {
        _currentQuizID++;
        if(_currentQuizID >= _maxQuizNum)
        {
            _sceneTransitioner.ToTitle();
        }
    }
}
