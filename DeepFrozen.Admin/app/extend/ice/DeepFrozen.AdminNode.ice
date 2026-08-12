#pragma once

#include <Ice/Identity.ice>
#include "DeepFrozen.ice"

module DeepFrozenIceImpl
{
    interface IAdminServiceAdapter
    {
        //目前用于和GM工具通信（存在于服务器外部）
		["ami"] ByteArray externalRequest(string fromInfo, ByteArray req) throws RpcException;
    }

};
