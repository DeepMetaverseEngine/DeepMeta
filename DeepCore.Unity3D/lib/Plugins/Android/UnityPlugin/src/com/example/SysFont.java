package com.example;

import java.nio.Buffer;
import java.nio.ByteBuffer;

import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.Bitmap.Config;
import android.graphics.BlendMode;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Paint.Align;
import android.graphics.Paint.FontMetricsInt;
import android.graphics.Paint.Style;
import android.graphics.PaintFlagsDrawFilter;
import android.graphics.Typeface;
import android.opengl.GLES20;
import android.opengl.GLUtils;
import android.text.TextPaint;

public class SysFont {
	public static class FontStyle {
		final public static int STYLE_PLAIN = 0;
		final public static int STYLE_BOLD = 1;
		final public static int STYLE_ITALIC = 2;
		final public static int STYLE_UNDERLINED = 4;
	}

	public static class TextBorderCount {
		final public static int Null = 0;
		final public static int Shadow = 1;
		final public static int Border_4 = 4;
		final public static int Border = 8;
		final public static int Shadow_L_T = 10;
		final public static int Shadow_C_T = 11;
		final public static int Shadow_R_T = 12;
		final public static int Shadow_L_C = 13;
		final public static int Shadow_C_C = 14;
		final public static int Shadow_R_C = 15;
		final public static int Shadow_L_B = 16;
		final public static int Shadow_C_B = 17;
		final public static int Shadow_R_B = 18;
	}

	/** call by unity */
	public static int sysFontTestW;

	/** call by unity */
	public static int sysFontTestH;

	// static native private void copyPixelsToBufferRGBA(Bitmap bitmap, Buffer
	// buffer);
	// static native private void convertToRGBA(Buffer buffer, int length);

	private static float[][] offset_8 = { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 0, 1 },
			/* 1, 1 */{ 2, 1 }, { 0, 2 }, { 1, 2 }, { 2, 2 } };

	private static float[][] offset_4 = { /* 0, 0 */{ 1, 0 }, /* 2, 0 */
			{ 0, 1 }, /* 1, 1 */{ 2, 1 }, /* 0, 2 */{ 1, 2 },/* 2, 2 */
	};

	private static Bitmap font_bitmap;
	private static ByteBuffer font_bitmap_buffer;
	private static Canvas font_bitmap_canvas;
	private static TextPaint paint;
	private static String font_name;
	private static Typeface font_normal;
	private static Typeface font_bold;
	private static Typeface font_italic;
	private static FontMetricsInt metrics = new FontMetricsInt();

	synchronized static  private void init() {
		if (paint == null) {
			paint = new TextPaint();
			paint.setAntiAlias(true);
			paint.setSubpixelText(true);
			paint.setDither(true);
			paint.setTextAlign(Align.LEFT);
			paint.setStyle(Paint.Style.FILL);
			//paint.setBlendMode(BlendMode.SRC_OVER);
			font_normal = Typeface.create(Typeface.DEFAULT, Typeface.NORMAL);
			font_bold = Typeface.create(Typeface.DEFAULT, Typeface.BOLD);
			font_italic = Typeface.create(Typeface.DEFAULT, Typeface.ITALIC);
		}
	}
	synchronized public static void sysSetFontFromAssets(AssetManager assets, String path) {
		init() ;
		try {
			if (path != null && !path.isEmpty() && !path.equalsIgnoreCase(font_name)) {
				Typeface font = Typeface.createFromAsset(assets, path);
				font_normal = font;
				font_bold = Typeface.create(font, Typeface.BOLD);
				font_italic = Typeface.create(font, Typeface.ITALIC);
				font_name = path;
			}
		} catch (Error err) {
			err.printStackTrace();
		}
	}	
	synchronized public static void sysSetFontFromFile(String fontFile) {
		init() ;
		try {			
			if (fontFile != null && !fontFile.isEmpty() && !fontFile.equalsIgnoreCase(font_name)) {
				Typeface font = Typeface.createFromFile(fontFile);
				font_normal = font;
				font_bold = Typeface.create(font, Typeface.BOLD);
				font_italic = Typeface.create(font, Typeface.ITALIC);
				font_name = fontFile;
			}
		} catch (Error err) {
			err.printStackTrace();
		}
	}	
	synchronized public static void sysSetFont(String fontName) {
		init() ;
		try {
			if (fontName != null && !fontName.isEmpty() && !fontName.equalsIgnoreCase(font_name)) {
				Typeface font = Typeface.create(fontName, Typeface.NORMAL);
				font_normal = font;
				font_bold = Typeface.create(font, Typeface.BOLD);
				font_italic = Typeface.create(font, Typeface.ITALIC);
				font_name = fontName;
			}
		} catch (Error err) {
			err.printStackTrace();
		}
	}
	synchronized public static void sysSetFontStyle(int fontStyle, int fontSize) {
		init();
		switch (fontStyle) {
		case FontStyle.STYLE_PLAIN:
			paint.setTypeface(font_normal);
			paint.setUnderlineText(false);
			break;
		case FontStyle.STYLE_BOLD:
			paint.setTypeface(font_bold);
			paint.setUnderlineText(false);
			break;
		case FontStyle.STYLE_ITALIC:
			paint.setTypeface(font_italic);
			paint.setUnderlineText(false);
			break;
		case FontStyle.STYLE_UNDERLINED:
			paint.setTypeface(font_normal);
			paint.setUnderlineText(true);
			break;
		default:
			paint.setTypeface(font_normal);
			paint.setUnderlineText(false);
			break;
		}
		paint.setTextSize(fontSize);
	}

	synchronized static private Bitmap sysFontCreateBitmap(
			String pText,
			int fontColorRGBA, 
			int bgCount,
			int bgColorRGBA, 
			int expectSizeW,
			int expectSizeH) {
		init();
		{
			if (font_bitmap == null) {
				font_bitmap = Bitmap.createBitmap(expectSizeW, expectSizeH, Config.ARGB_8888);
			} else if (font_bitmap.getWidth() < expectSizeW || font_bitmap.getHeight() < expectSizeH) {
				font_bitmap.recycle();
				font_bitmap = Bitmap.createBitmap(expectSizeW, expectSizeH, Config.ARGB_8888);
			}
			font_bitmap.eraseColor(Color.TRANSPARENT);
		}
		{
			if (font_bitmap_canvas == null) {
				font_bitmap_canvas = new Canvas(font_bitmap);
			} else {
				font_bitmap_canvas.setBitmap(font_bitmap);
			}
		}
		Bitmap fontBitmap = font_bitmap;
		Canvas canvas = font_bitmap_canvas;
		canvas.save();
		try {
			canvas.translate(0, expectSizeH);
			canvas.scale(1, -1);
			canvas.translate(0, -metrics.top);
			if (bgCount > 0) {
				int br = (bgColorRGBA >>> 24) & 0xff;
				int bg = (bgColorRGBA >>> 16) & 0xff;
				int bb = (bgColorRGBA >>> 8) & 0xff;
				int ba = (bgColorRGBA >>> 0) & 0xff;
				paint.setARGB(ba, br, bg, bb);
				switch (bgCount) {
				case TextBorderCount.Border_4:
					for (int i = 0; i < offset_4.length; i++) {
						canvas.drawText(pText, offset_4[i][0], offset_4[i][1], paint);
					}
					break;
				case TextBorderCount.Border:
					for (int i = 0; i < offset_8.length; i++) {
						canvas.drawText(pText, offset_8[0][0], offset_8[0][1], paint);
					}
					break;
				case TextBorderCount.Shadow:
					canvas.drawText(pText, 1, 2, paint);
					break;

				case TextBorderCount.Shadow_L_T:
					canvas.drawText(pText, 0, 0, paint);
					break;
				case TextBorderCount.Shadow_C_T:
					canvas.drawText(pText, 1, 0, paint);
					break;
				case TextBorderCount.Shadow_R_T:
					canvas.drawText(pText, 2, 0, paint);
					break;
				case TextBorderCount.Shadow_L_C:
					canvas.drawText(pText, 0, 1, paint);
					break;
				case TextBorderCount.Shadow_C_C:
					break;
				case TextBorderCount.Shadow_R_C:
					canvas.drawText(pText, 2, 1, paint);
					break;
				case TextBorderCount.Shadow_L_B:
					canvas.drawText(pText, 0, 2, paint);
					break;
				case TextBorderCount.Shadow_C_B:
					canvas.drawText(pText, 1, 2, paint);
					break;
				case TextBorderCount.Shadow_R_B:
					canvas.drawText(pText, 2, 2, paint);
					break;
				}
			}
			int fr = (fontColorRGBA >>> 24) & 0xff;
			int fg = (fontColorRGBA >>> 16) & 0xff;
			int fb = (fontColorRGBA >>> 8) & 0xff;
			int fa = (fontColorRGBA >>> 0) & 0xff;
			paint.setARGB(fa, fr, fg, fb);
			canvas.drawText(pText, 1, 1, paint);
		} finally {
			canvas.restore();
		}
		return fontBitmap;
	}

	/**
	 * 获取文字宽高
	 * 
	 * @param pText
	 * @param bgCount
	 * @param expectSizeW
	 * @param expectSizeH
	 * @param outSize
	 * @return
	 */
	public static boolean sysFontTest(
			String pText, 
			int bgCount, 
			int expectSizeW,
			int expectSizeH) {
		init();
		sysFontTestW = 0;
		sysFontTestH = 0;
		paint.getFontMetricsInt(metrics);
		int glyphWidth = (int) paint.measureText(pText, 0, pText.length());
		if (glyphWidth > 0) {
			sysFontTestW = Math.max(expectSizeW, glyphWidth);
			sysFontTestH = Math.max(expectSizeH, metrics.bottom - metrics.top);
			if (bgCount != 0) {
				sysFontTestW += 3;
				sysFontTestH += 3;
			}
			return true;
		}
		return false;
	}

	public static boolean sysFontTexture2(
			String pText, 
			int fontColorRGBA, 
			int bgCount,
			int bgColorRGBA,
			int expectSizeW, 
			int expectSizeH, 
			int glTextureID) {
		Bitmap fontBitmap = sysFontCreateBitmap(pText, fontColorRGBA, bgCount, bgColorRGBA, expectSizeW, expectSizeH);
		if (fontBitmap != null) {
			try {
				GLES20.glBindTexture(GLES20.GL_TEXTURE_2D, glTextureID);
				GLES20.glPixelStorei(GLES20.GL_UNPACK_ALIGNMENT, 4);
				// GLES20.glPixelStorei(GLES20.GL_UNPACK_ALIGNMENT, 1);
				// GLES20.glTexParameteri(GLES20.GL_TEXTURE_2D,
				// GLES20.GL_TEXTURE_MIN_FILTER, GLES20.GL_NEAREST);
				// GLES20.glTexParameteri(GLES20.GL_TEXTURE_2D,
				// GLES20.GL_TEXTURE_MAG_FILTER, GLES20.GL_LINEAR);
				// GLES20.glTexParameteri(GLES20.GL_TEXTURE_2D,
				// GLES20.GL_TEXTURE_WRAP_S, GLES20.GL_CLAMP_TO_EDGE);
				// GLES20.glTexParameteri(GLES20.GL_TEXTURE_2D,
				// GLES20.GL_TEXTURE_WRAP_T, GLES20.GL_CLAMP_TO_EDGE);
				GLUtils.texImage2D(GLES20.GL_TEXTURE_2D, 0, GLES20.GL_RGBA, fontBitmap, 0);
				return true;
			} catch (Exception err) {
				err.printStackTrace();
				return false;
			}
		}
		return false;
	}

	public static byte[] sysFontGetPixels(
			String pText, 
			int fontColorRGBA,
			int bgCount,
			int bgColorRGBA,
			int pixelW,
			int pixelH) {
		Bitmap fontBitmap = sysFontCreateBitmap(pText, fontColorRGBA, bgCount, bgColorRGBA, pixelW, pixelH);
		if (fontBitmap != null) {
			try {
				byte[] rgba = new byte[pixelW * pixelH * 4];
				for (int y = 0; y < pixelH; y++) {
					for (int x = 0; x < pixelW; x++) {
						int pos = (x + y * pixelW) * 4;
						int pix = fontBitmap.getPixel(x, y);
						rgba[pos + 0] = (byte)( Color.red(pix) & 0xFF); // red
						rgba[pos + 1] = (byte)( Color.green(pix) & 0xFF); // green
						rgba[pos + 2] = (byte)( Color.blue(pix) & 0xFF); // blue
						rgba[pos + 3] = (byte)( Color.alpha(pix) & 0xFF); // alpha
					}
				}
				return rgba;
			} catch (Exception err) {
				return null;
			}
		}
		return null;
	}


	// static native private void copyPixelsToBufferRGBA(Bitmap bitmap, Buffer
	// buffer);
	// static native private void convertToRGBA(Buffer buffer, int length);

	// static synchronized private byte[] getPixelsRGBA(Bitmap bitmap) {
	// int length = bitmap.getByteCount();
	// if (font_bitmap_buffer == null) {
	// font_bitmap_buffer = ByteBuffer.allocateDirect(length);
	// } else if (font_bitmap_buffer.capacity() < bitmap.getByteCount()) {
	// font_bitmap_buffer = ByteBuffer.allocateDirect(length);
	// } else {
	// font_bitmap_buffer.position(0);
	// }
	// bitmap.copyPixelsToBuffer(font_bitmap_buffer);
	// convertToRGBA(font_bitmap_buffer, length);
	// return font_bitmap_buffer.array();
	// }

	// static native private void copyPixelsToBufferRGBA(Bitmap bitmap, Buffer
	// buffer);
	// static native private void convertToRGBA(Buffer buffer, int length);

	// static synchronized private byte[] getPixelsRGBA(Bitmap bitmap) {
	// int length = bitmap.getByteCount();
	// if (font_bitmap_buffer == null) {
	// font_bitmap_buffer = ByteBuffer.allocateDirect(length);
	// } else if (font_bitmap_buffer.capacity() < bitmap.getByteCount()) {
	// font_bitmap_buffer = ByteBuffer.allocateDirect(length);
	// } else {
	// font_bitmap_buffer.position(0);
	// }
	// bitmap.copyPixelsToBuffer(font_bitmap_buffer);
	// convertToRGBA(font_bitmap_buffer, length);
	// return font_bitmap_buffer.array();
	// }

}
