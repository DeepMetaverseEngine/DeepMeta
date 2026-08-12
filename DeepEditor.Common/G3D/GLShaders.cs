using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using DeepCore;
using DeepCore.IO;
using static OpenTK.Compute.OpenCL.CLGL;

namespace DeepEditor.Common.G3D
{
    //-------------------------------------------------------------------------------------------------------------------
    public class TintShader : Shader
    {
        string vert = @"
#version 330 core
layout(location = 0) in vec3 aPosition;  
layout(location = 1) in vec4 aColor;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
out vec4 ourColor;
void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    ourColor = aColor;
}
";
        string frag = @"
#version 330 core
out vec4 FragColor;
in vec4 ourColor;
void main()
{
    FragColor = ourColor;
}
";
        public TintShader()
        {
            Load(vert, frag);
        }
    }
    //-------------------------------------------------------------------------------------------------------------------
    public class LightingShader : Shader
    {
        string vert = @"
#version 330 core
layout(location = 0) in vec3 aPosition;  
layout(location = 1) in vec4 aColor;
layout(location = 2) in vec3 aNormal;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
out vec3 ourFragPos;
out vec3 ourColor;
out vec3 ourNormal;
void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    ourFragPos = vec3(vec4(aPosition, 1.0) * model);
    ourColor =  vec3(aColor);
    ourNormal = aNormal * mat3(transpose(inverse(model)));
}
";
        string frag = @"
#version 330 core
out vec4 FragColor;

//In order to calculate some basic lighting we need a few things per model basis, and a few things per fragment basis:
//uniform vec3 objectColor; //The color of the object.
uniform vec3 lightColor; //The color of the light.
uniform vec3 lightPos; //The position of the light.
uniform vec3 viewPos; //The position of the view and/or of the player.

in vec3 ourNormal; //The normal of the fragment is calculated in the vertex shader.
in vec3 ourColor;
in vec3 ourFragPos; //The fragment position.

void main()
{
    //The ambient color is the color where the light does not directly hit the object.
    //You can think of it as an underlying tone throughout the object. Or the light coming from the scene/the sky (not the sun).
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

    //We calculate the light direction, and make sure the normal is normalized.
    vec3 norm = normalize(ourNormal);
    vec3 lightDir = normalize(lightPos - ourFragPos); //Note: The light is pointing from the light to the fragment

    //The diffuse part of the phong model.
    //This is the part of the light that gives the most, it is the color of the object where it is hit by light.
    float diff = max(dot(norm, lightDir), 0.0); //We make sure the value is non negative with the max function.
    vec3 diffuse = diff * lightColor;


    //The specular light is the light that shines from the object, like light hitting metal.
    //The calculations are explained much more detailed in the web version of the tutorials.
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - ourFragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32); //The 32 is the shininess of the material.
    vec3 specular = specularStrength * spec * lightColor;

    //At last we add all the light components together and multiply with the color of the object. Then we set the color
    //and makes sure the alpha value is 1
    vec3 result = (ambient + diffuse + specular) * ourColor;
    FragColor = vec4(result, 1.0);
    
    //Note we still use the light color * object color from the last tutorial.
    //This time the light values are in the phong model (ambient, diffuse and specular)
}
";
        public Vector3 LightPosition { get; set; }
        public Vector3 LightColor { get; set; } = new Vector3(1f, 1f, 1f);
        public LightingShader()
        {
            Load(vert, frag);
        }
        public override void Use(PaintEventArgs3D e)
        {
            base.Use(e);
            //SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
            SetVector3("lightColor", LightColor);
            SetVector3("lightPos", LightPosition);
            SetVector3("viewPos", e.Camera.CamPosition);
        }
    }
    //-------------------------------------------------------------------------------------------------------------------
    public class DirectionalLightingShader : Shader
    {
        string vert = @"
#version 330 core

layout(location = 0) in vec3 aPosition;  
layout(location = 1) in vec4 aColor;
layout(location = 2) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 ourColor;
out vec3 ourNormal;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    ourColor =  vec3(aColor);
    ourNormal = aNormal * mat3(transpose(inverse(model)));  
}
";
        string frag = @"
#version 330 core
struct Material {
    float     shininess;
};
struct Light {
    //For a directional light we dont need the lights position to calculate the direction.
    //Since the direction is the same no matter the position of the fragment we also dont need that.
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;
uniform Material material;
uniform vec3 viewPos;

out vec4 FragColor;

in vec3 ourColor;
in vec3 ourNormal;

void main()
{
    // ambient
    vec3 ambient = light.ambient * ourColor;

    // diffuse 
    vec3 norm = normalize(ourNormal);
    vec3 lightDir = normalize(-light.direction);//We still normalize the light direction since we techically dont know,
                                                    //wether it was normalized for us or not.
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * ourColor;

    // specular
    vec3 viewDir = normalize(viewPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * ourColor;
   
    vec3 result = ambient + diffuse + specular;

    FragColor = vec4(result, 1.0);
}
";
        private Texture owner_DiffuseMap;
        private Texture owner_SpecularMap;
        public Texture DiffuseMap { get; set; }
        public Texture SpecularMap { get; set; }

        public float material_shininess { get; set; } = 100;
        public Vector3 light_direction { get; set; } = new Vector3(-0.5f, -0.8f, -0.5f);
        public Vector3 light_ambient { get; set; } = new Vector3(0.5f);
        public Vector3 light_diffuse { get; set; } = new Vector3(0.8f);
        public Vector3 light_specular { get; set; } = new Vector3(1.0f);

        public DirectionalLightingShader(Texture diffuseMap = null, Texture specularMap = null)
        {
            Load(vert, frag);
            this.DiffuseMap = diffuseMap;
            this.SpecularMap = specularMap;
            if (DiffuseMap == null)
            {
                DiffuseMap = owner_DiffuseMap = Texture.LoadFromBinary(Resource.LoadFromAssembly(GetType().Assembly, "G3D/Resources/container2.png"));
            }
            if (SpecularMap == null)
            {
                SpecularMap = owner_SpecularMap = Texture.LoadFromBinary(Resource.LoadFromAssembly(GetType().Assembly, "G3D/Resources/container2_specular.png"));
            }
        }
        protected override void Disposing()
        {
            base.Disposing();
            owner_DiffuseMap?.Dispose();
            owner_SpecularMap?.Dispose();
        }
        public override void Use(PaintEventArgs3D e)
        {
            base.Use(e);

            SetFloat("material.shininess", material_shininess);

            SetVector3("light.direction", light_direction);
            SetVector3("light.ambient", light_ambient);
            SetVector3("light.diffuse", light_diffuse);
            SetVector3("light.specular", light_specular);

            SetVector3("viewPos", e.Camera.CamPosition);
        }
    }
    //-------------------------------------------------------------------------------------------------------------------
    public class MatDirectionalLightingShader : Shader
    {
        string vert = @"
#version 330 core

layout(location = 0) in vec3 aPosition;  
layout(location = 1) in vec4 aColor;
layout(location = 2) in vec3 aNormal;
layout(location = 3) in vec2 aTexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 ourColor;
out vec3 ourNormal;
out vec2 ourTexCoords;

void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    ourColor =  vec3(aColor);
    ourNormal = aNormal * mat3(transpose(inverse(model)));  
    ourTexCoords = aTexCoords;
}
";
        string frag = @"
#version 330 core
struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float     shininess;
};
struct Light {
    //For a directional light we dont need the lights position to calculate the direction.
    //Since the direction is the same no matter the position of the fragment we also dont need that.
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;
uniform Material material;
uniform vec3 viewPos;

out vec4 FragColor;

in vec3 ourColor;
in vec3 ourNormal;
in vec2 ourTexCoords;

void main()
{
    // ambient
    vec3 ambient = light.ambient * ourColor; //light.ambient * vec3(texture(material.diffuse, ourTexCoords));

    // diffuse 
    vec3 norm = normalize(ourNormal);
    vec3 lightDir = normalize(-light.direction);//We still normalize the light direction since we techically dont know,
                                                    //wether it was normalized for us or not.
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, ourTexCoords));

    // specular
    vec3 viewDir = normalize(viewPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, ourTexCoords));
   
    vec3 result = ambient + diffuse + specular;

    FragColor = vec4(result, 1.0);
}
";
        private Texture owner_DiffuseMap;
        private Texture owner_SpecularMap;
        public Texture DiffuseMap { get; set; }
        public Texture SpecularMap { get; set; }

        public Vector3 material_specular { get; set; } = new Vector3(0.5f, 0.5f, 0.5f);
        public float material_shininess { get; set; } = 100;
        public Vector3 light_direction { get; set; } = new Vector3(-0.5f, -0.8f, -0.5f);
        public Vector3 light_ambient { get; set; } = new Vector3(0.3f);
        public Vector3 light_diffuse { get; set; } = new Vector3(0.8f);
        public Vector3 light_specular { get; set; } = new Vector3(1.0f);

        public MatDirectionalLightingShader(Texture diffuseMap = null, Texture specularMap = null)
        {
            Load(vert, frag);
            this.DiffuseMap = diffuseMap;
            this.SpecularMap = specularMap;
            if (DiffuseMap == null)
            {
                DiffuseMap = owner_DiffuseMap = Texture.LoadFromBinary(Resource.LoadFromAssembly(GetType().Assembly, "G3D/Resources/diffuse.png"));
            }
            if (SpecularMap == null)
            {
                SpecularMap = owner_SpecularMap = Texture.LoadFromBinary(Resource.LoadFromAssembly(GetType().Assembly, "G3D/Resources/specular.png"));
            }
        }
        protected override void Disposing()
        {
            base.Disposing();
            owner_DiffuseMap?.Dispose();
            owner_SpecularMap?.Dispose();
        }
        public override void Use(PaintEventArgs3D e)
        {
            DiffuseMap?.Use(TextureUnit.Texture0);
            SpecularMap?.Use(TextureUnit.Texture1);

            base.Use(e);

            SetInt("material.diffuse", 0);
            SetInt("material.specular", 1);

            SetVector3("material.specular", material_specular);
            SetFloat("material.shininess", material_shininess);

            SetVector3("light.direction", light_direction);
            SetVector3("light.ambient", light_ambient);
            SetVector3("light.diffuse", light_diffuse);
            SetVector3("light.specular", light_specular);

            SetVector3("viewPos", e.Camera.CamPosition);

        }
        public override void EndUse()
        {
            base.EndUse();
            DiffuseMap?.EndUse(TextureUnit.Texture0);
            SpecularMap?.EndUse(TextureUnit.Texture1);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------
}


