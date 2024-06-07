using Niantic.Lightship.AR.ObjectDetection;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectDetectionSample : MonoBehaviour
{
    [SerializeField]
    private float _probabilityThreshold = 0.5f;
    [SerializeField]
    private ARObjectDetectionManager _objectDetectionManager;
    private Color[] _colors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.cyan,
        Color.white,
        Color.black
    };
    [SerializeField]
    private DrawRect _drawRect;
    private Canvas _canvas;
    private bool _isSearch;
    private string _targetObjName;
    [SerializeField] private QuizUIManager _uiManager;

    public RawImage targetTexture; // キャプチャ結果を格納するテクスチャ
    Texture2D capturedTexture;

    private void Awake()
    {
        _canvas = FindObjectOfType<Canvas>();
    }

    public void Start()
    {
        _objectDetectionManager.enabled = true;
        _objectDetectionManager.MetadataInitialized += OnMetadataInitialized;
    }

    private void OnMetadataInitialized(ARObjectDetectionModelEventArgs args)
    {
        _objectDetectionManager.ObjectDetectionsUpdated += ObjectDetectionsUpdated;
    }

    /// <summary>
    /// 探索開始
    /// </summary>
    /// <param name="targetName"></param>
    public void StartSearch(string targetName)
    {
        _isSearch = true;
        _targetObjName = targetName;
    }

    /// <summary>
    /// ターゲット発見
    /// </summary>
    public void FindTarget()
    {
        _isSearch = false;
        ScreenCapture();
    }

    private void ObjectDetectionsUpdated(ARObjectDetectionsUpdatedEventArgs args)
    {
        if (!_isSearch) return;
        string resultString = "";
        float _confidence = 0;
        string _name = "";
        var result = args.Results;
        if (result == null)
        {
            return;
        }
        _drawRect.ClearRects();
        for (int i = 0; i < result.Count; i++)
        {
            var detection = result[i];
            var categorizations = detection.GetConfidentCategorizations(_probabilityThreshold);
            if (categorizations.Count <= 0)
            {
                break;
            }

            categorizations.Sort((a, b) => b.Confidence.CompareTo(a.Confidence));
            var categoryToDisplay = categorizations[0];
            _confidence = categoryToDisplay.Confidence;
            _name = categoryToDisplay.CategoryName;

            int h = Mathf.FloorToInt(_canvas.GetComponent<RectTransform>().rect.height);
            int w = Mathf.FloorToInt(_canvas.GetComponent<RectTransform>().rect.width);

            //Get the rect around the detected object
            var _rect = result[i].CalculateRect(w, h, Screen.orientation);

            //resultString = $"{_name}: {_confidence}\n";
            resultString = $"{_name}";
            //Draw the Rect.
            _drawRect.CreateRect(_rect, _colors[i % _colors.Length], resultString);

            if (categoryToDisplay.CategoryName == _targetObjName)
            {
                if (categoryToDisplay.Confidence > 0.55f)
                {
                    _drawRect.ClearRects();
                    _drawRect.CreateRect(_rect, _colors[i % _colors.Length], resultString);
                    FindTarget();
                    break;
                }
            }
        }
    }

    public void ScreenCapture()
    {
        // カメラのレンダリングテクスチャをキャプチャする
        capturedTexture = new Texture2D(Screen.width, Screen.height);
        StartCoroutine(UpdateCurrentScreenShot());
    }

    private IEnumerator UpdateCurrentScreenShot()
    {
        // これがないとReadPixels()でエラーになる
        yield return new WaitForEndOfFrame();
        capturedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        capturedTexture.Apply();
        targetTexture.texture = capturedTexture;
        ConvertBase64FromTexture();
        _uiManager.OpenResultPanel();
    }

    private void ConvertBase64FromTexture()
    {
        //Texture->byte変換
        byte[] byte_Before = capturedTexture.EncodeToJPG();
        //BASE64への変換
        string encodedText
            = System.Convert.ToBase64String(byte_Before);
        Debug.Log(encodedText);
    }

    private void OnDestroy()
    {
        _objectDetectionManager.MetadataInitialized -= OnMetadataInitialized;
        _objectDetectionManager.ObjectDetectionsUpdated -= ObjectDetectionsUpdated;
    }
}