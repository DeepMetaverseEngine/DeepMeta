using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.Voxel.DisplayTerrainData
{
    public partial class FormVoxelCreater : DeepEditor.Common.G2D.G2DBaseForm
    {
        public TerrainDataViewer Viewer { get => terrainDataViewer1; }
        public TerrainDataCanvas Canvas { get => terrainDataViewer1.Canvas; }
        public FormVoxelCreater()
        {
            InitializeComponent();
        }
        public void LoadZipFile(FileInfo zipFile)
        {
            var data = Voxel3DPlugin.LoadTerrainData(zipFile.FullName);
            this.Canvas.InitTerrain(data);
        }
        public void SaveZipFile(FileInfo zipFile)
        { 
        
        }
    }
}
