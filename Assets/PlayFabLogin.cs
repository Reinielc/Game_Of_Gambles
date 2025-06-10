using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text messageText;
    public Button loginButton;
    public Button signUpButton;

    [Header("PlayFab Settings")]
    public string playFabTitleId = "159ADE";

    private bool isLoggingIn = false;
    private bool isRegistering = false;

    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = playFabTitleId;
        }
    }

    // LOGIN BUTTON
    public void OnLoginButtonClicked()
    {
        if (isLoggingIn || isRegistering) return; // prevent multiple calls

        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username.Length < 3 || password.Length < 6)
        {
            messageText.text = "Username must be at least 3 characters\nPassword at least 6.";
            return;
        }

        isLoggingIn = true;
        loginButton.interactable = false;
        signUpButton.interactable = false;
        messageText.text = "Logging in...";

        var request = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = password
        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailed);
    }

    public void OnSignUpButtonClicked()
    {
        if (isRegistering || isLoggingIn) return; // prevent multiple calls

        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username.Length < 3 || password.Length < 6)
        {
            messageText.text = "Username must be at least 3 characters\nPassword at least 6.";
            return;
        }

        isRegistering = true;
        loginButton.interactable = false;
        signUpButton.interactable = false;
        messageText.text = "Creating account...";

        var registerRequest = new RegisterPlayFabUserRequest
        {
            Username = username,
            Password = password,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,
            result =>
            {
                messageText.text = "Account created! You can now log in.";
                Debug.Log("Registered new user: " + result.PlayFabId);
                isRegistering = false;
                loginButton.interactable = true;
                signUpButton.interactable = true;
            },
            error =>
            {
                messageText.text = "Registration failed: " + error.ErrorMessage;
                Debug.LogError("Registration Error: " + error.GenerateErrorReport());
                isRegistering = false;
                loginButton.interactable = true;
                signUpButton.interactable = true;
            });
    }

    void OnLoginSuccess(LoginResult result)
{
    messageText.text = "Login successful!";
    Debug.Log("Logged in as: " + result.PlayFabId);

    string username = usernameInput.text;
    PlayerPrefs.SetString("CurrentUser", username);

    // ✅ 如果该用户首次登录，初始化余额为 10000
    string balanceKey = username + "_Balance";
    if (!PlayerPrefs.HasKey(balanceKey))
    {
        PlayerPrefs.SetInt(balanceKey, 10000);
        PlayerPrefs.Save();
    }

    isLoggingIn = false;
    SceneManager.LoadScene("GameHub"); 
}


    void OnLoginFailed(PlayFabError error)
    {
        messageText.text = "Login failed: " + error.ErrorMessage;
        Debug.LogWarning("PlayFab Login Error: " + error.GenerateErrorReport());
        isLoggingIn = false;
        loginButton.interactable = true;
        signUpButton.interactable = true;
    }
}
