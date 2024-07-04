namespace NEAcomputingForm
{
    partial class DebugMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnClr = new Button();
            btnClub = new Button();
            btnDebugOut = new Button();
            btnNumpad = new Button();
            btnOpenDebug = new Button();
            btnCombat = new Button();
            btnMegaHeal = new Button();
            btnStab = new Button();
            lblCombatDebug = new Label();
            lblOpenDebug = new Label();
            SuspendLayout();
            // 
            // btnClr
            // 
            btnClr.Location = new Point(680, 29);
            btnClr.Name = "btnClr";
            btnClr.Size = new Size(108, 23);
            btnClr.TabIndex = 0;
            btnClr.Text = "Clear Output Box";
            btnClr.UseVisualStyleBackColor = true;
            btnClr.Click += btnClr_Click;
            // 
            // btnClub
            // 
            btnClub.Location = new Point(176, 29);
            btnClub.Name = "btnClub";
            btnClub.Size = new Size(146, 23);
            btnClub.TabIndex = 1;
            btnClub.Text = "Set all weapons to club";
            btnClub.UseVisualStyleBackColor = true;
            btnClub.Click += btnClub_Click;
            // 
            // btnDebugOut
            // 
            btnDebugOut.Location = new Point(2, 29);
            btnDebugOut.Name = "btnDebugOut";
            btnDebugOut.Size = new Size(146, 23);
            btnDebugOut.TabIndex = 2;
            btnDebugOut.Text = "Output debug data";
            btnDebugOut.UseVisualStyleBackColor = true;
            btnDebugOut.Click += btnDebugOut_Click;
            // 
            // btnNumpad
            // 
            btnNumpad.Location = new Point(2, 56);
            btnNumpad.Name = "btnNumpad";
            btnNumpad.Size = new Size(146, 23);
            btnNumpad.TabIndex = 3;
            btnNumpad.Text = "Open Numpad";
            btnNumpad.UseVisualStyleBackColor = true;
            btnNumpad.Click += btnNumpad_Click;
            // 
            // btnOpenDebug
            // 
            btnOpenDebug.Location = new Point(2, 85);
            btnOpenDebug.Name = "btnOpenDebug";
            btnOpenDebug.Size = new Size(146, 23);
            btnOpenDebug.TabIndex = 4;
            btnOpenDebug.Text = "Open Debug Menu";
            btnOpenDebug.UseVisualStyleBackColor = true;
            btnOpenDebug.Click += btnOpenDebug_Click;
            // 
            // btnCombat
            // 
            btnCombat.Location = new Point(176, 85);
            btnCombat.Name = "btnCombat";
            btnCombat.Size = new Size(146, 23);
            btnCombat.TabIndex = 5;
            btnCombat.Text = "Put into debug combat";
            btnCombat.UseVisualStyleBackColor = true;
            btnCombat.Click += btnCombat_Click;
            // 
            // btnMegaHeal
            // 
            btnMegaHeal.Location = new Point(151, 114);
            btnMegaHeal.Name = "btnMegaHeal";
            btnMegaHeal.Size = new Size(193, 23);
            btnMegaHeal.TabIndex = 6;
            btnMegaHeal.Text = "Add lots of health to specialist";
            btnMegaHeal.UseVisualStyleBackColor = true;
            btnMegaHeal.Click += btnMegaHeal_Click;
            // 
            // btnStab
            // 
            btnStab.Location = new Point(176, 56);
            btnStab.Name = "btnStab";
            btnStab.Size = new Size(146, 23);
            btnStab.TabIndex = 7;
            btnStab.Text = "Set all weapons to Knife";
            btnStab.UseVisualStyleBackColor = true;
            btnStab.Click += btnStab_Click;
            // 
            // lblCombatDebug
            // 
            lblCombatDebug.AutoSize = true;
            lblCombatDebug.BorderStyle = BorderStyle.FixedSingle;
            lblCombatDebug.Location = new Point(205, 9);
            lblCombatDebug.Name = "lblCombatDebug";
            lblCombatDebug.Size = new Size(90, 17);
            lblCombatDebug.TabIndex = 8;
            lblCombatDebug.Text = "Combat Debug";
            // 
            // lblOpenDebug
            // 
            lblOpenDebug.AutoSize = true;
            lblOpenDebug.BorderStyle = BorderStyle.FixedSingle;
            lblOpenDebug.Location = new Point(35, 9);
            lblOpenDebug.Name = "lblOpenDebug";
            lblOpenDebug.Size = new Size(76, 17);
            lblOpenDebug.TabIndex = 9;
            lblOpenDebug.Text = "Open Debug";
            // 
            // DebugMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblOpenDebug);
            Controls.Add(lblCombatDebug);
            Controls.Add(btnStab);
            Controls.Add(btnMegaHeal);
            Controls.Add(btnCombat);
            Controls.Add(btnOpenDebug);
            Controls.Add(btnNumpad);
            Controls.Add(btnDebugOut);
            Controls.Add(btnClub);
            Controls.Add(btnClr);
            Name = "DebugMenu";
            Text = "DebugMenu";
            Load += DebugMenu_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnClr;
        private Button btnClub;
        private Button btnDebugOut;
        private Button btnNumpad;
        private Button btnOpenDebug;
        private Button btnCombat;
        private Button btnMegaHeal;
        private Button btnStab;
        private Label lblCombatDebug;
        private Label lblOpenDebug;
    }
}