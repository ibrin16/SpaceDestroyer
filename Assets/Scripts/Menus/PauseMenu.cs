using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject UIPanel = null;
    bool open;

    // Start is called before the first frame update
    void Start()
    {
        UIPanel.SetActive(false);
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            StopAllCoroutines();
            if (open)
            {
                StartCoroutine(CloseRoutine());
            }
            else if (!open)
            {
                StartCoroutine(OpenRoutine());
            }
        }
    }

    IEnumerator OpenRoutine ()
    {
        open = true;
        UIPanel.SetActive(true);
        UIPanel.transform.localScale = new Vector3(1, 1, 1);
        yield return null;
    }

    IEnumerator CloseRoutine ()
    {
        UIPanel.transform.localScale = new Vector3(0, 1, 1);
        open = false;
        UIPanel.SetActive(false);
        yield return null;
    }

    public void LoadScene (string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Close()
    {
        StartCoroutine(CloseRoutine());
    }
}
