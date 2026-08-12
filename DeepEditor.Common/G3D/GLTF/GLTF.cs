using glTFLoader.Schema;
using glTFLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepEditor.Common.G3D
{
    public class GLTFLoader
    {
        public Gltf Content { get; private set; }
        public glTFLoader.Schema.Gltf LoadFile(string filePath)
        {
            if (!filePath.EndsWith("gltf") && !filePath.EndsWith("glb")) return null;

            try
            {
                var deserializedFile = Interface.LoadModel(filePath);

                // read all buffers
                for (int i = 0; i < deserializedFile.Buffers?.Length; ++i)
                {
                    var expectedLength = deserializedFile.Buffers[i].ByteLength;
                    var bufferBytes = deserializedFile.LoadBinaryBuffer(i, filePath);
                }

                // open all images
                for (int i = 0; i < deserializedFile.Images?.Length; ++i)
                {
                    using (var s = deserializedFile.OpenImageFile(i, filePath))
                    {
                        using (var rb = new BinaryReader(s))
                        {
                            uint header = rb.ReadUInt32();
                            if (header == 0x474e5089) continue; // PNG
                            if ((header & 0xffff) == 0xd8ff) continue; // JPEG                            
                        }
                    }
                }

                return deserializedFile;
            }
            catch (Exception e)
            {
                throw new Exception(filePath, e);
            }
        }

    }
}
