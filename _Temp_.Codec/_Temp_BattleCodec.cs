
using DeepCore.IO;
using DeepMetaGame.Data.ZoneEditor;

namespace _Temp_.Codec
{
    public class _Temp_BattleCodec : MessageFactoryGenerator
    {
        public _Temp_BattleCodec() : base(new Serializer())
        {
            this.RegistExternalizableAssembly(
                typeof(Serializer),
                typeof(EditorTemplates).Assembly);
        }
    }
}
