using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemLabelController : MonoBehaviour
{
    public GameObject skeletalLabel;
    public GameObject muscularLabel;
    public GameObject respiratoryLabel;
    public GameObject digestiveLabel;
    public GameObject circulatoryLabel;

    public void UpdateLabelVisibility(bool showSkeletal, bool showMuscular, bool showNervous, bool showDigestive, bool showCirculatory)
    {
        skeletalLabel.SetActive(showSkeletal);
        muscularLabel.SetActive(showMuscular);
        respiratoryLabel.SetActive(showNervous);
        digestiveLabel.SetActive(showDigestive);
        circulatoryLabel.SetActive(showCirculatory);
    }
}
