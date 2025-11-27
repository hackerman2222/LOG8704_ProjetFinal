using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject confirmQuitPanel;
    public GameObject baseMenuPanel;
    public GameObject dominantHandSelectionPanel;
    public GameObject Settings;
    public GameObject tutoriel1;
    public GameObject tutoriel2;
    public GameObject tutoriel3;
    public GameObject tutoriel4;
    public GameObject tutoriel5;
    public GameObject fullUI;
    public HandColliderVisualizer leftHand;
    public HandColliderVisualizer rightHand;
    

    private GameObject activePanel;
    private List<GameObject> panelList = new List<GameObject>();
    private bool IsTutoriel = true;

    void Start()
    {
        OVRScene.RequestSpaceSetup();

        RegisterPanel(confirmQuitPanel);
        RegisterPanel(baseMenuPanel);
        RegisterPanel(dominantHandSelectionPanel);
        RegisterPanel(Settings);
        RegisterPanel(tutoriel1);
        RegisterPanel(tutoriel2);
        RegisterPanel(tutoriel3);
        RegisterPanel(tutoriel4);
        RegisterPanel(tutoriel5);

        ShowPanel(dominantHandSelectionPanel);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch) && !IsTutoriel) 
        {
            if (activePanel == null) 
            {
                ShowPanel(baseMenuPanel);
            }
            else 
            {
                CloseAllMenu();
                if (GlobalSettings.Instance.IsFreePractice) 
                {
                    leftHand.ActivateColliders();
                    rightHand.ActivateColliders();
                }
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

    public void EndTutoriel()
    {
        ShowPanel(baseMenuPanel);
        IsTutoriel = false;
    }
    
    public void PlayTutoriel()
    {
        IsTutoriel = true;
        ShowPanel(tutoriel1);
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
