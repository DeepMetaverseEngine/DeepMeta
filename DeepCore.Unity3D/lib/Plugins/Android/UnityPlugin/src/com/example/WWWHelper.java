package com.example;

import android.app.Activity;
import android.content.pm.ApplicationInfo;
import android.content.res.AssetManager;
import java.io.InputStream;
import java.util.HashMap;
import java.util.logging.Level;
import java.util.logging.Logger;

public class WWWHelper {

	private static Logger log = Logger.getLogger("WWWHelper");
	private static AssetManager sAssetManager;
	private static boolean isInit = false;
	private static Activity sActivity = null;
	private static HashMap<String, Boolean> mFileTable = new HashMap<String, Boolean>();

	public static void init(Object activity) {
		if (!isInit) {
			isInit = true;
			try {
				sActivity = (Activity) activity;
				final ApplicationInfo applicationInfo = sActivity.getApplicationInfo();
				WWWHelper.sAssetManager = sActivity.getAssets();
				log.log(Level.INFO, "WWWHelper inited.");
			} catch (Exception err) {
				log.log(Level.WARNING, err.getMessage(), err);
			}
		}
	}
	
	private static String formatPath(String path) {
		path = path.replace('\\', '/');
		path = path.replace("//", "/");
		while (path.startsWith("/")) {
			path = path.substring(1);
		}
		return path;
	}

	@SuppressWarnings("unchecked")
	public static boolean isFileExists(String path) {
		boolean ret = false;
		try {
			path = formatPath(path);
//			log.log(Level.INFO, "isFileExists " + path);
			if (mFileTable.containsKey(path))
				return mFileTable.get(path);
			if (sAssetManager != null) {
				InputStream input = null;
				try {
					input = sAssetManager.open(path);
					try {
						ret = true;
						mFileTable.put(path, true);
					} finally {
						input.close();
					}
				} catch (Throwable e) {
					ret = false;
					mFileTable.put(path, false);
				}
			}
		} catch (Throwable err) {
			log.log(Level.WARNING, err.getMessage(), err);
		}
//		log.log(Level.INFO, "isFileExists " + ret);
		return ret;
	}

	@SuppressWarnings("unchecked")
	public static byte[] getBytes(String path) {
		byte[] mBytes = null;
		try {
			path = formatPath(path);
//			log.log(Level.INFO, "getBytes " + path);
			if (sAssetManager != null) {
				InputStream input = null;
				try {
					input = sAssetManager.open(path);
					try {
						int length = input.available();
						mBytes = new byte[length];
						input.read(mBytes);
					} finally {
						input.close();
					}
					if (!mFileTable.containsKey(path)) {
						mFileTable.put(path, true);
					}
				} catch (Throwable e) {
					if (!mFileTable.containsKey(path)) {
						mFileTable.put(path, false);
					}
				}
			}
		} catch (Throwable err) {
			log.log(Level.WARNING, err.getMessage(), err);
		}
//		if (mBytes != null) {
//			log.log(Level.INFO, "getBytes ok " + mBytes.length);
//		} else {
//			log.log(Level.INFO, "getBytes null ");
//		}
		return mBytes;
	}

	public static String getString(String path) {
		byte[] mBytes = getBytes(path);
		if (mBytes != null) {
			try {
				return new String(mBytes, "utf-8");
			} catch (Exception e) {
			}
			return "";
		}
		return "";
	}
}
