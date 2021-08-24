using Eliza.Model;
using Eliza.UI.Widgets;
using Eto.Forms;
using System;

namespace Eliza.UI.Forms
{
    public class DataForm : Form
    {
        private SaveData _saveData;

        public DataForm(SaveData save)
        {
            this._saveData = save;

            this.Title = "Data";

            var data = save.saveData;
            var stackLayout = new VBox()
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };

            var saveFlagButton = new Button()
            {
                Text = "Save Flag"
            };

            saveFlagButton.Click += this.SaveFlagButton_Click;

            var worldDataButton = new Button()
            {
                Text = "World Data"
            };

            worldDataButton.Click += this.WorldDataButton_Click;

            var playerDataButton = new Button()
            {
                Text = "Player Data"
            };

            playerDataButton.Click += this.PlayerDataButton_Click;

            var eventDataButton = new Button()
            {
                Text = "Event Data"
            };

            eventDataButton.Click += this.EventDataButton_Click;

            var npcDataButton = new Button()
            {
                Text = "NPC Data"
            };

            npcDataButton.Click += this.NpcDataButton_Click;

            var fishingDataButton = new Button()
            {
                Text = "Fishing Data"
            };

            fishingDataButton.Click += this.FishingDataButton_Click;

            var stampDataButton = new Button()
            {
                Text = "Stamp Data"
            };

            stampDataButton.Click += this.StampDataButton_Click;

            var furnitureDataButton = new Button()
            {
                Text = "Furniture Data"
            };

            furnitureDataButton.Click += this.FurnitureDataButton_Click;

            var battleDataButton = new Button()
            {
                Text = "Battle Data"
            };

            battleDataButton.Click += this.BattleDataButton_Click;

            var itemDataButton = new Button()
            {
                Text = "Item Data"
            };

            itemDataButton.Click += this.ItemDataButton_Click;

            var supportMonsterDataButton = new Button()
            {
                Text = "Support Monster Data"
            };

            supportMonsterDataButton.Click += this.SupportMonsterDataButton_Click;

            var farmDataButton = new Button()
            {
                Text = "Farm Data"
            };

            farmDataButton.Click += this.FarmDataButton_Click;

            var statusDataButton = new Button()
            {
                Text = "Status Data"
            };

            statusDataButton.Click += this.StatusDataButton_Click;

            var orderDataButton = new Button()
            {
                Text = "Order Data"
            };

            orderDataButton.Click += this.OrderDataButton_Click;

            var makingDataButton = new Button()
            {
                Text = "Making Data"
            };

            makingDataButton.Click += this.MakingDataButton_Click;

            var policeBatchDataButton = new Button()
            {
                Text = "Police Batch Data"
            };

            policeBatchDataButton.Click += this.PoliceBatchDataButton_Click;

            var itemFlagDataButton = new Button()
            {
                Text = "Item Flag Data"
            };

            itemFlagDataButton.Click += this.ItemFlagDataButton_Click;

            var buildDataButton = new Button()
            {
                Text = "Build Data"
            };

            buildDataButton.Click += this.BuildDataButton_Click;

            var shippingDataButton = new Button()
            {
                Text = "Shipping Data"
            };

            shippingDataButton.Click += this.ShippingDataButton_Click;

            var lPocketDataButton = new Button()
            {
                Text = "LPocket Data"
            };

            lPocketDataButton.Click += this.LPocketDataButton_Click;

            var nameDataButton = new Button()
            {
                Text = "Name Data"
            };

            nameDataButton.Click += this.NameDataButton_Click;

            StackLayoutItem[] stackLayoutItems =
            {
                    saveFlagButton,
                    worldDataButton,
                    playerDataButton,
                    eventDataButton,
                    npcDataButton,
                    fishingDataButton,
                    stampDataButton,
                    furnitureDataButton,
                    battleDataButton,
                    itemDataButton,
                    supportMonsterDataButton,
                    farmDataButton,
                    statusDataButton,
                    orderDataButton,
                    makingDataButton,
                    policeBatchDataButton,
                    itemFlagDataButton,
                    buildDataButton,
                    shippingDataButton,
                    lPocketDataButton,
                    nameDataButton
                };
            foreach (var item in stackLayoutItems)
            {
                stackLayout.Items.Add(item);
            }

            this.Content = stackLayout;
        }

        private void SaveFlagButton_Click(object sender, EventArgs e)
        {
            var saveFlagForm = new SaveFlagForm(this._saveData.saveData.saveFlag);
            saveFlagForm.Show();
        }

        private void WorldDataButton_Click(object sender, EventArgs e)
        {
            var worldDataForm = new WorldDataForm(this._saveData.saveData.worldData);
            worldDataForm.Show();
        }

        private void PlayerDataButton_Click(object sender, EventArgs e)
        {
            var playerDataForm = new PlayerDataForm(this._saveData.saveData.playerData);
            playerDataForm.Show();
        }

        private void EventDataButton_Click(object sender, EventArgs e)
        {
            var eventDataForm = new EventDataForm(this._saveData.saveData.eventData);
            eventDataForm.Show();
        }

        private void NpcDataButton_Click(object sender, EventArgs e)
        {
            var npcDataForm = new NPCDataForm(this._saveData.saveData.npcData);
            npcDataForm.Show();
        }

        private void FishingDataButton_Click(object sender, EventArgs e)
        {
            var fishingDataForm = new FishingDataForm(this._saveData.saveData.fishingData);
            fishingDataForm.Show();
        }

        private void StampDataButton_Click(object sender, EventArgs e)
        {
            var stampDataForm = new StampDataForm(this._saveData.saveData.stampData);
            stampDataForm.Show();
        }

        private void FurnitureDataButton_Click(object sender, EventArgs e)
        {
            var furnitureDataForm = new FurnitureDataForm(this._saveData.saveData.furnitureData);
            furnitureDataForm.Show();
        }

        private void BattleDataButton_Click(object sender, EventArgs e)
        {
            var battleDataForm = new BattleDataForm(this._saveData.saveData.battleData);
            battleDataForm.Show();
        }

        private void ItemDataButton_Click(object sender, EventArgs e)
        {
            var itemDataForm = new ItemDataForm(this._saveData.saveData.itemData);
            itemDataForm.Show();
        }

        private void ShippingDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NameDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LPocketDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BuildDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ItemFlagDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PoliceBatchDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MakingDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OrderDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void StatusDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FarmDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SupportMonsterDataButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }




    }
}
