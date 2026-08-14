using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "SampleScene";

    private void Start()
    {
        CreateLogo();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private void CreateLogo()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null || canvas.transform.Find("BusyThiefLogo") != null) return;

        GameObject logoObject = new GameObject("BusyThiefLogo", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI), typeof(Shadow));
        logoObject.transform.SetParent(canvas.transform, false);

        RectTransform rect = logoObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -54f);
        rect.sizeDelta = new Vector2(1000f, 180f);

        TextMeshProUGUI logo = logoObject.GetComponent<TextMeshProUGUI>();
        logo.font = TMP_Settings.defaultFontAsset;
        logo.text = "<color=#FFD447>Busy</color><color=#41D65C>Thief</color>";
        logo.fontSize = 106f;
        logo.fontStyle = FontStyles.Bold;
        logo.alignment = TextAlignmentOptions.Center;
        logo.raycastTarget = false;
        logo.enableAutoSizing = true;
        logo.fontSizeMin = 72f;
        logo.fontSizeMax = 106f;

        Material logoMaterial = logo.fontMaterial;
        if (logoMaterial != null)
        {
            logoMaterial.SetColor(ShaderUtilities.ID_OutlineColor, new Color32(10, 13, 15, 255));
            logoMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.18f);
        }

        Shadow shadow = logoObject.GetComponent<Shadow>();
        shadow.effectColor = new Color(0f, 0f, 0f, 0.8f);
        shadow.effectDistance = new Vector2(7f, -7f);
    }
}
