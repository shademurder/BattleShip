namespace BattleShip
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            BattleShipp.DefaultAI defaultAI1 = new BattleShipp.DefaultAI();
            this.battleShip1 = new BattleShipp.BattleShip();
            this.SuspendLayout();
            // 
            // battleShip1
            // 
            this.battleShip1.AHorizontalCells = 20;
            this.battleShip1.ArtificialIntelligence = defaultAI1;
            this.battleShip1.AVerticalCells = 20;
            this.battleShip1.BorderColor = System.Drawing.Color.Black;
            this.battleShip1.CellBackground = System.Drawing.Color.White;
            this.battleShip1.CellSize = new System.Drawing.Size(20, 20);
            this.battleShip1.DamagedShipColor = System.Drawing.Color.Red;
            this.battleShip1.DestroedShipColor = System.Drawing.Color.DarkRed;
            this.battleShip1.DoubleDeckShips = 7;
            this.battleShip1.EmptyWaterColor = System.Drawing.Color.Aquamarine;
            this.battleShip1.FiveDeckShips = 1;
            this.battleShip1.FogColor = System.Drawing.Color.Gray;
            this.battleShip1.FourDeckShips = 2;
            this.battleShip1.HorizontalSpace = 40;
            this.battleShip1.Location = new System.Drawing.Point(13, 13);
            this.battleShip1.Name = "battleShip1";
            this.battleShip1.OneDeckShips = 9;
            this.battleShip1.ShipColor = System.Drawing.Color.LimeGreen;
            this.battleShip1.Size = new System.Drawing.Size(937, 481);
            this.battleShip1.SpaceBetweenFields = 15;
            this.battleShip1.TabIndex = 0;
            this.battleShip1.ThreeDeckShips = 4;
            this.battleShip1.UseDefaultCheckingFieldSize = false;
            this.battleShip1.VerticalSpace = 30;
            this.battleShip1.WaterColor = System.Drawing.Color.LightSkyBlue;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 509);
            this.Controls.Add(this.battleShip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private BattleShipp.BattleShip battleShip1;
    }
}

