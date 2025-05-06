using UnityEngine;
using UnityEngine.UI;

public class AuthPageNavigator : MonoBehaviour
{
    [Header("Pages")]
    public GameObject startingPage;
    public GameObject loginPage;
    public GameObject signupPage;
    public GameObject menuPanel;

    [Header("Buttons")]
    public Button getStartedButton;
    public Button goToSignupButton;
    public Button goToLoginButton;

    void Start()
    {
        // Assign button click listeners
        getStartedButton.onClick.AddListener(OpenLoginPage);
        goToSignupButton.onClick.AddListener(OpenSignupPage);
        goToLoginButton.onClick.AddListener(OpenLoginPageFromSignup);

        // Initially show starting page only
        //OpenStartingPage();
    }

    public void OpenStartingPage()
    {
        startingPage.SetActive(true);
        loginPage.SetActive(false);
        signupPage.SetActive(false);
        menuPanel.SetActive(false);
    }


    public void OpenLoginPage()
    {
        startingPage.SetActive(false);
        loginPage.SetActive(true);
        signupPage.SetActive(false);
        menuPanel.SetActive(false);
    }

    public void OpenSignupPage()
    {
        startingPage.SetActive(false);
        loginPage.SetActive(false);
        menuPanel.SetActive(false);
        signupPage.SetActive(true);
    }

    public void OpenLoginPageFromSignup()
    {
        startingPage.SetActive(false);
        loginPage.SetActive(true);
        signupPage.SetActive(false);
        menuPanel.SetActive(false);
    }

    public void openMenuPage()
    {
        menuPanel.SetActive(true);
        startingPage.SetActive(false);
        loginPage.SetActive(false);
        signupPage.SetActive(false);
    }


}
