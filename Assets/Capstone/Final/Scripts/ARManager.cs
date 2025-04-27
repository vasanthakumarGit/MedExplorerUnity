using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

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

    public void EnableARView()
    {
        XROrigin.SetActive(true);
        arSession.SetActive(true);
    }

    public void BackToNormalView()
    {
        XROrigin.SetActive(false);
        arSession.SetActive(false);
    }

    public void PlaceModel()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits))
        {
            Pose pose = hits[0].pose;

            GameObject obj = Instantiate(modelPrefab, pose.position, pose.rotation, modelParent);

            // Scale down by dividing original scale by 10
            obj.transform.localScale = obj.transform.localScale / 10f;

            modelHistory.Add(obj);

            // Disable planes after placement
            TogglePlaneManager(false);
        }
    }


    public void UndoLastPlacement()
    {
        if (modelHistory.Count > 0)
        {
            GameObject last = modelHistory[modelHistory.Count - 1];
            modelHistory.RemoveAt(modelHistory.Count - 1);
            Destroy(last);
        }

        //If no models are left, re-enable planes
        if (modelHistory.Count == 0)
        {
            TogglePlaneManager(true);
        }
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
