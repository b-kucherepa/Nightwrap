﻿namespace Nightwrap
{
    partial class GUIForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUIForm));
            labelTimer = new Label();
            checkBoxEnable = new CheckBox();
            buttonClose = new Button();
            checkBoxStartup = new CheckBox();
            numericTimer = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numericTimer).BeginInit();
            SuspendLayout();
            // 
            // labelTimer
            // 
            labelTimer.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelTimer.ForeColor = SystemColors.Control;
            labelTimer.Location = new Point(15, 11);
            labelTimer.Margin = new Padding(4, 0, 4, 0);
            labelTimer.Name = "labelTimer";
            labelTimer.Size = new Size(250, 22);
            labelTimer.TabIndex = 0;
            labelTimer.Text = "Set the timer to (in s):";
            // 
            // checkBoxEnable
            // 
            checkBoxEnable.Appearance = Appearance.Button;
            checkBoxEnable.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            checkBoxEnable.Location = new Point(272, 85);
            checkBoxEnable.Margin = new Padding(4);
            checkBoxEnable.Name = "checkBoxEnable";
            checkBoxEnable.Size = new Size(94, 35);
            checkBoxEnable.TabIndex = 1;
            checkBoxEnable.Text = "Start";
            checkBoxEnable.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxEnable.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            buttonClose.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            buttonClose.ForeColor = Color.Red;
            buttonClose.Location = new Point(15, 85);
            buttonClose.Margin = new Padding(4);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(194, 36);
            buttonClose.TabIndex = 2;
            buttonClose.Text = "Close the service";
            buttonClose.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartup
            // 
            checkBoxStartup.CheckAlign = ContentAlignment.MiddleRight;
            checkBoxStartup.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            checkBoxStartup.ForeColor = SystemColors.Control;
            checkBoxStartup.Location = new Point(15, 50);
            checkBoxStartup.Margin = new Padding(4);
            checkBoxStartup.Name = "checkBoxStartup";
            checkBoxStartup.Size = new Size(276, 28);
            checkBoxStartup.TabIndex = 3;
            checkBoxStartup.Text = "Launch on OS startup:";
            checkBoxStartup.UseVisualStyleBackColor = true;
            // 
            // numericTimer
            // 
            numericTimer.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            numericTimer.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            numericTimer.Location = new Point(272, 9);
            numericTimer.Margin = new Padding(4);
            numericTimer.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numericTimer.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            numericTimer.Name = "numericTimer";
            numericTimer.Size = new Size(94, 25);
            numericTimer.TabIndex = 4;
            numericTimer.TextAlign = HorizontalAlignment.Right;
            numericTimer.ThousandsSeparator = true;
            numericTimer.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // GUIForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MidnightBlue;
            ClientSize = new Size(378, 141);
            Controls.Add(numericTimer);
            Controls.Add(checkBoxStartup);
            Controls.Add(buttonClose);
            Controls.Add(checkBoxEnable);
            Controls.Add(labelTimer);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GUIForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Nightwrap";
            ((System.ComponentModel.ISupportInitialize)numericTimer).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label labelTimer;
        private CheckBox checkBoxEnable;
        private Button buttonClose;
        private CheckBox checkBoxStartup;
        private NumericUpDown numericTimer;
    }
}