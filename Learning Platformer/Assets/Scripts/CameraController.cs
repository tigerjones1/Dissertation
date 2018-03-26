using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform Player;

    public Vector2 Margin, Smoothing;

    public BoxCollider2D Bounds;

    float nextTimeToSearch = 0;

    Vector3 lastPlayerPosition;

    private Vector3
        min,
        max;
        
    public bool IsFollowing { get; set; }

    public void Start()
    {
        lastPlayerPosition = Player.position;
        min = Bounds.bounds.min;
        max = Bounds.bounds.max;
        IsFollowing = true;
    }
    public void Update()
    {

        if (Player == null)
        {
            FindPlayer();
            return;
        }

        var x = transform.position.x;
        var y = transform.position.y;

        if (IsFollowing)
        {
            if (Mathf.Abs(x - Player.position.x) > Margin.x)
                x = Mathf.Lerp(x, Player.position.x, Smoothing.x * Time.deltaTime);
            if (Mathf.Abs(y - Player.position.y) > Margin.y)
                y = Mathf.Lerp(y, Player.position.y, Smoothing.y * Time.deltaTime);

        }
        var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);

        x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);
        y = Mathf.Clamp(y, min.y + GetComponent<Camera>().orthographicSize, max.y - GetComponent<Camera>().orthographicSize);

        

        

        transform.position = new Vector3(x, y, transform.position.z);

        lastPlayerPosition = Player.position;
    }
    void FindPlayer()
    {
        if(nextTimeToSearch <= Time.time)
        {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null)
                Player = searchResult.transform;
            nextTimeToSearch = Time.time + 0.5f;
        }
    }
}
