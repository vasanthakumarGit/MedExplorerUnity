using UnityEngine;
using UnityEngine.UI;

public class SystemControlManager : MonoBehaviour
{
    // Buttons
    [Header("SelectionPanel Buttons")]
    public Button skeletalButton;
    public Button muscularButton;
    public Button nervousButton;
    public Button digestiveButton;
    public Button circulatoryButton;

    // Model Toggles
    [Header("Model Selection Toggles")]
    public Toggle skeletalToggle;
    public Toggle muscularToggle;
    public Toggle nervousToggle;
    public Toggle digestiveToggle;
    public Toggle circulatoryToggle;
    
    // Label Toggles
    [Header("Label Selection Toggles")]
    public Toggle skeletalLabelToggle;
    public Toggle muscularLabelToggle;
    public Toggle nervousLabelToggle;
    public Toggle digestiveLabelToggle;
    public Toggle circulatoryLabelToggle;

    // System Controller
    public SystemVisibilityController systemController;
    public SystemLabelController systemLabelController;

    public ModelsPageManager modelsPageManager;

    public GameObject humanAnatomyModel;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private bool fromButton = false; // To avoid feedback loop

    void Start()
    {

        originalPosition = humanAnatomyModel.transform.position;
        originalRotation = humanAnatomyModel.transform.rotation;
        originalScale = humanAnatomyModel.transform.localScale;

        // Button listeners
        skeletalButton.onClick.AddListener(() => OnSystemButtonClick("skeletal"));
        muscularButton.onClick.AddListener(() => OnSystemButtonClick("muscular"));
        nervousButton.onClick.AddListener(() => OnSystemButtonClick("nervous"));
        digestiveButton.onClick.AddListener(() => OnSystemButtonClick("digestive"));
        circulatoryButton.onClick.AddListener(() => OnSystemButtonClick("circulatory"));

        // Toggle listeners
        skeletalToggle.onValueChanged.AddListener(_ => OnModelToggleChanged());
        muscularToggle.onValueChanged.AddListener(_ => OnModelToggleChanged());
        nervousToggle.onValueChanged.AddListener(_ => OnModelToggleChanged());
        digestiveToggle.onValueChanged.AddListener(_ => OnModelToggleChanged());
        circulatoryToggle.onValueChanged.AddListener(_ => OnModelToggleChanged());
        
        // Toggle listeners
        skeletalLabelToggle.onValueChanged.AddListener(_ => OnLabelToggleChanged());
        muscularLabelToggle.onValueChanged.AddListener(_ => OnLabelToggleChanged());
        nervousLabelToggle.onValueChanged.AddListener(_ => OnLabelToggleChanged());
        digestiveLabelToggle.onValueChanged.AddListener(_ => OnLabelToggleChanged());
        circulatoryLabelToggle.onValueChanged.AddListener(_ => OnLabelToggleChanged());

        OnModelToggleChanged(); // Initial update
        OnLabelToggleChanged(); // Initial update
    }

    void OnSystemButtonClick(string system)
    {
        fromButton = true;

        modelsPageManager.ShowDisplayPage();
        // Enable only selected model
        bool skeletal = system == "skeletal";
        bool muscular = system == "muscular";
        bool nervous = system == "nervous";
        bool digestive = system == "digestive";
        bool circulatory = system == "circulatory";

        // Update toggles to match
        skeletalToggle.isOn = skeletal;
        muscularToggle.isOn = muscular;
        nervousToggle.isOn = nervous;
        digestiveToggle.isOn = digestive;
        circulatoryToggle.isOn = circulatory;

        // Update toggles to match
        skeletalLabelToggle.isOn = skeletal;
        muscularLabelToggle.isOn = muscular;
        nervousLabelToggle.isOn = nervous;
        digestiveLabelToggle.isOn = digestive;
        circulatoryLabelToggle.isOn = circulatory;



        // Directly update visibility
        systemController.UpdateSystemVisibility(skeletal, muscular, nervous, digestive, circulatory);
        systemLabelController.UpdateLabelVisibility(skeletal, muscular, nervous, digestive, circulatory);

        fromButton = false;
    }

    void OnModelToggleChanged()
    {
        if (fromButton) return; // Prevent recursive update

        // Update system based on current toggles
        systemController.UpdateSystemVisibility(
            skeletalToggle.isOn,
            muscularToggle.isOn,
            nervousToggle.isOn,
            digestiveToggle.isOn,
            circulatoryToggle.isOn
        );
    }
    void OnLabelToggleChanged()
    {
        if (fromButton) return; // Prevent recursive update

        // Update system based on current toggles
        systemLabelController.UpdateLabelVisibility(
            skeletalLabelToggle.isOn,
            muscularLabelToggle.isOn,
            nervousLabelToggle.isOn,
            digestiveLabelToggle.isOn,
            circulatoryLabelToggle.isOn
        );
    }


    public void ResetModelTransform()
    {
        if (humanAnatomyModel != null)
        {
            humanAnatomyModel.transform.position = originalPosition;
            humanAnatomyModel.transform.rotation = originalRotation;
            humanAnatomyModel.transform.localScale = originalScale;
        }
    }


    public void OnBackToNormalView()
    {
        //modelsPageManager.arManager.ClearAllPlacedModels(); // <-- clear placed models
        //arManager.ClearAllPlanes();         // clear detected planes
        modelsPageManager.arManager.BackToNormalView();
        modelsPageManager.ShowDisplayPage();
        humanAnatomyModel.SetActive(true);
        ResetModelTransform();
    }



}
