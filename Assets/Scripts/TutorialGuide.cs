using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGuide : MonoBehaviour
{
    public int number;
    public LayerMask playerLayer;
    public GameObject[] textPrefab;
    private GameObject text;
    private bool textSeen;
    

    // Start is called before the first frame update
    void Start()
    {
        textSeen = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if within ray cast display text on UI delete once out of area
        // what is displayed is based on the tutorial number
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 2, playerLayer);
        if(leftHit.collider != null)
        {
            Vector3 spawn = new Vector3(10, 10, 0);
            if(!textSeen)
            {
                text = Instantiate(textPrefab[number], spawn, Quaternion.identity);
                text.transform.SetParent(GameUICanvas.instance.transform);
                text.transform.position += new Vector3(325, 300,0);
                textSeen = true;
            }
            
        }
        if(leftHit.collider == null && text != null)
        {
            Destroy(text.gameObject);
            textSeen = false;
        }
    }
}
