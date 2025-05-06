using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelsPageManager : MonoBehaviour
{
    public GameObject selectionPanel;
    public GameObject displayPanel;
    public GameObject arPanel;

  
    public ARManager arManager;

    public void ShowSelectionPage()
    {
        selectionPanel.SetActive(true);
        displayPanel.SetActive(false);
        arPanel.SetActive(false);
    }

    public void ShowDisplayPage()
    {
        selectionPanel.SetActive(false);
        displayPanel.SetActive(true);
        arPanel.SetActive(false);
    }

    public void ShowARPage()
    {
        selectionPanel.SetActive(false);
        displayPanel.SetActive(false);
        arPanel.SetActive(true);
    }

    public void OnARView()
    {
        ShowARPage();
        arManager.EnableARView();
    }



}
