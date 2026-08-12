using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public class G2DEnumComboBox : G2DBaseComboBox
    {
        private Type enumType;
        public Type EnumType
        {
            get => enumType;
            set
            {
                if (enumType != value)
                {
                    this.enumType = value;
                    this.Items.Clear();
                    var names = Enum.GetNames(enumType);
                    if (names.Length > 0)
                    {
                        this.Items.AddRange(names);
                        this.Text = names[0];
                    }
                }
            }
        }
        public G2DEnumComboBox()
        {

        }
        public object GetEnumValue()
        {
            return Enum.Parse(EnumType, this.Text);
        }
        public T GetEnumValue<T>() where T : struct
        {
            if (Enum.TryParse<T>(this.Text, out var value))
            {
                return value;
            }
            return default(T);
        }
    }
}
