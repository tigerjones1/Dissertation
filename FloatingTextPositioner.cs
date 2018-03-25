using UnityEngine;

public interface FloatingTextPositioner {

    bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size);

}
