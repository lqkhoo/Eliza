using Eliza.UI.Helpers;
using Eto.Forms;

namespace Eliza.UI.Widgets
{
    public class QuaternionGroup : GenericGroupBox
    {
        private Ref<float> _w;
        private Ref<float> _x;
        private Ref<float> _y;
        private Ref<float> _z;

        private SpinBox wSpinBox;
        private SpinBox xSpinBox;
        private SpinBox ySpinBox;
        private SpinBox zSpinBox;

        public QuaternionGroup(Ref<float> w, Ref<float> x, Ref<float> y, Ref<float> z, string text) : base(text)
        {
            this._w = w;
            this._x = x;
            this._y = y;
            this._z = z;

            var itemStackLayout = new VBox();

            this.wSpinBox = new SpinBox(new Ref<float>(() => this._w.Value, v => { this._w.Value = v; }), "W");
            this.xSpinBox = new SpinBox(new Ref<float>(() => this._x.Value, v => { this._x.Value = v; }), "X");
            this.ySpinBox = new SpinBox(new Ref<float>(() => this._y.Value, v => { this._y.Value = v; }), "Y");
            this.zSpinBox = new SpinBox(new Ref<float>(() => this._z.Value, v => { this._z.Value = v; }), "Z");

            StackLayoutItem[] itemStackLayoutItems =
            {
                    this.wSpinBox,
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

        public QuaternionGroup(string text) : base(text)
        {
            var itemStackLayout = new VBox();

            this.wSpinBox = new SpinBox("W");
            this.xSpinBox = new SpinBox("X");
            this.ySpinBox = new SpinBox("Y");
            this.zSpinBox = new SpinBox("Z");

            StackLayoutItem[] itemStackLayoutItems =
            {
                    this.wSpinBox,
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

        public void ChangeReferenceValue(Ref<float> w, Ref<float> x, Ref<float> y, Ref<float> z)
        {
            this._w = w;
            this._x = x;
            this._y = y;
            this._z = z;

            this.wSpinBox.ChangeReferenceValue(new Ref<float>(() => this._w.Value, v => { this._w.Value = v; }));
            this.xSpinBox.ChangeReferenceValue(new Ref<float>(() => this._x.Value, v => { this._x.Value = v; }));
            this.ySpinBox.ChangeReferenceValue(new Ref<float>(() => this._y.Value, v => { this._y.Value = v; }));
            this.zSpinBox.ChangeReferenceValue(new Ref<float>(() => this._z.Value, v => { this._z.Value = v; }));
        }
    }
}
