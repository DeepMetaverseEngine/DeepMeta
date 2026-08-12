package com.onegame;



import java.io.ByteArrayInputStream;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;

import android.opengl.ETC1Util;
import android.opengl.ETC1Util.ETC1Texture;
import android.opengl.GLES20;

public class CompressedTexture 
{
	public static void glCheckError(String msg)
	{
		int errcode = GLES20.glGetError();
		if (errcode != GLES20.GL_NO_ERROR) {
			System.err.println(msg + errcode);
		}
	}

	
	public static int genCompressedTexImage2D(
			int glLevel,
			int glInternalformat, 
			int width,
			int height,
			int border, 
			int imageSize,
			byte[] imgdata,
			int offset,
			int len)
	{
		int glNativeTextureID;
		int genID[] = new int[]{0};
		GLES20.glGenTextures(1, genID, 0);
		glCheckError("genCompressedTexImage2D NativeTextureID Error Code : ");
		glNativeTextureID = genID[0];
		
		glCompressedTexImage2D(glNativeTextureID, glLevel, glInternalformat,
				width, height, border, imageSize, imgdata, offset, len);

		return glNativeTextureID;
	}
	
	public static void glCompressedTexImage2D(
			int glNativeTextureID, 
			int glLevel,
			int glInternalformat, 
			int width,
			int height,
			int border, 
			int imageSize,
			byte[] imgdata,
			int offset,
			int len)
	{
		System.out.println("glCompressedTexImage2D : " + glNativeTextureID);

		ByteBuffer buffer = ByteBuffer.wrap(imgdata, offset, len).order(ByteOrder.nativeOrder());
		GLES20.glBindTexture(GLES20.GL_TEXTURE_2D, glNativeTextureID);
		GLES20.glCompressedTexImage2D(
			GLES20.GL_TEXTURE_2D,
			glLevel, 
			glInternalformat,
			width, 
			height, 
			border,
			imageSize, 
			buffer);

		glCheckError("glCompressedTexImage2D Error Code : ");
	}
	

	public static void uploadPKM(int glNativeTextureID, int w, int h, int fileSize, byte[] pkmData)
	{
		System.out.println("uploadPKM : " + glNativeTextureID);

		ByteArrayInputStream input = new ByteArrayInputStream(pkmData);
		try {
			ETC1Texture etc1 = ETC1Util.createTexture(input);
			GLES20.glBindTexture(GLES20.GL_TEXTURE_2D, glNativeTextureID);
			ETC1Util.loadTexture(GLES20.GL_TEXTURE_2D, 0, 0, GLES20.GL_RGB, GLES20.GL_UNSIGNED_BYTE, etc1);
		} catch (Exception err) {
			err.printStackTrace();
		} finally {
			try {
				input.close();
			} catch (Exception err) {
			}
		}
		glCheckError("uploadPKM Error Code : ");
		
	}
	
}
