using System;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class PortfolioUISetup
{
    private const string ScenePath = "Assets/Scenes/SampleScene.unity";

    [MenuItem("Tools/Portfolio/Apply Web Resolution And UI")]
    public static void Apply()
    {
        ConfigureResolution();
        ConfigureInputHandling();

        Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        ConfigureCanvas(scene);
        ConfigureRankingPanel(scene);
        ConfigureRestartButton(scene);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log("[PortfolioUISetup] Applied 1920x1080 Web settings and refreshed the game-over UI.");
    }

    [MenuItem("Tools/Portfolio/Enable Both Input Systems")]
    public static void ConfigureInputHandling()
    {
        UnityEngine.Object[] settingsAssets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset");
        if (settingsAssets.Length == 0)
            throw new InvalidOperationException("PlayerSettings asset was not found.");

        SerializedObject playerSettings = new SerializedObject(settingsAssets[0]);
        SerializedProperty activeInputHandler = playerSettings.FindProperty("activeInputHandler");
        if (activeInputHandler == null)
            throw new InvalidOperationException("activeInputHandler setting was not found.");

        // 0: Input Manager, 1: Input System Package, 2: Both
        activeInputHandler.intValue = 2;
        playerSettings.ApplyModifiedPropertiesWithoutUndo();
        AssetDatabase.SaveAssets();
        Debug.Log("[PortfolioUISetup] Active Input Handling set to Both.");
    }

    private static void ConfigureResolution()
    {
        PlayerSettings.defaultScreenWidth = 1920;
        PlayerSettings.defaultScreenHeight = 1080;
#pragma warning disable 618
        PlayerSettings.defaultWebScreenWidth = 1920;
        PlayerSettings.defaultWebScreenHeight = 1080;
#pragma warning restore 618
        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
    }

    private static void ConfigureCanvas(Scene scene)
    {
        GameObject canvasObject = FindInScene(scene, "Canvas");
        if (canvasObject == null) throw new InvalidOperationException("Canvas was not found.");

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        if (scaler == null) scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        scaler.referencePixelsPerUnit = 100f;
    }

    private static void ConfigureRankingPanel(Scene scene)
    {
        GameObject panelObject = FindInScene(scene, "HighScorePanel");
        if (panelObject == null) throw new InvalidOperationException("HighScorePanel was not found.");

        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(620f, 600f);

        Image panelImage = panelObject.GetComponent<Image>();
        panelImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        panelImage.type = Image.Type.Sliced;
        panelImage.color = new Color32(20, 23, 25, 248);

        Outline outline = panelObject.GetComponent<Outline>();
        if (outline == null) outline = panelObject.AddComponent<Outline>();
        outline.effectColor = new Color32(65, 214, 92, 255);
        outline.effectDistance = new Vector2(3f, -3f);
        outline.useGraphicAlpha = true;

        TextMeshProUGUI title = FindText(panelObject, "ScoreBoardText");
        if (title != null)
        {
            SetRect(title.rectTransform, new Vector2(0f, 222f), new Vector2(520f, 72f));
            title.text = "HIGH SCORES";
            title.fontSize = 42f;
            title.fontStyle = FontStyles.Bold;
            title.color = new Color32(88, 232, 111, 255);
            title.alignment = TextAlignmentOptions.Center;
        }

        string[] rankNames = { "RankingText", "RankingText (1)", "RankingText (2)", "RankingText (3)", "RankingText (4)" };
        for (int i = 0; i < rankNames.Length; i++)
        {
            TextMeshProUGUI rank = FindText(panelObject, rankNames[i]);
            if (rank == null) continue;

            SetRect(rank.rectTransform, new Vector2(0f, 130f - i * 76f), new Vector2(460f, 54f));
            rank.fontSize = i == 0 ? 32f : 29f;
            rank.fontStyle = i == 0 ? FontStyles.Bold : FontStyles.Normal;
            rank.color = i == 0 ? new Color32(255, 211, 72, 255) : new Color32(226, 231, 228, 255);
            rank.alignment = TextAlignmentOptions.Center;
            rank.raycastTarget = false;
        }

        AddOrStyleDivider(panelObject.transform, "TopDivider", new Vector2(0f, 184f));
        AddOrStyleDivider(panelObject.transform, "BottomDivider", new Vector2(0f, -260f));
    }

    private static void ConfigureRestartButton(Scene scene)
    {
        GameObject buttonObject = FindInScene(scene, "RestartButton");
        if (buttonObject == null) throw new InvalidOperationException("RestartButton was not found.");

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, -352f);
        rect.sizeDelta = new Vector2(300f, 68f);

        Image image = buttonObject.GetComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        image.type = Image.Type.Sliced;
        image.color = new Color32(65, 214, 92, 255);

        Button button = buttonObject.GetComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color32(190, 255, 199, 255);
        colors.pressedColor = new Color32(39, 164, 61, 255);
        colors.selectedColor = colors.highlightedColor;
        colors.disabledColor = new Color32(80, 88, 82, 160);
        colors.fadeDuration = 0.08f;
        button.colors = colors;

        TextMeshProUGUI label = buttonObject.GetComponentInChildren<TextMeshProUGUI>(true);
        if (label != null)
        {
            label.text = "RESTART";
            label.fontSize = 30f;
            label.fontStyle = FontStyles.Bold;
            label.color = new Color32(17, 25, 19, 255);
            label.alignment = TextAlignmentOptions.Center;
            label.raycastTarget = false;
        }
    }

    private static void AddOrStyleDivider(Transform parent, string objectName, Vector2 position)
    {
        Transform existing = parent.Find(objectName);
        GameObject divider = existing != null ? existing.gameObject : new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        if (existing == null) divider.transform.SetParent(parent, false);

        RectTransform rect = divider.GetComponent<RectTransform>();
        SetRect(rect, position, new Vector2(460f, 2f));

        Image image = divider.GetComponent<Image>();
        image.color = new Color32(65, 214, 92, 130);
        image.raycastTarget = false;
    }

    private static void SetRect(RectTransform rect, Vector2 position, Vector2 size)
    {
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        rect.localScale = Vector3.one;
    }

    private static TextMeshProUGUI FindText(GameObject root, string objectName)
    {
        foreach (TextMeshProUGUI text in root.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (text.gameObject.name == objectName) return text;
        }
        return null;
    }

    private static GameObject FindInScene(Scene scene, string objectName)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Transform[] transforms = root.GetComponentsInChildren<Transform>(true);
            foreach (Transform candidate in transforms)
            {
                if (candidate.gameObject.name == objectName) return candidate.gameObject;
            }
        }
        return null;
    }
}
