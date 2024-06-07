using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class QuizUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _quizText,_quizIDText,_resultText;
    [SerializeField] private GameObject _questionPanel, _resultPanel;
    [SerializeField] private Image _correctCircleImage;
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private GameManager _gameManager;

    private void Start()
    {
        _correctCircleImage.fillAmount = 0;
    }

    /// <summary>
    /// 質問パネルを開く
    /// </summary>
    public void OpenQuestionPanel()
    {
        _questionPanel.SetActive(true);
        switch (_gameManager.CurrentQuizID)
        {
            case 0:
                _quizIDText.text = "Q1";
                _quizText.text = "外出好きな「つつつつつつつつつ」\nってなに?";
                break;
            case 1:
                _quizIDText.text = "Q2";
                _quizText.text = "呼んでもこない「ぱそ」ってなに?";
                break;
            case 2:
                _quizIDText.text = "Q3";
                _quizText.text = "足があってかけることができるのに、\n歩けないものなに?";
                break;
        }
    }
    /// <summary>
    /// 質問パネルを閉じる
    /// </summary>
    public void CloseQuestionPanel()
    {
        _questionPanel.SetActive(false);
    }
    /// <summary>
    /// リザルトパネルを開く
    /// </summary>
    public void OpenResultPanel()
    {
        switch (_gameManager.CurrentQuizID)
        {
            case 0:
                _resultText.text = "「くつ」";
                break;
            case 1:
                _resultText.text = "「パソコン」";
                break;
            case 2:
                _resultText.text = "「いす」";
                break;
        }
        _resultPanel.SetActive(true);
        PlayResultAnimation();
    }
    /// <summary>
    /// 正解アニメーション
    /// </summary>
    private void PlayResultAnimation()
    {
        _correctCircleImage.DOFillAmount(1, 0.5f).
            OnComplete(() =>
            {
                DOVirtual.DelayedCall(3, () =>
                {
                    _gameManager.GoNextQuiz();
                    _resultPanel.SetActive(false);
                    OpenQuestionPanel();
                    _correctCircleImage.fillAmount = 0;
                });
            });
    }
}
