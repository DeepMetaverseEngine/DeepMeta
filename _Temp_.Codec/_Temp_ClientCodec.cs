
using DeepCore.IO;

namespace _Temp_.Codec
{
    public partial class Serializer : MessageFactoryGenerator
    {

    }
    public partial class _Temp_ClientCodec : MessageFactoryGenerator
    {
        public _Temp_ClientCodec() : base(Serializer.CODE_HASH)
        {
            this.RegistCodec(new Serializer());
            this.RegistExternalizableAssembly(typeof(Serializer));
        }
    }
}
