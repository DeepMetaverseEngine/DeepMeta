using DeepCore.Game3D.Host.ZoneEditor.EventTrigger;
using DeepCore.Geometry;
using DeepCore.GUI.Data;
using DeepMetaGame.Data.Message;
using DeepMetaGame.Data.Message.UI;
using DeepMetaGame.Data.Template;
using System;

namespace DeepCore.Game3D.Host.Instance
{
    partial class InstanceZone
    {
        public InstanceUnit DraggingUnit { get; private set; }
        private Vector3 draggingUnitOffset;
        private void ProcessHUDMessage(UIInteractiveAction act)
        {

            if (act is MouseDownAction mouseDown)
            {
                if (mouseDown.raycast != null)
                {
                    var ray = mouseDown.raycast;
                    var unit = GetUnit(mouseDown.raycast.HitObjectID);
                    if (unit != null && unit.Info.Abilities.TryGetComponentAs<UnitDragAndDropAbility>(out var dnd))
                    {
                        unit.Retain();
                        this.DraggingUnit = unit;
                        this.draggingUnitOffset = DeepCore.Geometry.RayCast.RayPlaneIntersection(
                            ray.origin, ray.normal,
                            unit.Position, DeepCore.Geometry.Vector3.UnitZ);
                    }
                }
            }
            else if (act is MouseMoveAction mouseMove)
            {
                if (mouseMove.raycast != null)
                {
                    if (DraggingUnit != null)
                    {

                    }
                }
            }
            else if (act is MouseUpAction mouseUp)
            {
                this.DraggingUnit?.Release();
                this.DraggingUnit = null;
            }
        }

    }
}

