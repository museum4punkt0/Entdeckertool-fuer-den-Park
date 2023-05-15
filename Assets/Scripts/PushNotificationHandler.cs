using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mapbox.Unity.Location;

#if UNITY_IOS
using Unity.Notifications.iOS;
using NotificationSamples.iOS;
#endif

#if UNITY_ANDROID
using UnityEngine.Android;
using Unity.Notifications.Android;
#endif



public class PushNotificationHandler : MonoBehaviour {
    // Start is called before the first frame upd


    public List<Notification> notifications;
    public List<string> times = new List<string>();
    public CrossGameManager crossGameManager;
    public float centerOfSiteLat;
    public float centerOfSiteLng;
    public float allowedDistanceToCenterOfSite;
    public bool hasAssignedNotifications;
    public bool isWaitingForLocationProvider = true;

    public List<System.DateTime> riderTimes = new List<System.DateTime>();
#if UNITY_IOS
    public List<string> identifiers = new List<string>();
    public List<string> identifiersForNotificationsThatWillOccurOutsideOfLocation = new List<string>();
#endif
#if UNITY_ANDROID
    public List<int> identifiers = new List<int>();
    public List<int> identifiersForNotificationsThatWillOccurOutsideOfLocation = new List<int>();
#endif
    public string _title = "Pass auf, da kommt ein römischer Reiter";
    public string _text = "123";
    public string _buttonText = "Klicken Sie hier";
    public NotificationPopUp notificationPopUp;


    public Action<Location> evaluateDistance() {


        crossGameManager = GetComponent<CrossGameManager>();
        double distance = crossGameManager.CalculateDistanceFromCenterToPlayer(centerOfSiteLat, centerOfSiteLng);

        bool isCloseEnough = distance < allowedDistanceToCenterOfSite;
        if (isCloseEnough && !hasAssignedNotifications) {
            crossGameManager.ErrorLog("starts location based assignment");
            ///assigns all notifications
            AssignNotifications();
        } else if (identifiers != null && distance > allowedDistanceToCenterOfSite && hasAssignedNotifications && identifiers.Count > 0) {

            //deletes notifications that only occur on location
            cancelNotifications();
            //crossGameManager.ErrorLog("cancels notifications");

        }
        //crossGameManager.ErrorLog("doesnt assign anything");

        return null;
    }

    IEnumerator Start() {
        crossGameManager = GetComponent<CrossGameManager>();

#if UNITY_ANDROID

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        if (notificationIntentData != null) {

            crossGameManager.ErrorLog("PNH wants to open and find current notificaiton" + notificationIntentData.Id);
        
            showCurrentPopUpWNotificationString(notificationIntentData.Notification.Title);
        }
#endif

#if UNITY_IOS
        
        var notification = iOSNotificationCenter.GetLastRespondedNotification();
        if (notification != null) {
            crossGameManager.ErrorLog("PNH wants to open and find current notificaiton" + notification.Identifier);
            showCurrentPopUpWNotificationString(notification.Title);
        }


#endif


        yield return new WaitForSeconds(5);

        evaluateDistance();

    }

    void cancelNotifications() {
        hasAssignedNotifications = false;
#if UNITY_IOS

            foreach (string identifier in identifiers) {
                iOSNotificationCenter.RemoveScheduledNotification(identifier);
            }

#endif
#if UNITY_ANDROID

        foreach (int identifier in identifiers) {
                AndroidNotificationCenter.CancelNotification(identifier);
            }

    #endif


    }






    void AssignNotifications() {

        notifications = crossGameManager.notifications;
        hasAssignedNotifications = true;

        crossGameManager.ErrorLog("have nots: " + notifications.Count);
        double distance = crossGameManager.CalculateDistanceFromCenterToPlayer(centerOfSiteLat, centerOfSiteLng);
#if UNITY_IOS

        foreach (Notification not in notifications) {


            System.DateTime timing = DateTime.Parse(not.attributes.Timing);
            System.DateTime momentToCallAlert;

            crossGameManager.ErrorLog("from strapi timing:  " + timing.Hour);

            var timeTrigger = new iOSNotificationCalendarTrigger();

            if (not.attributes.Repeats) {
                momentToCallAlert = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, timing.Hour, timing.Minute, 0);
                timeTrigger = new iOSNotificationCalendarTrigger() {
                    Year = System.DateTime.Now.Year,
                    Month = System.DateTime.Now.Month,
                    Day = System.DateTime.Now.Day,
                    Hour = timing.Hour,
                    Minute = timing.Minute,
                    Second = 0,
                    Repeats = false
                };

            } else {
                momentToCallAlert = timing;
                timeTrigger = new iOSNotificationCalendarTrigger() {
                    Year = timing.Year,
                    Month = timing.Month,
                    Day = timing.Day,
                    Hour = timing.Hour,
                    Minute = timing.Minute,
                    Second = 0,
                    Repeats = false
                };
            }

            if (momentToCallAlert > System.DateTime.Now) {

                riderTimes.Add(momentToCallAlert);

                string identifier = not.id.ToString();
                not.IdentifierString = identifier;

                if (not.attributes.onlyOnLocation && distance < allowedDistanceToCenterOfSite) {

                    identifiers.Add(identifier);

                } else {
                    identifiersForNotificationsThatWillOccurOutsideOfLocation.Add(identifier);
                }

                var notification = new iOSNotification() {

                    Identifier = identifier,
                    Title = not.attributes.Title,
                    Body = "Scheduled at: " + DateTime.Now.ToShortDateString() + "",
                    Subtitle = not.attributes.Subtitle,
                    ShowInForeground = true,
                    ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                    CategoryIdentifier = "category_a",
                    ThreadIdentifier = "thread1",
                    Trigger = timeTrigger,
                };

                notification.Data = "{\"title\": \"Notification 1\", \"data\": \"200\"}";

                iOSNotificationCenter.ScheduleNotification(notification);


                iOSNotificationCenter.OnNotificationReceived += notification => {
                    notificationPopUp.Show(not.attributes.Title, not.attributes.Subtitle, not.attributes.ButtonText, not.attributes.Target);
                    iOSNotificationCenter.RemoveDeliveredNotification(identifier);

                };
            }



        }


#endif

#if UNITY_ANDROID



        var c = new AndroidNotificationChannel() {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(c);

        foreach (Notification not in notifications) {

            System.DateTime timing = DateTime.Parse(not.attributes.Timing);
            System.DateTime momentToCallAlert;

            crossGameManager.ErrorLog("from strapi timing:  " + timing.Hour);


            if (not.attributes.Repeats) {
                momentToCallAlert = new System.DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, timing.Hour, timing.Minute, 0);
            } else {
                momentToCallAlert = timing;
            }

            crossGameManager.ErrorLog("has timing:  " + momentToCallAlert);


            if (momentToCallAlert > System.DateTime.Now) {

                riderTimes.Add(momentToCallAlert);

                var notification = new AndroidNotification();
                notification.Title = not.attributes.Title;
                notification.Text = not.attributes.Subtitle;
                notification.FireTime = momentToCallAlert;
                
               

                //notification.IntentData = "{\"title\": \"Notification 1\", \"data\": \"200\"}";

                //AndroidNotificationCenter.SendNotification(notification, "channel_id");
                var identifier = AndroidNotificationCenter.SendNotification(notification, "channel_id");
                not.IdentifierInt = identifier;

                if (not.attributes.onlyOnLocation && distance < allowedDistanceToCenterOfSite) {                    
                    identifiers.Add(identifier);
                } else {
                    identifiersForNotificationsThatWillOccurOutsideOfLocation.Add(identifier);
                }


                crossGameManager.ErrorLog("makes notification:" + momentToCallAlert);


                AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler =
                delegate (AndroidNotificationIntentData data) {
                    var msg = "Notification received : " + data.Id + "\n";
                    msg += "\n Notification received: ";
                    msg += "\n .Title: " + data.Notification.Title;
                    msg += "\n .Body: " + data.Notification.Text;
                    msg += "\n .Channel: " + data.Channel;
                    Debug.Log(msg);
                    crossGameManager.ErrorLog("callback::: notification");


                    notificationPopUp.Show(not.attributes.Title, not.attributes.Subtitle, not.attributes.ButtonText, not.attributes.Target);
                    crossGameManager.loadsWithNotification = true;

                };
           
                AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
            }



        }
#endif


    }







    public void showCurrentPopUp(System.DateTime time) {
        crossGameManager.ErrorLog("shows popup");
        Notification not = notifications.Find(not => not.attributes.Timing == time.ToString());
        notificationPopUp.Show(not.attributes.Title, not.attributes.Subtitle, not.attributes.ButtonText, not.attributes.Target);

    }
    public void showCurrentPopUpWNotificationString(string Identifier) {
        crossGameManager.ErrorLog("shows popup");
        Notification not = notifications.Find(not => not.attributes.Title == Identifier);
        notificationPopUp.Show(not.attributes.Title, not.attributes.Subtitle, not.attributes.ButtonText, not.attributes.Target);

    }
    public void showCurrentPopUpWNotificationInt(int Identifier) {
        Notification not = notifications.Find(not => not.IdentifierInt == Identifier);

        bool hasNot = not != null;

        crossGameManager.ErrorLog("shows popup" + Identifier + hasNot);

        notificationPopUp.Show(not.attributes.Title, not.attributes.Subtitle, not.attributes.ButtonText, not.attributes.Target);

    }



    void Update() {

        if (isWaitingForLocationProvider && crossGameManager != null && crossGameManager.LocationProvider != null && crossGameManager.notifications.Count > 0) {
            isWaitingForLocationProvider = false;
            evaluateDistance();
        }

        foreach (System.DateTime time in riderTimes) {
            if (time == System.DateTime.Now) {
                crossGameManager.ErrorLog("manual check");
                showCurrentPopUp(time);

            }
        }

#if UNITY_ANDROID
        foreach (int identifier in identifiers) {
            if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Delivered) {


                Notification not = notifications.Find(not => not.IdentifierInt == identifier);
                crossGameManager.loadsWithNotification = true;
                crossGameManager.ErrorLog("notHandler::: has sent notification" + not.IdentifierInt);

                notificationPopUp.Show(not.attributes.Title, not.attributes.Subtitle, not.attributes.ButtonText, not.attributes.Target);


                //Remove the notification from the status bar
                AndroidNotificationCenter.CancelNotification(identifier);
                identifiers.Remove(identifier);
            }
        }
        foreach (int identifier in identifiersForNotificationsThatWillOccurOutsideOfLocation) {
            if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Delivered) {

                crossGameManager.loadsWithNotification = true;

                Notification not = notifications.Find(not => not.IdentifierInt == identifier);
                crossGameManager.ErrorLog("notHandler::: has sent notification" + not.IdentifierInt);

                notificationPopUp.Show(not.attributes.Title, not.attributes.Subtitle, not.attributes.ButtonText, not.attributes.Target);

                //Remove the notification from the status bar
                AndroidNotificationCenter.CancelNotification(identifier);
                identifiersForNotificationsThatWillOccurOutsideOfLocation.Remove(identifier);

            }
        }


#endif

    }



}
