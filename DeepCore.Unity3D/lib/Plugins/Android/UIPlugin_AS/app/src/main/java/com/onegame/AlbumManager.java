package com.onegame;

import java.io.File;
import java.nio.IntBuffer;

import javax.microedition.khronos.opengles.GL10;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Environment;
import android.provider.MediaStore;
import android.widget.Toast;

public class AlbumManager {
	
	private static int outImgW, outImgH, compressQuality;
	private static byte apiType;
	
	private static final byte API_TYPE_OPENALBUMS = 0;
	private static final byte API_TYPE_OPENCAMERA = 1;
	
	public static void saveScreenToAlbums(final String  filename, final String path, final Activity context)
	{	 
		context.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				//Toast.makeText(context, filename, Toast.LENGTH_LONG).show();
				//GL10 gl10 = context.getWindow().getDecorView().get().getCurGL10();
				//int width = context.getWindowManager().getDefaultDisplay().getWidth();  
		        //int height = context.getWindowManager().getDefaultDisplay().getHeight();  
				//Bitmap bmp = SavePixels(0, 0, width, height, gl10);
				//Bitmap bmp = takeScreenShot(Cocos2dxActivity.getInstance(), 0, 0, 800, 400);
				//Bitmap bmp = takeScreenShot(Cocos2dxActivity.getInstance());

				/*View view = context.getWindow().getDecorView();
				view.setDrawingCacheEnabled(true);
				view.buildDrawingCache();
				Bitmap bmp = view.getDrawingCache();*/
				Bitmap bitmap = AlbumManager.getDiskBitmap(path + filename);
				MediaStore.Images.Media.insertImage(context.getContentResolver(), bitmap, filename, "King3_ScreenShot");
				
				
				try{
					context.sendBroadcast(new Intent(Intent.ACTION_MEDIA_MOUNTED, Uri.parse("file://" + Environment.getExternalStorageDirectory())));
					Toast.makeText(context, "截图成功", Toast.LENGTH_LONG).show();
				} catch (Exception e) {  
					e.printStackTrace();  
				}  
			}
		});
		//AlbumManager.shoot(Cocos2dxActivity.getInstance(), Environment.getExternalStorageDirectory());
	}
	
	 // 读图
	public static Bitmap getDiskBitmap(String url) {
		Bitmap bitmap = null;
	    try {
	        File file = new File(url);
	        if (file.exists()) {
	            bitmap = BitmapFactory.decodeFile(url);
	
	        }
	    } catch (Exception e) {
	    	e.printStackTrace();
	    }
	    return bitmap;
	 
	}
	
	public static Bitmap SavePixels(int x, int y, int w, int h, GL10 gl)
	{  
	     int b[]=new int[w*(y+h)];
	     int bt[]=new int[w*h];
	     IntBuffer ib=IntBuffer.wrap(b);
	     ib.position(0);
	     gl.glReadPixels(x, 0, w, y+h, GL10.GL_RGBA, GL10.GL_UNSIGNED_BYTE, ib);
	 
	     for(int i=0, k=0; i<h; i++, k++)
	     {
	          //remember, that OpenGL bitmap is incompatible with Android bitmap
	          //and so, some correction need.        
	          for(int j=0; j<w; j++)
	          {
	               int pix=b[i*w+j];
	               int pb=(pix>>16)&0xff;
	               int pr=(pix<<16)&0xffff0000;
	               int pix1=(pix&0xff00ff00) | pr | pb;
	               bt[(h-k-1)*w+j]=pix1;
	          }
	     }
	 
	    Bitmap sb = Bitmap.createBitmap(bt, w, h, Bitmap.Config.ARGB_8888); 
	    return sb;
	}

}
