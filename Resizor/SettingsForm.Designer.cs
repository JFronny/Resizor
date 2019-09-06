namespace Resizor
{
    partial class SettingsForm
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
            this.keySelectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rowsSelect = new System.Windows.Forms.NumericUpDown();
            this.columnsSelect = new System.Windows.Forms.NumericUpDown();
            this.startupBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.rowsSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnsSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // keySelectButton
            // 
            this.keySelectButton.Location = new System.Drawing.Point(67, 35);
            this.keySelectButton.Name = "keySelectButton";
            this.keySelectButton.Size = new System.Drawing.Size(75, 23);
            this.keySelectButton.TabIndex = 0;
            this.keySelectButton.UseVisualStyleBackColor = true;
            this.keySelectButton.Click += new System.EventHandler(this.KeySelectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Key:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Rows";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Columns";
            // 
            // rowsSelect
            // 
            this.rowsSelect.Location = new System.Drawing.Point(67, 64);
            this.rowsSelect.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.rowsSelect.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.rowsSelect.Name = "rowsSelect";
            this.rowsSelect.ReadOnly = true;
            this.rowsSelect.Size = new System.Drawing.Size(75, 20);
            this.rowsSelect.TabIndex = 4;
            this.rowsSelect.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.rowsSelect.ValueChanged += new System.EventHandler(this.RowsSelect_ValueChanged);
            // 
            // columnsSelect
            // 
            this.columnsSelect.Location = new System.Drawing.Point(67, 90);
            this.columnsSelect.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.columnsSelect.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.columnsSelect.Name = "columnsSelect";
            this.columnsSelect.ReadOnly = true;
            this.columnsSelect.Size = new System.Drawing.Size(75, 20);
            this.columnsSelect.TabIndex = 5;
            this.columnsSelect.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.columnsSelect.ValueChanged += new System.EventHandler(this.ColumnsSelect_ValueChanged);
            // 
            // startupBox
            // 
            this.startupBox.AutoSize = true;
            this.startupBox.Location = new System.Drawing.Point(12, 12);
            this.startupBox.Name = "startupBox";
            this.startupBox.Size = new System.Drawing.Size(117, 17);
            this.startupBox.TabIndex = 6;
            this.startupBox.Text = "Start with Windows";
            this.startupBox.UseVisualStyleBackColor = true;
            this.startupBox.CheckedChanged += new System.EventHandler(this.StartupBox_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(154, 121);
            this.Controls.Add(this.startupBox);
            this.Controls.Add(this.columnsSelect);
            this.Controls.Add(this.rowsSelect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.keySelectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.rowsSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnsSelect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button keySelectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown rowsSelect;
        private System.Windows.Forms.NumericUpDown columnsSelect;
        private System.Windows.Forms.CheckBox startupBox;
    }
}