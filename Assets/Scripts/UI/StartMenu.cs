using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject controlsPanel;

    bool _areControlsActive = false;

    void Start()
    {
        controlsPanel.SetActive(_areControlsActive);
        MusicManager.SetMusicalTheme(MusicManager.Theme.MainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("FoolTarot");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _areControlsActive = !_areControlsActive;
            controlsPanel.SetActive(_areControlsActive);
        }
    }
}
