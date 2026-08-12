package com.onegame;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.net.ConnectivityManager;

import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

public class CustomReceiver extends BroadcastReceiver {

    private static final int TYPE_NETWORK = 1;

    private static final int TYPE_BATTERY = 2;

    @Override
    public void onReceive(Context context, Intent intent)
    {
        if (intent != null)
        {
            String action = intent.getAction();

            switch (action)
            {
                case ConnectivityManager.CONNECTIVITY_ACTION:
                    int status = NetWorkUtil.getNetWorkType(context);
                    SendMessage(TYPE_NETWORK, status);
                case Intent.ACTION_BATTERY_CHANGED:
                    int batteryLife = SystemUtils.getBatteryLeftQuantity(context);
                    SendMessage(TYPE_BATTERY, batteryLife);
            }
        }
    }

    private void SendMessage(int type, int value){
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("type",type);
            jsonObject.put("value",value);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        UnityPlayer.UnitySendMessage("AndroidPlugin", "AndroidReceive", jsonObject.toString());
    }
}
