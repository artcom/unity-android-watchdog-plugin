using UnityEngine;
using System.Collections;

namespace Artcom {
    public class UnityHeartbeat : MonoBehaviour {

        private AndroidJavaObject toastExample = null;
        private AndroidJavaObject activityContext = null;

        void Start() {
            if (toastExample == null) {
                using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                    activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                }

                using(AndroidJavaClass pluginClass = new AndroidJavaClass("com.artcom.HeartbeatWatchdog")) {
                    Debug.Log(pluginClass);
                    if (pluginClass != null) {
                        toastExample = pluginClass.CallStatic<AndroidJavaObject>("instance");
                        toastExample.Call("setContext", activityContext);
                        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                            toastExample.Call("startWatchdog");
                        }));
                    }
                }
            }
        }

        void Update() {
            toastExample.Call("tic");
        }

        public void ForceHangNow() {
            Debug.Log("Clicked");
            // Loop forever - yes we want this
            for (;;) {
            }
        }
    }

}