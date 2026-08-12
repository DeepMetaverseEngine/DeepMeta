package com.example;

import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.BatteryManager;
import android.telephony.TelephonyManager;
import android.text.TextUtils;
import android.util.Base64;

import java.net.NetworkInterface;
import java.util.Enumeration;

import com.unity3d.player.UnityPlayer;

public class NetWorkUtil {
	/** 没有网络 */
	public static final int NETWORKTYPE_INVALID = 0;
	/** wifi网络 */
	public static final int NETWORKTYPE_WIFI = 1;
	/** 2G网络 */
	public static final int NETWORKTYPE_2G = 2;
	/** 3G�?3G以上网络，或统称为快速网�? */
	public static final int NETWORKTYPE_3G = 3;
	/** 4G LTE网络 */
	public static final int NETWORKTYPE_4G = 4;
	/** wap网络 */
	public static final int NETWORKTYPE_WAP = 5;

	private static NetWorkBroadcastReceiver NetworkReceiver = null;

	public static void crateNetWorkBroadcast(final Context context) {
		if (NetworkReceiver == null) {
			NetworkReceiver = new NetWorkBroadcastReceiver();
			IntentFilter filter = new IntentFilter();
			filter.addAction("android.net.conn.CONNECTIVITY_CHANGE");
			context.registerReceiver(NetworkReceiver, filter);
		}
	}

	public static void destoryNetWorkBroadcast(final Context context) {
		if (NetworkReceiver != null) {
			context.unregisterReceiver(NetworkReceiver);
		}
	}

	/**
	 * 获取本机电量
	 *
	 * @return float百分�?
	 */
	public static float getBatteryLevel() {
		Intent batteryIntent = UnityPlayer.currentActivity.registerReceiver(null,
				new IntentFilter(Intent.ACTION_BATTERY_CHANGED));
		int level = batteryIntent.getIntExtra(BatteryManager.EXTRA_LEVEL, -1);
		int scale = batteryIntent.getIntExtra(BatteryManager.EXTRA_SCALE, -1);
		if (level == -1 || scale == -1) {
			return -1;
		}

		return ((float) level / (float) scale) * 100.0f;
	}

	/**
	 * 获取网络状�?�，wifi,wap,2g,3g,4g
	 *
	 * @param context
	 *            上下�?
	 * @return int 网络状�??
	 */
	public static int getNetWorkType(final Context context) {
		int mNetWorkType = 0;
		ConnectivityManager manager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
		NetworkInfo networkInfo = manager.getActiveNetworkInfo();

		if (networkInfo != null && networkInfo.isConnected()) {
			String type = networkInfo.getTypeName();

			if (type.equalsIgnoreCase("WIFI")) {
				mNetWorkType = NETWORKTYPE_WIFI;
			} else if (type.equalsIgnoreCase("MOBILE")) {
				String hostName = System.getProperty("http.proxyHost");
				if (TextUtils.isEmpty(hostName)) {
					mNetWorkType = isFastMobileNetwork(context);
				} else {
					mNetWorkType = NETWORKTYPE_WAP;
				}
			}
		} else {
			mNetWorkType = NETWORKTYPE_INVALID;
		}

		return mNetWorkType;
	}

	public static String getLocalMacAddress(final Context context) {
		String macaddress = "00:00:00:00:00";
		try {
			String wifiInterfaceName = "wlan0";
			Enumeration<NetworkInterface> interfaces = NetworkInterface.getNetworkInterfaces();
			while (interfaces.hasMoreElements()) {
				NetworkInterface iF = interfaces.nextElement();
				if (iF.getName().equalsIgnoreCase(wifiInterfaceName)) {
					byte[] addr = iF.getHardwareAddress();
					if (addr == null || addr.length == 0) {
						return null;
					}

					StringBuilder buf = new StringBuilder();
					for (byte b : addr) {
						buf.append(String.format("%02X:", b));
					}
					if (buf.length() > 0) {
						buf.deleteCharAt(buf.length() - 1);
					}
					macaddress = buf.toString();
					break;
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}

		String model = android.os.Build.MODEL;
		String result = "{\"mac\":\"" + macaddress + "\",\"model\":\"" + model + "\"}";
		byte[] enaddress = Base64.encode(result.getBytes(), Base64.NO_WRAP);
		return "android-" + new String(enaddress);
	}

	private static int isFastMobileNetwork(Context context) {
		TelephonyManager telephonyManager = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
		switch (telephonyManager.getNetworkType()) {
		case TelephonyManager.PHONE_TYPE_GSM:
		case TelephonyManager.PHONE_TYPE_CDMA:
		case TelephonyManager.NETWORK_TYPE_CDMA:
		case TelephonyManager.NETWORK_TYPE_1xRTT:
		case TelephonyManager.NETWORK_TYPE_IDEN:
			return NETWORKTYPE_2G; // ~ 50-100 kbps
		case TelephonyManager.NETWORK_TYPE_UMTS:
		case TelephonyManager.NETWORK_TYPE_EVDO_0:
		case TelephonyManager.NETWORK_TYPE_EVDO_A:
		case TelephonyManager.NETWORK_TYPE_HSUPA:
		case TelephonyManager.NETWORK_TYPE_HSPA:
		case TelephonyManager.NETWORK_TYPE_EVDO_B:
		case TelephonyManager.NETWORK_TYPE_EHRPD:
		case TelephonyManager.NETWORK_TYPE_HSPAP:
			return NETWORKTYPE_3G; // ~ 400-1000 kbps
		case TelephonyManager.NETWORK_TYPE_LTE:
			return NETWORKTYPE_4G; // ~ 10+ Mbps
		case TelephonyManager.NETWORK_TYPE_UNKNOWN:
			return NETWORKTYPE_4G;
		default:
			return NETWORKTYPE_4G;
		}
	}
}
