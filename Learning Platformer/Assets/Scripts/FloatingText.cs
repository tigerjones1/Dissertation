using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

    private static readonly GUISkin skin = Resources.Load<GUISkin>("GameSkin");

    public static FloatingText Show(string text, string style, FloatingTextPositioner positioner)
    {
        var go = new GameObject("Floating Text");
        var floatingText = go.AddComponent<FloatingText>();
        floatingText.Style = skin.GetStyle(style);
        floatingText.positioner = positioner;
        floatingText.content = new GUIContent(text);
        return floatingText;
    }

    private GUIContent content;
    private FloatingTextPositioner positioner;

    public string Text { get { return content.text; } set { content.text = value; } }
    public GUIStyle Style { get; set; }

    public void OnGUI()
    {
        var position = new Vector2();
        var contentSize = Style.CalcSize(content);
        if(!positioner.GetPosition(ref position, content, contentSize))
        {
            Destroy(gameObject);
            return;
        }

        GUI.Label(new Rect(position.x, position.y, contentSize.x, contentSize.y), content, Style);
    }


}

