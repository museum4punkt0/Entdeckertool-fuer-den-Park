using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_IOS
using UnityEngine.iOS;
using Unity.Notifications.iOS;

#endif
#if UNITY_ANDROID
using UnityEngine.Android;

#endif

public class AppPermissions : MonoBehaviour
{

    public introSceneButtons introSceneButtons;
    public bool IsTesting;

    private void OnEnable() {
        introSceneButtons = GetComponent<introSceneButtons>();
    }
    internal void CamPermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName) {
        Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");
    }

    internal void CamPermissionCallbacks_PermissionGranted(string permissionName) {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");

        #if UNITY_IOS
                    
                StartCoroutine(RequestAuthorizationToSendPushNotifications());

        #endif

        #if UNITY_ANDROID
            
            introSceneButtons.SceneKamera();

        #endif
    }

    internal void CamPermissionCallbacks_PermissionDenied(string permissionName) {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
    }

    internal void LocPermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName) {
        Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");
    }

    internal void LocPermissionCallbacks_PermissionGranted(string permissionName) {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");

        print("has access to location");

        introSceneButtons.SceneStandortFadeOut();
       
    }

    internal void LocPermissionCallbacks_PermissionDenied(string permissionName) {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
    }




    public void RquestCameraAccess() {

#if UNITY_IOS

        StartCoroutine(IOSCameraRequest());

        #endif

        #if UNITY_ANDROID

            if (UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera)) {
                CamPermissionCallbacks_PermissionGranted("");
            } else {
                 var callbacks = new PermissionCallbacks();
                 callbacks.PermissionDenied += CamPermissionCallbacks_PermissionDenied;
                 callbacks.PermissionGranted += CamPermissionCallbacks_PermissionGranted;
                 callbacks.PermissionDeniedAndDontAskAgain += CamPermissionCallbacks_PermissionDeniedAndDontAskAgain;
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera, callbacks);
            }

        #endif

        print("RquestCameraAccess");
    }

    public void RquestLocationAccess() {

#if UNITY_IOS
                StartCoroutine(IOSlocationRequest());
#endif

#if UNITY_ANDROID


        if (UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation)) {
                    LocPermissionCallbacks_PermissionGranted("");
                } else {
                    var callbacks = new PermissionCallbacks();
                    callbacks.PermissionDenied += LocPermissionCallbacks_PermissionDenied;
                    callbacks.PermissionGranted += LocPermissionCallbacks_PermissionGranted;
                    callbacks.PermissionDeniedAndDontAskAgain += LocPermissionCallbacks_PermissionDeniedAndDontAskAgain;
                    UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation, callbacks);
             }

    #endif

   }



#if UNITY_IOS


     IEnumerator IOSlocationRequest(){
        // // Check if the user has location service enabled.

                print("checks access");
                if (IsTesting) {
                    LocPermissionCallbacks_PermissionGranted("");

                }


                if (Input.location.isEnabledByUser){
                    LocPermissionCallbacks_PermissionGranted("");
                    yield break; 
                
                }
                    


                // Starts the location service.
                Input.location.Start();

                // Waits until the location service initializes
                int maxWait = 200;
                while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
                {

                    if(Input.location.status == LocationServiceStatus.Running){
                         LocPermissionCallbacks_PermissionGranted("");
                    }
                    yield return new WaitForSeconds(1);
                    maxWait--;

                    if (Input.location.status == LocationServiceStatus.Failed)
                    {
                         print("Unable to determine device location");
                         yield break;
                     }
                }

      }

      void FindWebCams() {
            foreach (var device in WebCamTexture.devices) {
                Debug.Log("Name: " + device.name);
            }
        }

      IEnumerator IOSCameraRequest() {
            FindWebCams();

            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (Application.HasUserAuthorization(UserAuthorization.WebCam)) {
                Debug.Log("webcam found");

                CamPermissionCallbacks_PermissionGranted("");


            } else {
                Debug.Log("webcam not found");
            }
        }




      IEnumerator RequestAuthorizationToSendPushNotifications(){

        if (IsTesting) {
            introSceneButtons.SceneKamera();
        }
        using (var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true))
          {
          while (!req.IsFinished)
              {

          
                yield return null;
              };

           if(req.Granted){

             introSceneButtons.SceneKamera();

          }
          string res = "\n RequestAuthorization: \n";
          res += "\n finished: " + req.IsFinished;
          res += "\n granted :  " + req.Granted;
          res += "\n error:  " + req.Error;
          res += "\n deviceToken:  " + req.DeviceToken;
          Debug.Log(res);
         
          }

     
      }

#endif
}

