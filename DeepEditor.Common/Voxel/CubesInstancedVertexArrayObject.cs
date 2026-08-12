using DeepEditor.Common.G3D;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace DeepEditor.Common.Voxel
{
    public class CubesInstancedVertexArrayObject : VertexArrayObject
    {
        private float gridSize;
        private int vbo_offset = 0;
        private List<Vector3> offsets = new List<Vector3>();
        private int instance_count = 0;
        public CubesInstancedVertexArrayObject(float gridSize) : base(OpenTK.Graphics.OpenGL.PrimitiveType.Quads, Color4.White)
        {
            this.gridSize = gridSize;
        }
        public sealed override void Add(Vector3 vec)
        {
            throw new NotSupportedException(nameof(vec));
        }
        protected override bool OnCheckDirty(out int arrayLength)
        {
            arrayLength = offsets.Count;
            return offsets.Count > 0;
        }
        protected override void OnClean()
        {
            base.OnClean();
            offsets.Clear();
            offsets.TrimExcess();
        }
        protected override void OnFlush()
        {
            base.OnFlush();
            if (vbo_offset > 0) GL.DeleteBuffer(vbo_offset);
            vbo_offset = 0;
        }
        public void AddCube2D(Vector3 pos, Color4 color)
        {
            offsets.Add(new Vector3(pos.X, pos.Z, pos.Y));
            colors.Add(color);
        }

        protected override void OnInit(int arrayLength)
        {
            instance_count = arrayLength * 24;
            this.vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            {
                var _vertices = GetCubeBuffe(gridSize);
                this.vertexBufferObject_Vertices = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_Vertices);
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
                GL.EnableVertexAttribArray(VAO_POINTS_LOCATION);
                GL.VertexAttribPointer(VAO_POINTS_LOCATION, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
                GL.EnableVertexAttribArray(VAO_NORMAL_LOCATION);
                GL.VertexAttribPointer(VAO_NORMAL_LOCATION, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(VAO_UV_LOCATION);
                GL.VertexAttribPointer(VAO_UV_LOCATION, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

                this.vertexBufferObject_Colors = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_Colors);
                GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * 4 * sizeof(float), colors.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(VAO_COLORS_LOCATION, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
                GL.EnableVertexAttribArray(VAO_COLORS_LOCATION);

                this.vbo_offset = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_offset);
                GL.BufferData(BufferTarget.ArrayBuffer, arrayLength * 3 * sizeof(float), offsets.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(VAO_OFFSET_LOCATION, 4, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(VAO_OFFSET_LOCATION);

                GL.VertexAttribDivisor(VAO_COLORS_LOCATION, 1); // tell OpenGL this is an instanced vertex attribute.
                GL.VertexAttribDivisor(VAO_OFFSET_LOCATION, 1); // tell OpenGL this is an instanced vertex attribute.
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        protected override void OnDraw()
        {
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArraysInstanced(objectType, 0, 24, instance_count);
            GL.BindVertexArray(0);
        }
        protected override void OnDraw(PaintEventArgs3D e)
        {
            if (_shader != null)
            {
                try
                {
                    _shader.Use(e);
                    _shader.SetMatrix4("model", e.ModelMatrix);
                    _shader.SetMatrix4("view", e.Camera.ViewMatrix);
                    _shader.SetMatrix4("projection", e.Camera.ProjectionMatrix);
                    GL.BindVertexArray(vertexArrayObject);
                    GL.DrawArraysInstanced(objectType, 0, 24, instance_count);
                    GL.BindVertexArray(0);
                }
                finally
                {
                    _shader.EndUse();
                }
            }
            else
            {
                GL.BindVertexArray(vertexArrayObject);
                GL.DrawArraysInstanced(objectType, 0, 24, instance_count);
                GL.BindVertexArray(0);
            }
        }

        public const int VAO_OFFSET_LOCATION = 4; // instance
        public static float[] GetCubeBuffe(float g)
        {
            var verts = new float[] { 
            // Positions  Normals     Texture coords
            // FORTH                  
            0, 0, g,       0, 0, 1,   0.0f, 0.0f,
            g, 0, g,       0, 0, 1,   1.0f, 0.0f,
            g, g, g,       0, 0, 1,   1.0f, 1.0f,
            0, g, g,       0, 0, 1,   0.0f, 1.0f,
            // BACK                   
            0, 0, 0,       0, 0,-1,   0.0f, 0.0f,
            g, 0, 0,       0, 0,-1,   1.0f, 0.0f,
            g, g, 0,       0, 0,-1,   1.0f, 1.0f,
            0, g, 0,       0, 0,-1,   0.0f, 1.0f,
            // LEFT                   
            0, 0, 0,      -1, 0, 0,   0.0f, 0.0f,
            0, g, 0,      -1, 0, 0,   1.0f, 0.0f,
            0, g, g,      -1, 0, 0,   1.0f, 1.0f,
            0, 0, g,      -1, 0, 0,   0.0f, 1.0f,
            // RIGHT                  
            g, 0, 0,       1, 0, 0,   0.0f, 0.0f,
            g, g, 0,       1, 0, 0,   1.0f, 0.0f,
            g, g, g,       1, 0, 0,   1.0f, 1.0f,
            g, 0, g,       1, 0, 0,   0.0f, 1.0f,
            // TOP                    
            0, g, 0,       0, 1, 0,   0.0f, 0.0f,
            g, g, 0,       0, 1, 0,   1.0f, 0.0f,
            g, g, g,       0, 1, 0,   1.0f, 1.0f,
            0, g, g,       0, 1, 0,   0.0f, 1.0f,
            // Bottom                 
            0, 0, 0,       0,-1, 0,   0.0f, 0.0f,
            g, 0, 0,       0,-1, 0,   1.0f, 0.0f,
            g, 0, g,       0,-1, 0,   1.0f, 1.0f,
            0, 0, g,       0,-1, 0,   0.0f, 1.0f,
            };
            return verts;
        }

        public override Shader CreateDefaultShader()
        {
            return new PointLightingShader();
        }
        public class PointLightingShader : Shader
        {
            string vert = @"
#version 330 core

layout(location = 0) in vec3 aPosition;  
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoords;
layout(location = 3) in vec4 aColor;
layout(location = 4) in vec3 aOffset;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 ourColor;
out vec3 ourNormal;
out vec2 ourTexCoords;
out vec3 ourFragPos;

void main(void)
{
    gl_Position = vec4(aPosition + aOffset, 1.0) * model * view * projection;
    ourColor =  vec3(aColor);
    ourNormal = aNormal * mat3(transpose(inverse(model)));  
    ourTexCoords = aTexCoords;    
    ourFragPos = vec3(vec4(aPosition + aOffset, 1.0) * model);
}
";
            string frag = @"
#version 330 core
out vec4 FragColor;

uniform vec3 lightColor;
uniform vec3 lightPos;
uniform vec3 viewPos;

in vec3 ourColor;
in vec3 ourNormal;
in vec2 ourTexCoords;
in vec3 ourFragPos;

void main()
{
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

    vec3 norm = normalize(ourNormal);
    vec3 lightDir = normalize(lightPos - ourFragPos); 

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - ourFragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;

    vec3 result = (ambient + diffuse + specular) * ourColor;
    FragColor = vec4(result, 1.0);
    }
";

            public Vector3 LightPosition { get; set; }
            public Vector3 LightColor { get; set; } = new Vector3(1f, 1f, 1f);
            public PointLightingShader(Texture diffuseMap = null, Texture specularMap = null)
            {
                Load(vert, frag);
            }
            protected override void Disposing()
            {
                base.Disposing();
            }
            public override void Use(PaintEventArgs3D e)
            {
                base.Use(e);
                SetVector3("lightColor", LightColor);
                SetVector3("lightPos", LightPosition);
                SetVector3("viewPos", e.Camera.CamPosition);
            }
            public override void EndUse()
            {
                base.EndUse();
            }
        }
    }
}