using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.PlayerAccounts.Samples
{
    internal class PlayerAccountsDemo : MonoBehaviour
    {
        //[SerializeField] private Text m_AccessTokenText;

        [SerializeField] private TMP_Text m_StatusText;

        //[SerializeField] private GameObject m_TokenPanel;
        [SerializeField] private Button m_LoginButton;

        [SerializeField] private GameObject cloudSlot1;
        [SerializeField] private GameObject cloudSlot2;
        [SerializeField] private GameObject cloudSlotSignIn;
        [SerializeField] private SettingsManager settingsManager;

        private async void Awake()
        {
            await UnityServices.InitializeAsync();
            PlayerAccountService.Instance.SignedIn += UpdateUI;

            if (AuthenticationService.Instance.SessionTokenExists) SignInWithUnityMethod();

            try
            {
                await UnityServices.InitializeAsync();
                var consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            }
            catch (ConsentCheckException e)
            {
                // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
            }
        }


        public async void SignInWithUnityMethod()
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                m_LoginButton.gameObject.SetActive(false);
                cloudSlot1.SetActive(true);
                cloudSlot2.SetActive(true);
                cloudSlotSignIn.SetActive(false);
                m_StatusText.text = "Signed In";
                settingsManager.RetrieveKeys();
                return;
            }

            if (AuthenticationService.Instance.SessionTokenExists)
            {
                SignInCachedUser();
            }
            else
            {
                await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
                m_StatusText.text = "<b>Request Successful!</b>";
            }
        }

        private async void SignInCachedUser()
        {
            // Check if a cached user already exists by checking if the session token exists
            if (!AuthenticationService.Instance.SessionTokenExists)
                // if not, then do nothing
                return;

            // Sign in Anonymously
            // This call will sign in the cached user.
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");
                m_StatusText.text = "<b>Sign in Successful!</b>";
                m_LoginButton.gameObject.SetActive(false);
                cloudSlot1.SetActive(true);
                cloudSlot2.SetActive(true);
                cloudSlotSignIn.SetActive(false);
                settingsManager.RetrieveKeys();

                // Shows how to get the playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }


        public async void StartSignInAsync()
        {
            await PlayerAccountService.Instance.StartSignInAsync();
        }

        public async void RefreshToken()
        {
            await PlayerAccountService.Instance.RefreshTokenAsync();
            UpdateUI();
        }

        public void SignOut()
        {
            AuthenticationService.Instance.SignOut(true);
            PlayerAccountService.Instance.SignOut();
            m_LoginButton.gameObject.SetActive(true);
            cloudSlot1.SetActive(false);
            cloudSlot2.SetActive(false);
            cloudSlotSignIn.SetActive(true);
            m_StatusText.text = "Signed Out";
        }

        private void UpdateUI()
        {
            m_StatusText.text = "<b>Request Successful!</b>";
            m_LoginButton.gameObject.SetActive(false);
            cloudSlot1.SetActive(true);
            cloudSlot2.SetActive(true);
            cloudSlotSignIn.SetActive(false);
            SignInWithUnityMethod();
        }


        public void DeleteAllCloudData()
        {
            DeleteKeys();
        }

        public void DeleteUnityAccount()
        {
            Application.OpenURL("https://player-account.unity.com/");
        }

        private async void DeleteKeys()
        {
            var keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();

            for (var i = 0; i < keys.Count; i++)
            {
                await CloudSaveService.Instance.Data.ForceDeleteAsync(keys[i]);
                Debug.Log(keys[i]);
            }

            settingsManager.RetrieveKeys();
        }
    }
}