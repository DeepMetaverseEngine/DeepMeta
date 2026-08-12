#include <jni.h>
#include <android/log.h>
#include <android/bitmap.h>

#define  LOG_TAG    "lib_morefun"
#define  LOGI(...)  __android_log_print(ANDROID_LOG_INFO,LOG_TAG,__VA_ARGS__)
#define  LOGE(...)  __android_log_print(ANDROID_LOG_ERROR,LOG_TAG,__VA_ARGS__)

typedef struct
{
    uint8_t alpha;
    uint8_t red;
    uint8_t green;
    uint8_t blue;
} argb;


JNIEXPORT void JNICALL Java_com_morefun_SysFont_convertToRGBA(JNIEnv* env, jclass clazz, jobject buffer, jint length)
{
	void* pixels = env->GetDirectBufferAddress(buffer);
	unsigned char * p_argb = (unsigned char *)pixels;
	unsigned char a;
	for (int i = 0; i < length; i+=4)
	{
		a = p_argb[i];
		p_argb[i  ] = p_argb[i+1];
		p_argb[i+1] = p_argb[i+2];
		p_argb[i+2] = p_argb[i+3];
		p_argb[i+3] = a;
	}
}

JNIEXPORT void JNICALL Java_com_morefun_SysFont_copyPixelsToBufferRGBA(JNIEnv* env, jclass clazz, jobject bitmap, jobject buffer)
{
    AndroidBitmapInfo  info_bitmap;
    int                ret;
    void*              pixelscolor;

    if ((ret = AndroidBitmap_getInfo(env, bitmap, &info_bitmap)) < 0) {
        LOGE("AndroidBitmap_getInfo() failed ! error=%d", ret);
        return;
    }
    if (info_bitmap.format != ANDROID_BITMAP_FORMAT_RGBA_8888) {
        LOGE("Bitmap format is not RGBA_8888 !");
        return;
    }

    if ((ret = AndroidBitmap_lockPixels(env, bitmap, &pixelscolor)) < 0) {
        LOGE("AndroidBitmap_lockPixels() failed ! error=%d", ret);
    }
    {
        for (int y=0; y<info_bitmap.height; y++) {
            argb * line = (argb *) pixelscolor;
            for (int x=0; x<info_bitmap.width; x++) {
                //grayline[x] = 0.3 * line[x].red + 0.59 * line[x].green + 0.11*line[x].blue;
            }
            pixelscolor = (char *)pixelscolor + info_bitmap.stride;
        }
    }
    AndroidBitmap_unlockPixels(env, bitmap);
}

