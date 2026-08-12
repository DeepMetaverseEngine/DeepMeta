using OpenTK.Mathematics;
using System.Collections.Generic;

namespace DeepEditor.Common.G3D.GLTF
{
    //update on 2020-10-9

    public abstract class MaterialGLTFExtension
    {
        protected string extName;

        public virtual void InitUniform(Uniforms u)
        {}
        public virtual void UpdateUniformsExt(Uniforms u)
        {}

        public virtual void SetupProperty(List<Texture2D> textures, object obj)
        {

        }

        public static MaterialGLTFExtension FatoryCreate(string name)
        {
            MaterialGLTFExtension mtl = null;
            switch(name)
            {
                case "KHR_materials_clearcoat":
                mtl = new ClearCoatMaterial();
                break;
                case "KHR_materials_pbrSpecularGlossiness":
                mtl = new PBRSpecularGlossinessMaterial();
                break;
                case "KHR_materials_transmission":
                mtl = new TransmissionMaterial();
                break;
                case "KHR_materials_unlit":
                mtl = new UnlitMaterial();
                break;
                case "KHR_materials_sheen":
                mtl = new SheenMaterial();
                break;
                case "KHR_material_variants":
                mtl = new VariantMaterial();
                break;
            }
            return mtl;
        }
    }

    //https://github.com/KhronosGroup/glTF/tree/master/extensions/2.0/Khronos/KHR_materials_clearcoat
    public class ClearCoatMaterial:MaterialGLTFExtension
    {//透明涂层材质
        public ClearCoatMaterial()
        {
            extName = "KHR_materials_clearcoat";
        }
        
        public Texture2D clearcoatTexture;
        public Texture2D clearcoatRoughnessTexture;
        public float clearcoatFactor;//diffuseFactor
        public float clearcoatRoughnessFactor;//specularFactor
        public Texture2D clearcoatNormalTexture;
    }

    //https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/README.md#appendix-b-brdf-implementation
    //https://github.com/KhronosGroup/glTF/tree/master/extensions/2.0/Khronos/KHR_materials_pbrSpecularGlossiness
    public class PBRSpecularGlossinessMaterial:MaterialGLTFExtension
    {//镜面光滑材质
        public PBRSpecularGlossinessMaterial()
        {
            extName="KHR_materials_pbrSpecularGlossiness";
        }
        public Texture2D diffuseTexture;
        public Texture2D specularGlossinessTexture;
        public Vector4 diffuseColor;//diffuseFactor
        public Vector3 specularColor;//specularFactor
        public float glossinessFactor;

        public override void InitUniform(Uniforms u)
        {
            u.RegName("DiffuseTexture");
            u.RegName("SpecularGlossinessTexture");
            u.RegName("DiffuseColor");//default:[1,1,1]
            u.RegName("SpecularColor");//default:[1,1,1]
            u.RegName("GlossinessFactor");//default:1

            diffuseColor = new Vector4(1,1,1,1);
            specularColor = new Vector3(1,1,1);
            glossinessFactor = 1;
        }
        public override void UpdateUniformsExt(Uniforms u)
        {
            u.UpdateUniformTexture("DiffuseTexture",diffuseTexture);
            u.UpdateUniformTexture("SpecularGlossinessTexture",specularGlossinessTexture);
            u.UpdateUniform("DiffuseColor",diffuseColor);
            u.UpdateUniform("SpecularColor",specularColor);
            u.UpdateUniform("GlossinessFactor",glossinessFactor);
        }

        public override void SetupProperty(List<Texture2D> textures, object obj)
        {
            Newtonsoft.Json.Linq.JObject jobj = obj as Newtonsoft.Json.Linq.JObject;
            // Console.WriteLine(jobj.ToString());
            Newtonsoft.Json.Linq.JToken token;
            if(jobj.TryGetValue("diffuseTexture",out token))
            {
                Newtonsoft.Json.Linq.JToken jtIndex = token["index"];
                int index = jtIndex.ToObject<int>();
                diffuseTexture = textures[index];
            }
            if(jobj.TryGetValue("specularGlossinessTexture",out token))
            {
                Newtonsoft.Json.Linq.JToken jtIndex = token["index"];
                int index = jtIndex.ToObject<int>();
                specularGlossinessTexture = textures[index];
            }
            if(jobj.TryGetValue("diffuseFactor",out token))
            {
                float[] color = token.ToObject<float[]>();
                diffuseColor = Material.ToVector(color);
            }
            if(jobj.TryGetValue("specularFactor",out token))
            {
                float[] color = token.ToObject<float[]>();
                specularColor = Material.ToVector(color).Xyz;
            }
            if(jobj.TryGetValue("glossinessFactor",out token))
            {
                glossinessFactor = token.ToObject<float>();
            }
        }
    }

    //status:draft
    //https://github.com/KhronosGroup/glTF/tree/master/extensions/2.0/Khronos/KHR_materials_transmission
    public class TransmissionMaterial:MaterialGLTFExtension
    {
        public TransmissionMaterial()
        {
            extName = "KHR_materials_transmission";
        }
        public Texture2D transmissionTexture;
        public float transmissionFactor;//diffuseFactor


    }

    //https://github.com/KhronosGroup/glTF/tree/master/extensions/2.0/Khronos/KHR_materials_unlit
    public class UnlitMaterial:MaterialGLTFExtension
    {//
        public UnlitMaterial()
        {
            extName = "KHR_materials_unlit";
        }

    }

    //status:draft
    //https://github.com/KhronosGroup/glTF/tree/master/extensions/2.0/Khronos/KHR_materials_sheen
    public class SheenMaterial:MaterialGLTFExtension
    {
        public SheenMaterial()
        {
            extName="KHR_materials_sheen";
        }
        public Texture2D sheenColorTexture;
        public Texture2D sheenRoughnessTexture;
        public Color4 sheenColorFactor;
        public float sheenRoughnessFactor;
        

    }

    //status:draft
    //https://github.com/KhronosGroup/glTF/tree/master/extensions/2.0/Khronos/KHR_materials_variants
    public class VariantMaterial:MaterialGLTFExtension
    {
        public VariantMaterial()
        {
            extName="KHR_materials_sheen";
        }
        Dictionary<int,int[]> mapping;//material-variants
    }
}