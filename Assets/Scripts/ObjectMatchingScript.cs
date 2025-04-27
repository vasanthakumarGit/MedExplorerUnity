using UnityEngine;
using TMPro;

namespace ObjectMatching
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(LineRenderer))]
    public class ObjectMatchingScript : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private SpriteRenderer spriteRenderer;

        [Header("Declare")]
        [SerializeField] private int matchId;
        [SerializeField] private TMP_Text scoreValue;
        [SerializeField] private GameObject tryAgainArea;
        [SerializeField] private GameObject completedArea;
        [SerializeField] private GameObject BGArea;

        private bool isDragging;
        private Vector3 endPoint;

        // Reference to the MatchingID script
        private MatchingID matchingID;

        // Static variables for counting correct and incorrect matches
        private static int incorrectMatchCount = 0;
        private static int correctMatchCount = 0;
        private bool boolean = true;

        private void Start()
        {
            incorrectMatchCount = 0;
            correctMatchCount = 0;
            Debug.Log("Values Resetted");

            spriteRenderer = GetComponent<SpriteRenderer>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                scoreValue.text = "";

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    this.enabled = true;
                    isDragging = true;
                    mousePosition.z = 0f;
                    lineRenderer.SetPosition(0, mousePosition);
                }
            }

            if (isDragging)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                lineRenderer.SetPosition(1, mousePosition);
                endPoint = mousePosition;
                RaycastHit2D hit = Physics2D.Raycast(endPoint, Vector2.zero);
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                RaycastHit2D hit = Physics2D.Raycast(endPoint, Vector2.zero);
                if (hit.collider != null && hit.collider.TryGetComponent(out matchingID) && matchId == matchingID.Get_ID())
                {
                    // If correct match is made, sprite color changed to green.
                    spriteRenderer.color = Color.green;
                    correctMatchCount++;
                    Debug.Log("Added Correct Match: " + correctMatchCount);
                    // Check if all correct matches are made
                    if (correctMatchCount == 5)
                    {
                        completedArea.SetActive(true);
                        BGArea.SetActive(false);
                    }
                    // Disable script when matched correctly.
                    this.enabled = false;
                }
                else if (hit.collider != null)
                {
                    // Used to match to sprites which are not correct match.
                    if (boolean)
                    {
                        incorrectMatchCount++;
                        Debug.Log("Added Incorrect Match: " + incorrectMatchCount);
                        if (correctMatchCount + incorrectMatchCount == 5)
                        {
                            scoreValue.text = "" + correctMatchCount;
                            tryAgainArea.SetActive(true);
                            BGArea.SetActive(false);
                        }
                    }
                    boolean = false;
                }
                else
                {
                    // Removes the LineRenderer
                    lineRenderer.positionCount = 0;
                }
                // Reset line position count
                lineRenderer.positionCount = 2;
            }
        }
    }
}
