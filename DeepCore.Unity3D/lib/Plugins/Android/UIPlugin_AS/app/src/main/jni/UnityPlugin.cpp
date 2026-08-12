#include <jni.h>
#include <android/log.h>

#include <GLES2/gl2.h>
#include <GLES2/gl2ext.h>

#include <stdio.h>
#include <stdlib.h>
#include <math.h>

extern "C" void Argb2Rgba(void* argb, int length)
{
	unsigned char * p_argb = (unsigned char *)argb;
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

