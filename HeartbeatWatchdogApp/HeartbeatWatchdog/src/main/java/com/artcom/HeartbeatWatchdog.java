package com.artcom;

import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

public class HeartbeatWatchdog {
    private static final String LOG_TAG = HeartbeatWatchdog.class.getSimpleName();

    private Context mContext;
    private int mFrameCount = 0;
    private Thread mTicObserver;

    private static HeartbeatWatchdog instance;

    public static HeartbeatWatchdog instance() {
        if(instance == null) {
            instance = new HeartbeatWatchdog();
        }
        return instance;
    }

    public void setContext(Context context) {
        mContext = context;
    }

    public void startWatchdog(final long timeout) {
        Log.d(LOG_TAG, "starting watchdog");
        mTicObserver = new Thread(new Runnable() {
            private int mLastFrameCount = -1;
            @Override
            public void run() {
                while(true) {
                    if(mLastFrameCount == mFrameCount) {
                        restart();
                    }
                    mLastFrameCount = mFrameCount;
                    try {
                        Thread.sleep(timeout);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
            }
        });
        mTicObserver.start();
        Log.d(LOG_TAG, "watchdog started");
    }


    public void startWatchdog() {
        startWatchdog(3 * 1000);
    }

    public void tic() {
        mFrameCount++;
    }

    public void restart() {
        if(mContext != null) {
            String packageName = mContext.getPackageName();
            Intent mStartActivity = mContext.getPackageManager().getLaunchIntentForPackage(packageName);
            int mPendingIntentId = 422142;
            PendingIntent pendingIntent = PendingIntent.getActivity(mContext, mPendingIntentId, mStartActivity, PendingIntent.FLAG_CANCEL_CURRENT);
            AlarmManager alarmManager = (AlarmManager) mContext.getSystemService(Context.ALARM_SERVICE);
            alarmManager.set(AlarmManager.RTC, System.currentTimeMillis() + 100, pendingIntent);
            Log.d(LOG_TAG, "app will be restarted in 100ms.");
            System.exit(1);
        } else {
            Log.e(LOG_TAG, "no context available. Be sure to call setContext(context)");
        }
    }
}
