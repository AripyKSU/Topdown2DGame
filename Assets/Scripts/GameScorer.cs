using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameScorer : MonoBehaviour
{
    public static GameScorer I { get; private set; }

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    public int Score => score;

    private void Awake()
    {
        I = this;
        SetupScoreUI();
    }

    public void AddMoney()
    {
        score += 1000;
    }

    public void OnGameOver()
    {
        score = 0;
    }

    public void Update()
    {
        UpdateScoreDisplay();
    }
    
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = string.Format("LOOT  {0}", GetFormattedScore());
        }
    }

    private void SetupScoreUI()
    {
        if (scoreText == null || scoreText.transform.parent.Find("ScoreCard") != null) return;

        Transform canvasTransform = scoreText.canvas.transform;
        GameObject cardObject = new GameObject("ScoreCard", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Outline));
        cardObject.transform.SetParent(canvasTransform, false);

        RectTransform cardRect = cardObject.GetComponent<RectTransform>();
        cardRect.anchorMin = new Vector2(1f, 1f);
        cardRect.anchorMax = new Vector2(1f, 1f);
        cardRect.pivot = new Vector2(1f, 1f);
        cardRect.anchoredPosition = new Vector2(-34f, -34f);
        cardRect.sizeDelta = new Vector2(350f, 86f);

        Image background = cardObject.GetComponent<Image>();
        background.color = new Color32(20, 23, 25, 232);
        background.raycastTarget = false;

        Outline outline = cardObject.GetComponent<Outline>();
        outline.effectColor = new Color32(65, 214, 92, 220);
        outline.effectDistance = new Vector2(3f, -3f);

        scoreText.transform.SetParent(cardObject.transform, false);
        RectTransform textRect = scoreText.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(20f, 8f);
        textRect.offsetMax = new Vector2(-20f, -8f);

        scoreText.fontSize = 34f;
        scoreText.fontStyle = FontStyles.Bold;
        scoreText.alignment = TextAlignmentOptions.Center;
        scoreText.color = new Color32(255, 211, 72, 255);
        scoreText.raycastTarget = false;
    }

    public string GetFormattedScore()
    {
        return FormatScore(score);
    }

    public void Reset()
    {
        score = 0;
        UpdateScoreDisplay();
    }

    public static string FormatScore(int score)
    {
        return string.Format("{0}", score);
    }
}
