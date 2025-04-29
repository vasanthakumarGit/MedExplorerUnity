using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemVisibilityController : MonoBehaviour
{
    public GameObject skeletalSystem;
    public GameObject muscularSystem;
    public GameObject respiratorySystem;
    public GameObject digestiveSystem;
    public GameObject circulatorySystem;

    public void UpdateSystemVisibility(bool showSkeletal, bool showMuscular, bool showNervous, bool showDigestive, bool showCirculatory)
    {
        skeletalSystem.SetActive(showSkeletal);
        muscularSystem.SetActive(showMuscular);
        respiratorySystem.SetActive(showNervous);
        digestiveSystem.SetActive(showDigestive);
        circulatorySystem.SetActive(showCirculatory);
    }
}

