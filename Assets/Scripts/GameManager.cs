using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Patterns;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);

        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //StartCoroutine(_ExitApplication());
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CanvasManager.ShowControlsPanel(!CanvasManager.IsShowingControls);
        }
    }

    IEnumerator _ExitApplication()
    {
        float timer = 2f;

        while (timer > 0f)
        {
            if (!Input.GetKey(KeyCode.Escape))
                yield break;

            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Application.Quit();
    }

    public static void LoadScene(string sceneName)
    {
        CanvasManager.ShowControlsPanel(false);
        SceneManager.LoadScene(sceneName);
    }
}
