using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.PlayerAccounts;
using UnityEngine;

public class SignInWithUnity : MonoBehaviour
{
    private void Start()
    {
        InitializeUnityCloudSave();
        
    }

    // Update is called once per frame
    private async void InitializeUnityCloudSave()
    {
        await UnityServices.InitializeAsync();
    }

    public async void SignInWithUnityMethod()
    {
        if (AuthenticationService.Instance.SessionTokenExists)
        {
            SignInCachedUser();
        }
        else
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
        }
    }
    
   async void SignInCachedUser()
    {
        // Check if a cached user already exists by checking if the session token exists
        if (!AuthenticationService.Instance.SessionTokenExists) 
        {
            // if not, then do nothing
            return;
        }

        // Sign in Anonymously
        // This call will sign in the cached user.
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

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
}