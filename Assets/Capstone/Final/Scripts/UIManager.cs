using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ModelManager modelManager;
    public ARManager arManager;

    public void OnSelectModel(int index)
    {
        modelManager.SelectModel(index);
    }

    public void OnBackToSelection()
    {
        modelManager.BackToSelection();
    }

    public void OnResetTransform()
    {
        modelManager.ResetModelTransform();
    }

    public void OnARView()
    {
        GameObject current = modelManager.GetCurrentARModel();
        if (current != null)
        {
            //arManager.SetARModel(current); // Pass selected model to ARManager
            modelManager.displayPanel.SetActive(false);
            modelManager.selectionPanel.SetActive(false);
            modelManager.arPanel.SetActive(true);
            arManager.EnableARView();
        }
    }
    public void OnBackToNormalView()
    {
        arManager.ClearAllPlacedModels(); // <-- clear placed models
        //arManager.ClearAllPlanes();         // clear detected planes
        arManager.BackToNormalView();

        modelManager.arPanel.SetActive(false);
        modelManager.displayPanel.SetActive(true);
        modelManager.ResetModelTransform();
    }



    public void OnPlaceInAR()
    {
        arManager.PlaceModel();
    }

    public void OnUndoAR()
    {
        arManager.UndoLastPlacement();
    }
}
