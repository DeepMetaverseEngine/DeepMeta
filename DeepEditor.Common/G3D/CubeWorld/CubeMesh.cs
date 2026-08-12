using DeepCore;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;
using System.Drawing;

namespace DeepEditor.Common.G3D.CubeWorld
{
    public class CubeMesh : Disposable
    {

        private static float[] temp_vertices =
        {
            // Positions          Normals               Colors                      Texture coords
            -0.5f, -0.5f, -0.5f,   0.0f,  0.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f,   0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,   0.0f,  0.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f,   1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,   0.0f,  0.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f,   1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,   0.0f,  0.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f,   1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,   0.0f,  0.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f,   0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,   0.0f,  0.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f,   0.0f, 0.0f,
                                                                            
            -0.5f, -0.5f,  0.5f,   0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,   0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,   0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,   1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,   0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,   1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,   0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,   1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,   0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,   0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,   0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,   0.0f, 0.0f,
                                                                            
            -0.5f,  0.5f,  0.5f,  -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   1.0f, 0.0f,
                                                                            
             0.5f,  0.5f,  0.5f,   1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,   1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,   1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,   1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,   1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,   1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,   1.0f, 0.0f,
                                                                            
            -0.5f, -0.5f, -0.5f,   0.0f, -1.0f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,   0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,   0.0f, -1.0f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,   1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,   0.0f, -1.0f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,   1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,   0.0f, -1.0f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,   1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,   0.0f, -1.0f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,   0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,   0.0f, -1.0f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,   0.0f, 1.0f,
                                                                            
            -0.5f,  0.5f, -0.5f,   0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,   0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,   0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,   1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,   0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,   1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,   0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,   1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,   0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,   0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,   0.0f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,   0.0f, 1.0f
        };
        private float _cubeSize;
        private List<float> _addVertices = new List<float>();
        public CubeMesh(float size) { this._cubeSize = size; }
        public virtual void AddCube(Vector3 pos, Color4 color) 
        {
        
        }

        private Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);
        private int _vertexBufferObject;
        private int _vaoModel;
        private int _vaoLamp;

        private Shader _lampShader;
        private Shader _lightingShader;
        private GLTexture _diffuseMap;
        private GLTexture _specularMap;

        protected override void Disposing()
        {
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vaoModel);
        }

        public void Init()
        {
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

            {
                _vaoModel = GL.GenVertexArray();
                GL.BindVertexArray(_vaoModel);

                var positionLocation = _lightingShader.GetAttribLocation("aPos");
                GL.EnableVertexAttribArray(positionLocation);
                GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

                var normalLocation = _lightingShader.GetAttribLocation("aNormal");
                GL.EnableVertexAttribArray(normalLocation);
                GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

                var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoords");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            }

            {
                _vaoLamp = GL.GenVertexArray();
                GL.BindVertexArray(_vaoLamp);

                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

                var positionLocation = _lampShader.GetAttribLocation("aPos");
                GL.EnableVertexAttribArray(positionLocation);
                GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            }

            _diffuseMap = Texture.LoadFromFile("Resources/container2.png");
            _specularMap = Texture.LoadFromFile("Resources/container2_specular.png");

        }

        public void Draw(PaintEventArgs3D e)
        {
            GL.BindVertexArray(_vaoModel);

            _diffuseMap.Use(TextureUnit.Texture0);
            _specularMap.Use(TextureUnit.Texture1);
            _lightingShader.Use();

            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            _lightingShader.SetVector3("viewPos", _camera.Position);

            _lightingShader.SetInt("material.diffuse", 0);
            _lightingShader.SetInt("material.specular", 1);
            _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader.SetFloat("material.shininess", 32.0f);

            // Directional light needs a direction, in this example we just use (-0.2, -1.0, -0.3f) as the lights direction
            _lightingShader.SetVector3("light.direction", new Vector3(-0.2f, -1.0f, -0.3f));
            _lightingShader.SetVector3("light.ambient", new Vector3(0.2f));
            _lightingShader.SetVector3("light.diffuse", new Vector3(0.5f));
            _lightingShader.SetVector3("light.specular", new Vector3(1.0f));

            // We want to draw all the cubes at their respective positions
            for (int i = 0; i < _cubePositions.Length; i++)
            {
                // Then we translate said matrix by the cube position
                Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
                // We then calculate the angle and rotate the model around an axis
                float angle = 20.0f * i;
                model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
                // Remember to set the model at last so it can be used by opentk
                _lightingShader.SetMatrix4("model", model);

                // At last we draw all our cubes
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

            GL.BindVertexArray(_vaoModel);

            _lampShader.Use();

            Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
            lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

            _lampShader.SetMatrix4("model", lampMatrix);
            _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

        }





    }


}
