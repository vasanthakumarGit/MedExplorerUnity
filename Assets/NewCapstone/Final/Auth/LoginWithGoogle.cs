using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginWithGoogle : MonoBehaviour
{
    public string GoogleAPI = "594092736595-ania5njkug7lkv3p3nbtjlcf926bbv70.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public TextMeshProUGUI Username, UserEmail;
    public Image UserProfilePic;
    //public GameObject MenuPanel;
    public AuthPageNavigator authUIMgr;

    private string imageUrl;
    private bool isGoogleSignInInitialized = false;

    private void Start()
    {
        InitFirebase();
        CheckPreviousLogin();
    }

    void InitFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    void CheckPreviousLogin()
    {
        if (PlayerPrefs.HasKey("UserName") && PlayerPrefs.HasKey("UserEmail") && PlayerPrefs.HasKey("UserPhoto"))
        {
            // Already logged in
            string fullName = PlayerPrefs.GetString("UserName");
            string firstName = fullName.Contains(" ") ? fullName.Split(' ')[0] : fullName;
            Username.text = "Welcome, "+ firstName + "!";
            UserEmail.text = PlayerPrefs.GetString("UserEmail");
            StartCoroutine(LoadImage(PlayerPrefs.GetString("UserPhoto")));
            authUIMgr.openMenuPage();
        }
        else
        {
            // Not logged in
            authUIMgr.OpenStartingPage();
        }
    }

    public void Login()
    {
        if (!isGoogleSignInInitialized)
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                WebClientId = GoogleAPI,
                RequestEmail = true
            };
            isGoogleSignInInitialized = true;
        }
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            WebClientId = GoogleAPI,
            RequestEmail = true
        };

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();
        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();

        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
                Debug.Log("Cancelled");
            }
            else if (task.IsFaulted)
            {
                signInCompleted.SetException(task.Exception);
                Debug.Log("Faulted " + task.Exception);
            }
            else
            {
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                    }
                    else if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                        Debug.Log("Faulted In Auth " + authTask.Exception);
                    }
                    else
                    {
                        signInCompleted.SetResult(authTask.Result);
                        Debug.Log("Success");

                        user = auth.CurrentUser;
                        string fullName = user.DisplayName;
                        string firstName = fullName.Contains(" ") ? fullName.Split(' ')[0] : fullName;
                        Username.text = "Welcome, "+ firstName + "!";
                        UserEmail.text = user.Email;

                        string profileUrl = CheckImageUrl(user.PhotoUrl.ToString());
                        StartCoroutine(LoadImage(profileUrl));

                        // Save user data locally
                        PlayerPrefs.SetString("UserName", user.DisplayName);
                        PlayerPrefs.SetString("UserEmail", user.Email);
                        PlayerPrefs.SetString("UserPhoto", profileUrl);
                        PlayerPrefs.Save();

                        // Show MenuPanel
                        //MenuPanel.SetActive(true);
                        authUIMgr.openMenuPage();
                    }
                });
            }
        });
    }

    private string CheckImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }
        return imageUrl;
    }

    IEnumerator LoadImage(string imageUri)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUri);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            UserProfilePic.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Debug.Log("Image loaded successfully");
        }
        else
        {
            Debug.Log("Error loading image: " + www.error);
        }
    }
}
