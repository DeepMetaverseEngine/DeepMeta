using DeepMetaGame.Win32;

namespace _Temp_.Win32Editor
{
    public class _Temp_Win32Factory : ZoneWin32Factory
    {

//         public override BattleView3D CreateBattleView(GLControl control, System.Windows.Forms.Timer timer)
//         {
//             return new ZumaBattleView3D(control, timer);
//         }
//         public override InstanceBattle CreateBattle(EditorTemplates templates, PanelBattleView3D.BattleConfig cfg, SceneData sceneData)
//         {
//             if (cfg.exeType == LocalExecuteType.Preview)
//             {
//                 return cfg.hostFactory.CreatePreview(templates, cfg.slaveFactory, sceneData);
//             }
//             return new _Temp_BattleRuntime(templates, sceneData);
//             // return base.CreateBattle(templates, cfg, sceneData);
//         }
//         public override LayerZoneUnit3D CreateUnitView(BattleView3D parent, LayerUnit obj)
//         {
//             return new ZumaLayerZoneUnit3D(parent, obj);
//         }
//         public class ZumaBattleView3D : BattleView3D
//         {
//             private BezierCurveTrack track;
//             public ZumaBattleView3D(GLControl control, System.Windows.Forms.Timer timer) : base(control, timer)
//             {
//             }
//             protected override void OnLayerInitFinished(LayerZone layer)
//             {
//                 base.OnLayerInitFinished(layer);
//                 this.track = new BezierCurveTrack();
//                 ForEachObjects<LayerZonePoint3D>(point =>
//                 {
//                     this.track.AddPoint(this.SceneData, point.ZPoint.Data, 0.5f, 10);
//                     return false;
//                 });
//             }
//             protected override void DrawHUDGDI(PaintEventArgs e)
//             {
//                 base.DrawHUDGDI(e);
//                 //e.Graphics.DrawString($"总共路径长度:{track.TotalLength}", this.GLControl.Font, Brushes.White, 10, 100);
//             }
//             protected override void BattleView3D_OnEndRender(GLView sender, PaintEventArgs3D e)
//             {
//                 base.BattleView3D_OnEndRender(sender, e);
//                 foreach (var point in track)
//                 {
//                     DrawingVoxelObject.DrawBody3D(Color4.White, Color4.White, Color4.White, point.Position.ToGL(), 1, 0.1f);
//                 }
//             }
//         }
//         public class ZumaLayerZoneUnit3D : LayerZoneUnit3D
//         {
//             public ZumaLayerZoneUnit3D(BattleView3D parent, LayerUnit obj) : base(parent, obj)
//             {
//             }
//             protected override void DrawHUD(PaintEventArgs3D e, ref Vector2 offset)
//             {
//                 if (ZUnit.Info.ZumaProperties.UnitType == ZumaUnitProp.ZumaUnitType.MonsterBody)
//                 {
// 
//                 }
//                 else
//                 {
//                     base.DrawHUD(e, ref offset);
//                 }
//             }
//         }
    }
}
