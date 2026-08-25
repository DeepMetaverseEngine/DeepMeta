using DeepCore.GUI.Input;
using DeepCore.Unity;
using DeepGame3D.Unity.BattleView;
using DeepMetaGame.Unity;
using UnityEngine;

namespace _Temp_.Client.Unity.Battle;

/// <summary>
/// 这部分是战斗图形逻辑部分，写到DLL里面。
/// 这部分写到DLL里面的好处是，VS编译会把DLL复制到UnityRun编辑器运行时里面，
/// 这样不需要重新打UnityPC包就可以更新逻辑。
/// </summary>
public partial class _Temp_UnityBattleFactory : UnityBattleFactory
{
    public _Temp_UnityBattleFactory(string root) : base(root)
    {
        
    }

}

