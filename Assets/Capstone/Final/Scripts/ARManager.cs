using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Lean.Touch;
public class ARManager : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    public GameObject XROrigin;
    public GameObject arSession;
    public GameObject modelPrefab; // Clone of selected model prefab
    public Transform modelParent;

    private GameObject placedModel;
    private List<GameObject> modelHistory = new List<GameObject>();

    public Button undoButton;
    public Button placeButton;

    public void EnableARView()
    {
        XROrigin.SetActive(true);
        arSession.SetActive(true);
        modelPrefab.SetActive(false);
        // enable planes after placement
        TogglePlaneManager(true);
    }

    public void BackToNormalView()
    {
        XROrigin.SetActive(false);
        arSession.SetActive(false);
        placeButton.gameObject.SetActive(true);
        undoButton.gameObject.SetActive(false);
        modelPrefab.GetComponent<LeanDragTranslate>().Sensitivity = 1;
        modelPrefab.GetComponent<LeanTwistRotateAxis>().Sensitivity = 2;
        modelPrefab.GetComponent<LeanPinchScale>().Relative = true;
    }

    public void PlaceModel()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits))
        {
            Pose pose = hits[0].pose;

            //GameObject obj = Instantiate(modelPrefab, pose.position, pose.rotation, modelParent);
            modelPrefab.transform.position = pose.position;
            modelPrefab.transform.rotation = pose.rotation;
            //modelPrefab.transform.SetParent(modelParent);
            // Scale down by dividing original scale by 10
            modelPrefab.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
            modelPrefab.GetComponent<LeanDragTranslate>().Sensitivity = 0.1f;
            modelPrefab.GetComponent<LeanTwistRotateAxis>().Sensitivity = 1;
            modelPrefab.GetComponent<LeanPinchScale>().Relative = false;
            modelPrefab.SetActive(true);
            modelHistory.Add(modelPrefab);
            undoButton.gameObject.SetActive(true);
            placeButton.gameObject.SetActive(false);
            // Disable planes after placement
            TogglePlaneManager(false);
        }
    }


    public void UndoLastPlacement()
    {

        modelPrefab.SetActive(false);
        placeButton.gameObject.SetActive(true);
        undoButton.gameObject.SetActive(false);

        // enable planes after placement
        TogglePlaneManager(true);

    }

    public void SetARModel(GameObject model)
    {
        modelPrefab = model;
    }

    public void ClearAllPlacedModels()
    {
        foreach (GameObject obj in modelHistory)
        {
            Destroy(obj);
        }
        modelHistory.Clear();
    }

    public void ClearAllPlanes()
    {
        if (planeManager != null)
        {
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
                //Destroy(plane);
            }
            
        }
    }

    private void TogglePlaneManager(bool enable)
    {
        if (planeManager != null)
        {

            // Deactivate/Activate All Existing Planes.
            planeManager.SetTrackablesActive(enable);

            planeManager.enabled = enable;
            
        }
    }


}
