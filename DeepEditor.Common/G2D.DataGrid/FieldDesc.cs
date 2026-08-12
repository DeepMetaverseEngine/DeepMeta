using DeepCore.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D.DataGrid
{
    public class G2DFieldDescValue
    {
        public object ComponentData { get; set; }
        public object FieldMember { get; set; }
        public object FieldValue { get; set; }
    }
    public class G2DFieldElementDesc
    {
        public object RootData { get; set; }
        public object ComponentData { get; set; }
        public object FieldName { get; set; }
        public Type FieldDecleardType { get; set; }
        public MemberInfo FieldMember { get; set; }
        public object FieldValue { get; set; }
        public GridItem Cell { get; set; }


        public FieldOwnerValue ToFieldOwner() 
        {
            if (FieldMember is FieldInfo field)
            {
                return new FieldOwnerValue(RootData, field, FieldValue, ComponentData);
            }
            return null;
        }
    

    }
}
