using UnityEditor;
using UnityEngine;

public class TCUnityBuildWindow : EditorWindow
{
    private const int SPACE = 5;
    private Header header;
    private bool groupEnabled;
    
    private GUIStyle labelStyle;
    private GUIStyle buttonStyle;
    private Vector2 scrollPosition;
    [MenuItem("Window/TC Build")]
    static void Init()
    {
        TCUnityBuildWindow window = (TCUnityBuildWindow) GetWindow(typeof(TCUnityBuildWindow));
        window.Show();
    }

    private void OnEnable()
    {
        header = new Header();
    }
    
    void OnGUI()
    {
        if (buttonStyle == null)
        {
            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 15;
            buttonStyle.fontStyle = FontStyle.Bold;
            
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 20;
            labelStyle.fontStyle = FontStyle.Bold;
        }
        
        int headerWidth = header.Draw(position);
        GUILayout.Space(headerWidth);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        GUILayout.Space(SPACE * 3);

        GUILayout.Label("Run Tests:", labelStyle);
        GUILayout.Space(SPACE);
        if (Button("Unit Tests"))
        {
            WipMessage();
        }
        if (Button("Play Mode Tests"))
        {
            WipMessage();
        }
        if (Button("Performance Tests"))
        {
            WipMessage();
        }
        if (Button("Smoke Tests"))
        {
            WipMessage();
        }

        GUILayout.Space(SPACE * 3);
        
        GUILayout.Label("Create Build:", labelStyle);
        GUILayout.Space(SPACE);
        if (Button("Android Build"))
        {
            WipMessage();
        }
        if (Button("iOS Build"))
        {
            WipMessage();
        }
        if (Button("Amazone Build"))
        {
            WipMessage();
        }
        if (Button("Test Build"))
        {
            WipMessage();
        }
        
        GUILayout.Label("Other:", labelStyle);
        GUILayout.Space(SPACE);
        if (Button("Build Asset Bundles"))
        {
            WipMessage();
        }
        if (Button("Build Unity Package"))
        {
            WipMessage();
        }

        GUILayout.Space(SPACE * 3);
        
//        GUILayout.FlexibleSpace();
//        if (GUILayout.Button("Drop Styles"))
//        {
//            buttonStyle = null;
//        }
        EditorGUILayout.EndScrollView();
    }

    bool Button(string text)
    {
        bool result = GUILayout.Button(text, buttonStyle, GUILayout.Height(30));
        GUILayout.Space(SPACE);
        return result;
    }

    void WipMessage()
    {
        EditorUtility.DisplayDialog("Work In Progress", "This functional is not supported yet.", "Ok");
    }

    public class Header
    {
        private Texture iconTexture;
        private GUIStyle labelStyle;
        private Color labelColor = new Color(0.1f,0.1f,0.1f);
        private const int PADDING = 5;
        private const int ICON_HEIGHT = 50;
        private const int HEIGHT = ICON_HEIGHT + PADDING * 2;
        
        public Header()
        {
            iconTexture = Resources.Load("teamcity-icon-logo-png-transparent") as Texture;
        }

        private void CreateButtonStyle()
        {
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 40;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = labelColor;
        }

        public int Draw(Rect screen)
        {
            if (labelStyle == null)
            {
                CreateButtonStyle();
            }

            Rect fullRect = new Rect(0, 0, screen.width, HEIGHT);
            EditorGUI.DrawRect(fullRect, EditorGUIUtility.isProSkin ? new Color(.3f, .3f, .3f): new Color(0.85f, 0.85f, 0.85f));
            EditorGUI.DrawRect(new Rect(0, HEIGHT-1, screen.width, 1), Color.black);
            
            Rect logoRect = new Rect(PADDING, PADDING, ICON_HEIGHT, ICON_HEIGHT);
            GUI.DrawTexture(logoRect, iconTexture, ScaleMode.ScaleToFit, true, 1.0F);
            
            Rect textRect = new Rect(PADDING + ICON_HEIGHT + PADDING, PADDING, screen.width, ICON_HEIGHT);
            GUI.Label(textRect, "TC Unity Builder", labelStyle);
            return HEIGHT;
        }

    }
}
