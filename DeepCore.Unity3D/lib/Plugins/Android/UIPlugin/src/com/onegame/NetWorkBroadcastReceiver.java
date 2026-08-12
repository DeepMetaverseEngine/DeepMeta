package com.onegame;

import com.unity3d.player.UnityPlayer;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public class NetWorkBroadcastReceiver extends BroadcastReceiver 
{

	@Override
	public void onReceive(Context context, Intent intent) 
	{
		int status = NetWorkUtil.getNetWorkType(context);
		UnityPlayer.UnitySendMessage("Main Camera", "AndroidReceive", String.valueOf(status));
	}

}
