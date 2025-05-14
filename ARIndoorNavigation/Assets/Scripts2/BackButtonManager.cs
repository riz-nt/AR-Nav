using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class BackButtonManager : MonoBehaviour
{
    public GameObject landingPanel;
    public GameObject qrScanningPanel;
    public GameObject nextPanel;

    private GameObject currentPanel;
    private GameObject previousPanel;

    private bool doubleBackToExitPressedOnce = false;

    void Start()
    {
        // Start on landing panel
        ShowPanel(landingPanel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBack();
        }
    }

    public void ShowPanel(GameObject panelToShow)
    {
        if (currentPanel != null)
        {
            previousPanel = currentPanel;
            currentPanel.SetActive(false);
        }

        currentPanel = panelToShow;
        currentPanel.SetActive(true);
    }

    public void HandleBack()
    {
        if (currentPanel == landingPanel)
        {
            // Optional: Double-tap back to exit
            if (doubleBackToExitPressedOnce)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            else
            {
                doubleBackToExitPressedOnce = true;
                Debug.Log("Press back again to exit");
                StartCoroutine(ResetBackPress());
            }
        }
        else if (currentPanel == qrScanningPanel)
        {
            ShowPanel(landingPanel);
        }
        else if (currentPanel == nextPanel)
        {
            ShowPanel(qrScanningPanel);
        }
    }

    IEnumerator ResetBackPress()
    {
        yield return new WaitForSeconds(2f);
        doubleBackToExitPressedOnce = false;
    }
}

   