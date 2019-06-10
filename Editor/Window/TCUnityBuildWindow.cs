using UnityEditor;
using UnityEngine;

public class TCUnityBuildWindow : EditorWindow
{
    private const int SPACE = 5;
    private Header header;
    private bool groupEnabled;
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
        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = 15;
        customButton.fontStyle = FontStyle.Bold;

        int headerWidth = header.Draw(position);
        GUILayout.Space(headerWidth);
        GUILayout.Space(SPACE*3);

        GUILayout.Button("Test1", customButton, GUILayout.Height(30));
        GUILayout.Space(SPACE);
        GUILayout.Button("Test2", customButton, GUILayout.Height(30));
    }



    public class Header
    {
        private Texture iconTexture;
        private GUIStyle buttonStyle;
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
            buttonStyle = GUI.skin.label;
            buttonStyle.fontSize = 40;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.normal.textColor = labelColor;
        }

        public int Draw(Rect screen)
        {
            if (buttonStyle == null)
            {
                CreateButtonStyle();
            }

            Rect fullRect = new Rect(0, 0, screen.width, HEIGHT);
            EditorGUI.DrawRect(fullRect, EditorGUIUtility.isProSkin ? new Color(.3f, .3f, .3f): new Color(0.85f, 0.85f, 0.85f));
            EditorGUI.DrawRect(new Rect(0, HEIGHT, screen.width, 1), Color.black);
            
            Rect logoRect = new Rect(PADDING, PADDING, ICON_HEIGHT, ICON_HEIGHT);
            GUI.DrawTexture(logoRect, iconTexture, ScaleMode.ScaleToFit, true, 1.0F);
            
            Rect textRect = new Rect(PADDING + ICON_HEIGHT + PADDING, PADDING, screen.width, ICON_HEIGHT);
            GUI.Label(textRect, "TC Unity Builder", buttonStyle);
            return HEIGHT;
        }

    }
}
