namespace DeepCore.Game3D.Host.Instance
{
    public class InstanceUnitFormula : Disposable
    {
        public InstanceUnit Owner { get; }
        public InstanceUnitFormula(InstanceUnit owner)
        {
            this.Owner = owner;
        }
        protected virtual internal void Init() { }
        protected virtual internal void LatedInit()
        {
        }
        protected override void Disposing() { }
    }
}
