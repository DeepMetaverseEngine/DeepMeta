/*
 * ETC Texture Compressor
 *   2013-03-20
 *   M. Kim
 */
#include <jni.h>
#include <android/log.h>

#include <GLES2/gl2.h>
#include <GLES2/gl2ext.h>

#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#include "rg_etc1.h"

#define  LOG_TAG    "unity plugin"
#define  LOGI(...)  __android_log_print(ANDROID_LOG_INFO,LOG_TAG,__VA_ARGS__)
#define  LOGE(...)  __android_log_print(ANDROID_LOG_ERROR,LOG_TAG,__VA_ARGS__)

static void checkGlError(const char* op) {
    for (GLint error = glGetError(); error; error
            = glGetError()) {
        LOGI("after %s() glError (0x%x)\n", op, error);
    }
}

int g_etcInited = 0;

struct compressor_t
{
	GLuint glTex;
	int w;
	int h;
	int count;
	void** in;
	unsigned int* out;
};

extern "C" void* GenTextureCompressor(void* texPtr, int w, int h, int mipmapCount, void* mipmapData[])
{
	if (!g_etcInited)
	{
		rg_etc1::pack_etc1_block_init();
		g_etcInited = 1;
	}

	compressor_t* compressor = new compressor_t;
	
	compressor->glTex = (GLuint)(size_t)(texPtr);
	compressor->w = w;
	compressor->h = h;
	compressor->count = mipmapCount;
	compressor->in = new void*[mipmapCount];
	for (int i = 0; i < mipmapCount; i++)
		compressor->in[i] = mipmapData[i];
	compressor->out = NULL;

	return compressor;
}

extern "C" void CompressTexture(compressor_t* compressor)
{
	int w = compressor->w;
	int h = compressor->h;

	int memSize = 0;

	for (int i = 0; i < compressor->count; i++)
	{
		if (w == 0) w = 1;
		if (h == 0) h = 1;

		int dataSize = (w * h) >> 1; // bytes
		if (dataSize < 8)
			dataSize = 8;

		memSize += dataSize;

		//LOGI("%d x %d: %d", w, h, dataSize);

		w >>= 1;
		h >>= 1;
	}

	compressor->out = new unsigned int[memSize >> 2];

	w = compressor->w;
	h = compressor->h;

	unsigned int* compressed = compressor->out;

	rg_etc1::etc1_pack_params params;
	params.m_quality = rg_etc1::cLowQuality;
	params.m_dithering = false;
	
	for (int i = 0; i < compressor->count ; i++)
	{
		if (w == 0) w = 1;
		if (h == 0) h = 1;
		
		unsigned int* data = (unsigned int*)compressor->in[i];
		
		if (w >= 4 && h >= 4)
		{
			for (int y = 0; y < h / 4; y++)
			{
				for (int x = 0; x < w / 4; x++)
				{
					unsigned int src_blk[16];
					unsigned int* d = src_blk;
					unsigned int* s = &data[y * 4 * w + x * 4];
					
					for (int k = 0; k < 4; k++) 
					{
						*d++ = *s++;
						*d++ = *s++;
						*d++ = *s++;
						*d++ = *s++;

						s += w - 4;

						//if (x == 0 && y == 0)
						//	LOGI("%08x %08x %08x %08x", *(d - 3), *(d - 2), *(d - 1), *d);
					}

					rg_etc1::pack_etc1_block(compressed, src_blk, params);
					compressed += 2;
				}
			}

			//LOGI("[%d] %d x %d [%08xh]: ok", i, w, h, data);
		}
		else
		{
			//LOGI("[%d] %d x %d [%08xh]", i, w, h, data);

			unsigned int p[16];
			for (int j = 0; j < 16; j++)
			{
				p[j] = data[j % (w * h)];
			}
			
			rg_etc1::pack_etc1_block(compressed, p, params);
			compressed += 2;
		}

		w >>= 1;
		h >>= 1;
	}
}

extern "C" void UploadCompressedTexture(compressor_t* compressor)
{
	glBindTexture(GL_TEXTURE_2D, compressor->glTex);
	checkGlError("glBindTexture");

	int w = compressor->w;
	int h = compressor->h;

	unsigned char* data = (unsigned char*)compressor->out;
	
	for (int i = 0; i < compressor->count; i++)
	{
		if (w == 0) w = 1;
		if (h == 0) h = 1;

		int dataSize = (w * h) >> 1; // bytes
		if (dataSize < 8)
			dataSize = 8;

		glCompressedTexImage2D(GL_TEXTURE_2D, i, GL_ETC1_RGB8_OES, w, h, 0, dataSize, data);
		checkGlError("glCompressedTexImage2D");

		data += dataSize;

		w >>= 1;
		h >>= 1;
	}

	delete[] compressor->out;
	delete[] compressor->in;
	delete compressor;
}



extern "C" void UploadCompressedTexImage2D(
			void* texPtr,
			GLuint glLevel,
			GLuint glInternalformat,
			GLuint width,
			GLuint height,
			GLuint border,
			GLuint imageSize,
			void* imgdata)
{
	GLuint glNativeTextureID = (GLuint)(size_t)(texPtr);
	glBindTexture(GL_TEXTURE_2D, glNativeTextureID);
	checkGlError("glBindTexture");

	glCompressedTexImage2D(
			GL_TEXTURE_2D,
			0,
			GL_ETC1_RGB8_OES,
			width,
			height,
			0,
			imageSize,
			imgdata);
	checkGlError("glCompressedTexImage2D");

	LOGI(">>> [%d] <<< (%d x %d) len=%d : ok", glNativeTextureID, width, height, imageSize);

}
