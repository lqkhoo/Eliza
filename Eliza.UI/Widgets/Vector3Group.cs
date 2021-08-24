using Eliza.UI.Helpers;
using Eto.Forms;

namespace Eliza.UI.Widgets
{
    public class Vector3Group : GenericGroupBox
    {
        private Ref<float> _x;
        private Ref<float> _y;
        private Ref<float> _z;

        private SpinBox xSpinBox;
        private SpinBox ySpinBox;
        private SpinBox zSpinBox;


        public Vector3Group(Ref<float> x, Ref<float> y, Ref<float> z, string text) : base(text)
        {
            this._x = x;
            this._y = y;
            this._z = z;

            var itemStackLayout = new VBox();

            this.xSpinBox = new SpinBox(new Ref<float>(() => this._x.Value, v => { this._x.Value = v; }), "X");
            this.ySpinBox = new SpinBox(new Ref<float>(() => this._y.Value, v => { this._y.Value = v; }), "Y");
            this.zSpinBox = new SpinBox(new Ref<float>(() => this._z.Value, v => { this._z.Value = v; }), "Z");

            StackLayoutItem[] itemStackLayoutItems =
            {
                    this.xSpinBox,
                    this.ySpinBox,
                    this.zSpinBox
            };

            foreach (var item in itemStackLayoutItems)
            {
                itemStackLayout.Items.Add(item);

            }

            this.Content = itemStackLayout;
        }

        public Vector3Group(string text) : base(text)
        {
            var itemStackLayout = new VBox();

            this.xSpinBox = new SpinBox("X");
            this.ySpinBox = new SpinBox("Y");
            this.zSpinBox = new SpinBox("Z");

            StackLayoutItem[] itemStackLayoutItems =
            {
                    this.xSpinBox,
                    this.ySpinBox,
                    this.zSpinBox
            };

            foreach (var item in itemStackLayoutItems)
            {
                itemStackLayout.Items.Add(item);

            }

            this.Content = itemStackLayout;
        }

        public void ChangeReferenceValue(Ref<float> x, Ref<float> y, Ref<float> z)
        {
            this._x = x;
            this._y = y;
            this._z = z;

            this.xSpinBox.ChangeReferenceValue(new Ref<float>(() => this._x.Value, v => { this._x.Value = v; }));
            this.ySpinBox.ChangeReferenceValue(new Ref<float>(() => this._y.Value, v => { this._y.Value = v; }));
            this.zSpinBox.ChangeReferenceValue(new Ref<float>(() => this._z.Value, v => { this._z.Value = v; }));
        }
    }
}
