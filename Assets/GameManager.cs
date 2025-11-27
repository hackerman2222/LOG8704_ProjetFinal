using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject confirmQuitPanel;
    public GameObject baseMenuPanel;
    public GameObject dominantHandSelectionPanel;
    public GameObject fullUI;

    private GameObject activePanel;
    private List<GameObject> panelList = new List<GameObject>();

    void Start()
    {
        OVRScene.RequestSpaceSetup();

        RegisterPanel(confirmQuitPanel);
        RegisterPanel(baseMenuPanel);
        RegisterPanel(dominantHandSelectionPanel);

        activePanel = dominantHandSelectionPanel;
        ShowPanel(dominantHandSelectionPanel);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch)) 
        {
            if (activePanel == null) 
            {
                ShowPanel(baseMenuPanel);
            }
            else 
            {
                CloseAllMenu();
            }
        }
    }

    public void AskQuit()
    {
        ShowPanel(confirmQuitPanel);
    }

    public void CancelQuit()
    {
        ShowPanel(baseMenuPanel);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void ShowPanel(GameObject panelToShow)
    {
        if (panelToShow == null)
        {
            return;
        }
        GlobalSettings.Instance.IsMenuOpen = true;
        fullUI.SetActive(true);
        
        foreach (var panel in panelList) 
        {
            panel.SetActive(panel == panelToShow);
        }

        activePanel = panelToShow;
    }

    public void CloseAllMenu() 
    {
        foreach (var panel in panelList) 
        {
            panel.SetActive(false);
        }
        activePanel = null;
        GlobalSettings.Instance.IsMenuOpen = false;
        fullUI.SetActive(false);
    }

    private void RegisterPanel(GameObject panel)
    {
        if (panel != null && !panelList.Contains(panel)) 
        {
            panelList.Add(panel);
        }
        else if (panel == null)
        {
            return;
        }
    }
}
