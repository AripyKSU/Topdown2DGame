using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class MainMenuSceneBuilder
{
    private const string MainMenuPath = "Assets/Scenes/MainMenu.unity";
    private const string GameScenePath = "Assets/Scenes/SampleScene.unity";
    private const string ThumbnailPath = "Assets/Arts/PortfolioThumbnail.png";
    private const string BgmPath = "Assets/retro-bgm-chan-spy-523660.mp3";
    private const string MoneyPickupPath = "Assets/freesound_community-money-pickup-2-89563.mp3";
    private const string HurtPath = "Assets/freesound_community-ouch-43811.mp3";

    [MenuItem("Tools/Portfolio/Create Main Menu Scene")]
    public static void CreateMainMenuScene()
    {
        SceneSetup[] previousSetup = EditorSceneManager.GetSceneManagerSetup();

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject root = new GameObject("MainMenu");
        MainMenuController controller = root.AddComponent<MainMenuController>();

        CreateCamera();
        CreateGameAudio();
        Canvas canvas = CreateCanvas();
        CreateBackground(canvas.transform);
        CreateOverlay(canvas.transform);
        CreateStartButton(canvas.transform, controller);
        CreateEventSystem();

        EditorSceneManager.SaveScene(scene, MainMenuPath);
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(MainMenuPath, true),
            new EditorBuildSettingsScene(GameScenePath, true)
        };
        AssetDatabase.SaveAssets();

        if (previousSetup.Length > 0)
            EditorSceneManager.RestoreSceneManagerSetup(previousSetup);

        Debug.Log("[MainMenuSceneBuilder] MainMenu scene created and set as build scene 0.");
    }

    private static void CreateGameAudio()
    {
        GameObject audioObject = new GameObject("GameAudio");
        AudioSource bgmSource = audioObject.AddComponent<AudioSource>();
        AudioSource sfxSource = audioObject.AddComponent<AudioSource>();
        GameAudio gameAudio = audioObject.AddComponent<GameAudio>();

        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.volume = 0.55f;
        bgmSource.spatialBlend = 0f;

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.volume = 0.9f;
        sfxSource.spatialBlend = 0f;

        SerializedObject serializedAudio = new SerializedObject(gameAudio);
        serializedAudio.FindProperty("bgmSource").objectReferenceValue = bgmSource;
        serializedAudio.FindProperty("sfxSource").objectReferenceValue = sfxSource;
        serializedAudio.FindProperty("bgmClip").objectReferenceValue = AssetDatabase.LoadAssetAtPath<AudioClip>(BgmPath);
        serializedAudio.FindProperty("moneyPickupClip").objectReferenceValue = AssetDatabase.LoadAssetAtPath<AudioClip>(MoneyPickupPath);
        serializedAudio.FindProperty("hurtClip").objectReferenceValue = AssetDatabase.LoadAssetAtPath<AudioClip>(HurtPath);
        serializedAudio.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void CreateCamera()
    {
        GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        cameraObject.tag = "MainCamera";
        Camera camera = cameraObject.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color32(8, 10, 12, 255);
        camera.orthographic = true;
        cameraObject.transform.position = new Vector3(0f, 0f, -10f);
    }

    private static Canvas CreateCanvas()
    {
        GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        return canvas;
    }

    private static void CreateBackground(Transform parent)
    {
        GameObject background = CreateUIObject("ThumbnailBackground", parent, typeof(Image));
        Stretch(background.GetComponent<RectTransform>());

        Sprite thumbnail = null;
        foreach (Object asset in AssetDatabase.LoadAllAssetsAtPath(ThumbnailPath))
        {
            if (asset is Sprite sprite)
            {
                thumbnail = sprite;
                break;
            }
        }

        Image image = background.GetComponent<Image>();
        image.sprite = thumbnail;
        image.color = Color.white;
        image.preserveAspect = false;
        image.raycastTarget = false;
    }

    private static void CreateOverlay(Transform parent)
    {
        GameObject overlay = CreateUIObject("DarkOverlay", parent, typeof(Image));
        Stretch(overlay.GetComponent<RectTransform>());
        Image image = overlay.GetComponent<Image>();
        image.color = new Color(0.015f, 0.02f, 0.025f, 0.26f);
        image.raycastTarget = false;
    }

    private static void CreateStartButton(Transform parent, MainMenuController controller)
    {
        GameObject buttonObject = CreateUIObject("StartGameButton", parent, typeof(Image), typeof(Button));
        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, 128f);
        rect.sizeDelta = new Vector2(340f, 78f);

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
        UnityEventTools.AddPersistentListener(button.onClick, controller.StartGame);

        GameObject labelObject = CreateUIObject("Label", buttonObject.transform, typeof(TextMeshProUGUI));
        Stretch(labelObject.GetComponent<RectTransform>());
        TextMeshProUGUI label = labelObject.GetComponent<TextMeshProUGUI>();
        label.text = "START GAME";
        label.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset");
        label.fontSize = 32f;
        label.fontStyle = FontStyles.Bold;
        label.color = new Color32(17, 25, 19, 255);
        label.alignment = TextAlignmentOptions.Center;
        label.raycastTarget = false;
    }

    private static void CreateEventSystem()
    {
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }

    private static GameObject CreateUIObject(string name, Transform parent, params System.Type[] components)
    {
        GameObject gameObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer));
        foreach (System.Type component in components)
        {
            if (gameObject.GetComponent(component) == null)
                gameObject.AddComponent(component);
        }
        gameObject.transform.SetParent(parent, false);
        return gameObject;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
