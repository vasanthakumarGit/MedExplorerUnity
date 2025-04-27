using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public GameObject selectionPanel;
    public GameObject displayPanel;
    public GameObject arPanel;
    public GameObject ModelParent3D;

    public GameObject[] models; // Assign in Inspector
    public GameObject[] Armodels; // Assign in Inspector
    private GameObject currentModel;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private int currentModelIndex = -1;


    void Start()
    {
        // On Start: Only selection panel is shown
        selectionPanel.SetActive(true);
        displayPanel.SetActive(false);
        arPanel.SetActive(false);

        if (currentModel != null)
            Destroy(currentModel);
    }

    public void SelectModel(int index)
    {
        if (currentModel != null)
            Destroy(currentModel);

        currentModel = Instantiate(models[index], ModelParent3D.transform);
        originalPosition = currentModel.transform.position;
        originalRotation = currentModel.transform.rotation;
        originalScale = currentModel.transform.localScale;
        currentModelIndex = index; // <-- store the index

        selectionPanel.SetActive(false);
        displayPanel.SetActive(true);
        arPanel.SetActive(false);
    }


    public void BackToSelection()
    {
        if (currentModel != null)
            Destroy(currentModel);

        selectionPanel.SetActive(true);
        displayPanel.SetActive(false);
        arPanel.SetActive(false);
    }

    public void ResetModelTransform()
    {
        if (currentModel != null)
        {
            currentModel.transform.position = originalPosition;
            currentModel.transform.rotation = originalRotation;
            currentModel.transform.localScale = originalScale;
        }
    }

    public GameObject GetCurrentModel()
    {
        return currentModel;
    }

    public GameObject GetCurrentARModel()
    {
        if (currentModelIndex >= 0 && currentModelIndex < Armodels.Length)
        {
            return Armodels[currentModelIndex];
        }
        return null;
    }

}
