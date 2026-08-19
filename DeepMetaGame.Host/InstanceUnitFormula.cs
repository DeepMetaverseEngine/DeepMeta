namespace DeepCore.Game3D.Host.Instance
{
    public class InstanceUnitFormula : Recyclable
    {
        public InstanceUnit Owner { get; private set; }
        public virtual InstanceUnitFormula Init(InstanceUnit owner)
        {
            this.Owner = owner;
            return this;
        }
        protected override void Disposing()
        {
            this.Owner = null;
        }

        protected virtual internal void Init() { }
        protected virtual internal void LatedInit()
        {
        }
    }
}
