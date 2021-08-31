using Eliza.Model.World;
using Eliza.UI.Helpers;
using Eto.Forms;

namespace Eliza.UI.Widgets
{
    public class RewardItemDataGroup : VBox
    {
        private RewardItemData _rewardItemData;

        private SpinBox itemId;
        private SpinBox amount;
        private SpinBox level;

        public RewardItemDataGroup(RewardItemData rewardItemData)
        {
            this._rewardItemData = rewardItemData;

            this.itemId = new SpinBox(new Ref<int>(() => this._rewardItemData.ItemId, v => { this._rewardItemData.ItemId = v; }), "Item ID");
            this.amount = new SpinBox(new Ref<int>(() => this._rewardItemData.Amount, v => { this._rewardItemData.Amount = v; }), "Amount");
            this.level = new SpinBox(new Ref<int>(() => this._rewardItemData.Level, v => { this._rewardItemData.Level = v; }), "Level");

            StackLayoutItem[] vBoxItems =
            {
                this.itemId,
                this.amount,
                this.level
            };

            foreach (var item in vBoxItems)
            {
                this.Items.Add(item);
            }
        }

        public RewardItemDataGroup()
        {
            var vBox = new VBox();

            this.itemId = new SpinBox("Item ID");
            this.amount = new SpinBox("Amount");
            this.level = new SpinBox("Level");

            StackLayoutItem[] vBoxItems =
            {
                this.itemId,
                this.amount,
                this.level
            };

            foreach (var item in vBoxItems)
            {
                this.Items.Add(item);
            }
        }

        public void ChangeReferenceValue(RewardItemData rewardItemData)
        {
            this._rewardItemData = rewardItemData;
            this.itemId.ChangeReferenceValue(new Ref<int>(() => this._rewardItemData.ItemId, v => { this._rewardItemData.ItemId = v; }));
            this.amount.ChangeReferenceValue(new Ref<int>(() => this._rewardItemData.Amount, v => { this._rewardItemData.Amount = v; }));
            this.level.ChangeReferenceValue(new Ref<int>(() => this._rewardItemData.Level, v => { this._rewardItemData.Level = v; }));
        }
    }
}
