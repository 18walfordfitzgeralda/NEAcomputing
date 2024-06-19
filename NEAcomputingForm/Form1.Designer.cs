namespace NEAcomputingForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtOut = new RichTextBox();
            buttonContinue = new Button();
            buttonClear = new Button();
            Runtime = new System.Windows.Forms.Timer(components);
            labelTime = new Label();
            Runtime2 = new System.Windows.Forms.Timer(components);
            labelGamestate = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtCurrentInput = new TextBox();
            label4 = new Label();
            SuspendLayout();
            // 
            // txtOut
            // 
            txtOut.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtOut.Location = new Point(82, 85);
            txtOut.Name = "txtOut";
            txtOut.ReadOnly = true;
            txtOut.Size = new Size(519, 196);
            txtOut.TabIndex = 1;
            txtOut.Text = "";
            // 
            // buttonContinue
            // 
            buttonContinue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            buttonContinue.Location = new Point(482, 367);
            buttonContinue.Name = "buttonContinue";
            buttonContinue.Size = new Size(218, 33);
            buttonContinue.TabIndex = 12;
            buttonContinue.Text = "Debug button 2";
            buttonContinue.UseVisualStyleBackColor = true;
            buttonContinue.Click += buttonContinue_Click;
            // 
            // buttonClear
            // 
            buttonClear.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            buttonClear.Location = new Point(482, 313);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(218, 33);
            buttonClear.TabIndex = 13;
            buttonClear.Text = "Debug button 1";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += buttonClear_Click;
            // 
            // Runtime
            // 
            Runtime.Enabled = true;
            Runtime.Interval = 1;
            Runtime.Tick += Runtime_Tick;
            // 
            // labelTime
            // 
            labelTime.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelTime.AutoSize = true;
            labelTime.Location = new Point(97, 43);
            labelTime.Name = "labelTime";
            labelTime.Size = new Size(13, 15);
            labelTime.TabIndex = 14;
            labelTime.Text = "0";
            // 
            // Runtime2
            // 
            Runtime2.Enabled = true;
            Runtime2.Interval = 1000;
            Runtime2.Tick += Runtime2_Tick;
            // 
            // labelGamestate
            // 
            labelGamestate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelGamestate.AutoSize = true;
            labelGamestate.Location = new Point(284, 43);
            labelGamestate.Name = "labelGamestate";
            labelGamestate.Size = new Size(50, 15);
            labelGamestate.TabIndex = 15;
            labelGamestate.Text = "Loading";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(82, 28);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 16;
            label1.Text = "Running for:";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(82, 58);
            label2.Name = "label2";
            label2.Size = new Size(50, 15);
            label2.TabIndex = 17;
            label2.Text = "seconds";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(284, 28);
            label3.Name = "label3";
            label3.Size = new Size(66, 15);
            label3.TabIndex = 18;
            label3.Text = "Gamestate:";
            // 
            // txtCurrentInput
            // 
            txtCurrentInput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtCurrentInput.Location = new Point(607, 55);
            txtCurrentInput.Name = "txtCurrentInput";
            txtCurrentInput.ReadOnly = true;
            txtCurrentInput.Size = new Size(93, 23);
            txtCurrentInput.TabIndex = 19;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(520, 58);
            label4.Name = "label4";
            label4.Size = new Size(81, 15);
            label4.TabIndex = 20;
            label4.Text = "Current Input:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(txtCurrentInput);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(labelGamestate);
            Controls.Add(labelTime);
            Controls.Add(buttonClear);
            Controls.Add(buttonContinue);
            Controls.Add(txtOut);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox txtOut;
        private Button buttonContinue;
        private Button buttonClear;
        private System.Windows.Forms.Timer Runtime;
        private Label labelTime;
        private System.Windows.Forms.Timer Runtime2;
        private Label labelGamestate;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtCurrentInput;
        private Label label4;
    }
}