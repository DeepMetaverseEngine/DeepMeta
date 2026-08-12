package com.onegame;

import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Build;
import android.os.Environment;
import android.util.Log;
import android.webkit.WebSettings;

import com.unity3d.player.UnityPlayer;

import java.io.File;

/**
 * Created by bowen.meng on 2018/1/18.
 */
public class SystemUtils {

    /**
     * 获取设备型号
     * @return
     */
    public static String getProductModel()
    {
        return android.os.Build.MODEL;
    }

    /**
     * 获取设备UA
     * @param activity
     * @return
     */
    public static String getUserAgent(final android.app.Activity activity){
        String userAgent = "";
        try {
            userAgent = WebSettings.getDefaultUserAgent(activity);
        } catch (Throwable e) {
            try {
                userAgent = System.getProperty("http.agent");
            } catch (Exception e1) {
                userAgent = "DefaultUserAgent";
            }
            Log.e("UnityPlugin","getUserAgent has error. " + e.getMessage());
        }
        return userAgent;
    }


    /**
     * 获取可用存储空间
     * @param path
     * @return
     */
    public static long getDirFreeMemory(String path) {
        if (path == null) {
            return 0;
        }
        try {
            File filePath = new File(path);
            return filePath.getFreeSpace();
        }catch (Exception e){
            Log.e("UnityPlugin","getDirFreeMemory has error " + e.getMessage());
            return 0;
        }
    }

    /**
     * 获取存储空间总大小
     * @param path
     * @return
     */
    public static long getDirTotalMemory(String path) {
        if (path == null) {
            return 0;
        }
        try {
            File filePath = new File(path);
            return filePath.getTotalSpace();
        }catch (Exception e){
            Log.e("UnityPlugin","getDirTotalMemory has error " + e.getMessage());
            return 0;
        }
    }

    /**
     * 获取数据存储目录
     * @param context
     * @return
     */
    public static String getFilePath(Context context) {
        String directoryPath = "";
        try {
            if (Environment.MEDIA_MOUNTED.equals(Environment.getExternalStorageState())) {
                directoryPath = context.getExternalCacheDir().getAbsolutePath();
            } else {
                directoryPath = context.getFilesDir().getAbsolutePath();
            }
        }catch(Exception e){
            Log.e("UnityPlugin","getFilePath has error " + e.getMessage());
        }
        return directoryPath;
    }

    /**
     * 获取粘贴板内容
     * @param activity
     */
    public static void getPasteboard(final android.app.Activity activity) {
        activity.runOnUiThread(new Runnable() {

            @Override
            public void run() {
                String resultString = "";
                int version = Build.VERSION.SDK_INT;
                if (version >= 11){
                    android.content.ClipboardManager cm =(android.content.ClipboardManager)activity.getSystemService(Context.CLIPBOARD_SERVICE);
                    if(cm.hasPrimaryClip()){
                        android.content.ClipData clipData = cm.getPrimaryClip();
                        int count = clipData.getItemCount();
                        for (int i = 0; i < count; ++i) {
                            android.content.ClipData.Item item = clipData.getItemAt(i);
                            CharSequence str = item.coerceToText(activity);
                            resultString += str;
                        }
                    }
                }
                else{
                    android.text.ClipboardManager cm = (android.text.ClipboardManager)activity.getSystemService(Context.CLIPBOARD_SERVICE);
                    if(cm.hasText()){
                        resultString = cm.getText().toString();
                    }
                }
                android.util.Log.i("rerfsf", resultString);
                UnityPlayer.UnitySendMessage("SDKCallBack", "OnPasteboardCB", resultString);
            }
        });
    }


    /**
     * 设置粘贴板内容
     * @param context
     * @param text
     */
    public static void setPasteboard(Context context, String text){
        int version = Build.VERSION.SDK_INT;
        if (version >= 11){
            android.content.ClipboardManager cm =(android.content.ClipboardManager)context.getSystemService(Context.CLIPBOARD_SERVICE);
            android.content.ClipData clip = android.content.ClipData.newPlainText("simple text", text);
            cm.setPrimaryClip(clip);
        }
        else{
            android.text.ClipboardManager cm = (android.text.ClipboardManager)context.getSystemService(Context.CLIPBOARD_SERVICE);
            cm.setText(text);
        }
    }

    public static int getBatteryLeftQuantity(Context context){
        Intent intent = context.registerReceiver(null, new IntentFilter(Intent.ACTION_BATTERY_CHANGED));
        //获取当前电量
        int level = intent.getIntExtra("level", 0);
        //电量的总刻度
        int scale = intent.getIntExtra("scale", 100);

        return level * 100 / scale;
    }
}
