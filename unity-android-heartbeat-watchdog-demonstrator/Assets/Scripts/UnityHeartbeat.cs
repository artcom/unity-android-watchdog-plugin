using UnityEngine;
using System.Collections;

namespace Artcom {
    public class UnityHeartbeat : MonoBehaviour {

        private AndroidJavaObject watchdog = null;
        private AndroidJavaObject activityContext = null;

        void Start() {
            if (watchdog == null) {
                using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                    activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                }

                using(AndroidJavaClass pluginClass = new AndroidJavaClass("com.artcom.HeartbeatWatchdog")) {
                    Debug.Log(pluginClass);
                    if (pluginClass != null) {
                        watchdog = pluginClass.CallStatic<AndroidJavaObject>("instance");
                        watchdog.Call("setContext", activityContext);
                        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                            watchdog.Call("startWatchdog");
                        }));
                    }
                }
            }
        }

        void Update() {
            watchdog.Call("tic");
        }

        public void ForceHangNow() {
            Debug.Log("Clicked");
            // Loop forever - yes we want this
            for (;;) {
            }
        }
    }
}
