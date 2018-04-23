using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

    public GameObject Player; 
    public int rotationOffSet = 90;
   // public bool FacingRight;

    void Awake()
    {
        
    }

	// Update is called once per frame
	void Update () {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffSet);

        if(Player.GetComponent<Player>().isFacingRight != true)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.x);
            //GetComponent<Player>().isFacingRigt = transform.localScale.x > 0;
        }
	}
}
