﻿namespace NEAcomputingForm
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
            btnTestSave = new Button();
            btnTestLoad = new Button();
            btnChangeSquadName = new Button();
            txtDebugInput = new TextBox();
            label1 = new Label();
            label2 = new Label();
            btnaddtokens = new Button();
            btnLockpick = new Button();
            btnCrash = new Button();
            btnChangeTokens = new Button();
            btnChangeAmountTrained = new Button();
            btnChangeStatEff = new Button();
            btnResetFiles = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // btnClr
            // 
            btnClr.Location = new Point(2, 114);
            btnClr.Name = "btnClr";
            btnClr.Size = new Size(193, 23);
            btnClr.TabIndex = 0;
            btnClr.Text = "Clear Output Box";
            btnClr.UseVisualStyleBackColor = true;
            btnClr.Click += btnClr_Click;
            // 
            // btnClub
            // 
            btnClub.Location = new Point(205, 29);
            btnClub.Name = "btnClub";
            btnClub.Size = new Size(193, 23);
            btnClub.TabIndex = 1;
            btnClub.Text = "Set all weapons to club";
            btnClub.UseVisualStyleBackColor = true;
            btnClub.Click += btnClub_Click;
            // 
            // btnDebugOut
            // 
            btnDebugOut.Location = new Point(2, 29);
            btnDebugOut.Name = "btnDebugOut";
            btnDebugOut.Size = new Size(193, 23);
            btnDebugOut.TabIndex = 2;
            btnDebugOut.Text = "Output debug data";
            btnDebugOut.UseVisualStyleBackColor = true;
            btnDebugOut.Click += btnDebugOut_Click;
            // 
            // btnNumpad
            // 
            btnNumpad.Location = new Point(2, 56);
            btnNumpad.Name = "btnNumpad";
            btnNumpad.Size = new Size(193, 23);
            btnNumpad.TabIndex = 3;
            btnNumpad.Text = "Open Numpad";
            btnNumpad.UseVisualStyleBackColor = true;
            btnNumpad.Click += btnNumpad_Click;
            // 
            // btnOpenDebug
            // 
            btnOpenDebug.Location = new Point(2, 85);
            btnOpenDebug.Name = "btnOpenDebug";
            btnOpenDebug.Size = new Size(193, 23);
            btnOpenDebug.TabIndex = 4;
            btnOpenDebug.Text = "Open Debug Menu";
            btnOpenDebug.UseVisualStyleBackColor = true;
            btnOpenDebug.Click += btnOpenDebug_Click;
            // 
            // btnCombat
            // 
            btnCombat.Location = new Point(205, 114);
            btnCombat.Name = "btnCombat";
            btnCombat.Size = new Size(193, 23);
            btnCombat.TabIndex = 5;
            btnCombat.Text = "Put into debug combat";
            btnCombat.UseVisualStyleBackColor = true;
            btnCombat.Click += btnCombat_Click;
            // 
            // btnMegaHeal
            // 
            btnMegaHeal.Location = new Point(205, 85);
            btnMegaHeal.Name = "btnMegaHeal";
            btnMegaHeal.Size = new Size(193, 23);
            btnMegaHeal.TabIndex = 6;
            btnMegaHeal.Text = "Add lots of health to specialist";
            btnMegaHeal.UseVisualStyleBackColor = true;
            btnMegaHeal.Click += btnMegaHeal_Click;
            // 
            // btnStab
            // 
            btnStab.Location = new Point(205, 56);
            btnStab.Name = "btnStab";
            btnStab.Size = new Size(193, 23);
            btnStab.TabIndex = 7;
            btnStab.Text = "Set all weapons to Knife";
            btnStab.UseVisualStyleBackColor = true;
            btnStab.Click += btnStab_Click;
            // 
            // lblCombatDebug
            // 
            lblCombatDebug.AutoSize = true;
            lblCombatDebug.BorderStyle = BorderStyle.FixedSingle;
            lblCombatDebug.Location = new Point(258, 9);
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
            lblOpenDebug.Size = new Size(79, 17);
            lblOpenDebug.TabIndex = 9;
            lblOpenDebug.Text = "Debug Basics";
            // 
            // btnTestSave
            // 
            btnTestSave.Location = new Point(407, 29);
            btnTestSave.Name = "btnTestSave";
            btnTestSave.Size = new Size(193, 23);
            btnTestSave.TabIndex = 10;
            btnTestSave.Text = "Test Save";
            btnTestSave.UseVisualStyleBackColor = true;
            btnTestSave.Click += btnTestSave_Click;
            // 
            // btnTestLoad
            // 
            btnTestLoad.Location = new Point(407, 85);
            btnTestLoad.Name = "btnTestLoad";
            btnTestLoad.Size = new Size(193, 23);
            btnTestLoad.TabIndex = 11;
            btnTestLoad.Text = "Test Load";
            btnTestLoad.UseVisualStyleBackColor = true;
            btnTestLoad.Click += button1_Click;
            // 
            // btnChangeSquadName
            // 
            btnChangeSquadName.Location = new Point(407, 56);
            btnChangeSquadName.Name = "btnChangeSquadName";
            btnChangeSquadName.Size = new Size(193, 23);
            btnChangeSquadName.TabIndex = 12;
            btnChangeSquadName.Text = "Change Squad Name";
            btnChangeSquadName.UseVisualStyleBackColor = true;
            btnChangeSquadName.Click += btnChangeSquadName_Click;
            // 
            // txtDebugInput
            // 
            txtDebugInput.Location = new Point(496, 415);
            txtDebugInput.Name = "txtDebugInput";
            txtDebugInput.Size = new Size(292, 23);
            txtDebugInput.TabIndex = 13;
            txtDebugInput.TextChanged += txtDebugInput_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new Point(496, 395);
            label1.Name = "label1";
            label1.Size = new Size(292, 17);
            label1.TabIndex = 14;
            label1.Text = "Enter the string for the thing you want to change here";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new Point(453, 9);
            label2.Name = "label2";
            label2.Size = new Size(100, 17);
            label2.TabIndex = 15;
            label2.Text = "Save Load Debug";
            // 
            // btnaddtokens
            // 
            btnaddtokens.Location = new Point(205, 143);
            btnaddtokens.Name = "btnaddtokens";
            btnaddtokens.Size = new Size(193, 23);
            btnaddtokens.TabIndex = 16;
            btnaddtokens.Text = "Add 10 training tokens";
            btnaddtokens.UseVisualStyleBackColor = true;
            btnaddtokens.Click += btnaddtokens_Click;
            // 
            // btnLockpick
            // 
            btnLockpick.Location = new Point(205, 172);
            btnLockpick.Name = "btnLockpick";
            btnLockpick.Size = new Size(193, 23);
            btnLockpick.TabIndex = 17;
            btnLockpick.Text = "Unlock currently selected level";
            btnLockpick.UseVisualStyleBackColor = true;
            btnLockpick.Click += btnLockpick_Click;
            // 
            // btnCrash
            // 
            btnCrash.Location = new Point(2, 143);
            btnCrash.Name = "btnCrash";
            btnCrash.Size = new Size(193, 23);
            btnCrash.TabIndex = 18;
            btnCrash.Text = "Crash the game";
            btnCrash.UseVisualStyleBackColor = true;
            btnCrash.Click += btnCrash_Click;
            // 
            // btnChangeTokens
            // 
            btnChangeTokens.Location = new Point(205, 201);
            btnChangeTokens.Name = "btnChangeTokens";
            btnChangeTokens.Size = new Size(193, 23);
            btnChangeTokens.TabIndex = 19;
            btnChangeTokens.Text = "Change tokens per level";
            btnChangeTokens.UseVisualStyleBackColor = true;
            btnChangeTokens.Click += btnChangeTokens_Click;
            // 
            // btnChangeAmountTrained
            // 
            btnChangeAmountTrained.Location = new Point(205, 230);
            btnChangeAmountTrained.Name = "btnChangeAmountTrained";
            btnChangeAmountTrained.Size = new Size(193, 23);
            btnChangeAmountTrained.TabIndex = 20;
            btnChangeAmountTrained.Text = "Change amount trained";
            btnChangeAmountTrained.UseVisualStyleBackColor = true;
            btnChangeAmountTrained.Click += btnChangeAmountTrained_Click;
            // 
            // btnChangeStatEff
            // 
            btnChangeStatEff.Location = new Point(205, 259);
            btnChangeStatEff.Name = "btnChangeStatEff";
            btnChangeStatEff.Size = new Size(193, 23);
            btnChangeStatEff.TabIndex = 21;
            btnChangeStatEff.Text = "Change stat efficiency";
            btnChangeStatEff.UseVisualStyleBackColor = true;
            btnChangeStatEff.Click += btnChangeStatEff_Click;
            // 
            // btnResetFiles
            // 
            btnResetFiles.Location = new Point(2, 172);
            btnResetFiles.Name = "btnResetFiles";
            btnResetFiles.Size = new Size(193, 23);
            btnResetFiles.TabIndex = 22;
            btnResetFiles.Text = "Reset Files";
            btnResetFiles.UseVisualStyleBackColor = true;
            btnResetFiles.Click += btnResetFiles_Click;
            // 
            // button1
            // 
            button1.Location = new Point(2, 201);
            button1.Name = "button1";
            button1.Size = new Size(193, 23);
            button1.TabIndex = 23;
            button1.Text = "Reset Weapons File";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // button2
            // 
            button2.Location = new Point(2, 230);
            button2.Name = "button2";
            button2.Size = new Size(193, 23);
            button2.TabIndex = 24;
            button2.Text = "Reset Menu File";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(2, 259);
            button3.Name = "button3";
            button3.Size = new Size(193, 23);
            button3.TabIndex = 25;
            button3.Text = "Reset Tutorial Lines File";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // DebugMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(btnResetFiles);
            Controls.Add(btnChangeStatEff);
            Controls.Add(btnChangeAmountTrained);
            Controls.Add(btnChangeTokens);
            Controls.Add(btnCrash);
            Controls.Add(btnLockpick);
            Controls.Add(btnaddtokens);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtDebugInput);
            Controls.Add(btnChangeSquadName);
            Controls.Add(btnTestLoad);
            Controls.Add(btnTestSave);
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
        private Button btnTestSave;
        private Button btnTestLoad;
        private Button btnChangeSquadName;
        private TextBox txtDebugInput;
        private Label label1;
        private Label label2;
        private Button btnaddtokens;
        private Button btnLockpick;
        private Button btnCrash;
        private Button btnChangeTokens;
        private Button btnChangeAmountTrained;
        private Button btnChangeStatEff;
        private Button btnResetFiles;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}