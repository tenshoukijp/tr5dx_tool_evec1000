namespace Evec1000DX
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this._SourceDirectoryTextBox = new System.Windows.Forms.TextBox();
            this._DirectorySelectButton = new System.Windows.Forms.Button();
            this._SenScriptsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this._LogTextBox = new System.Windows.Forms.TextBox();
            this._ReplaceSenScriptButton = new System.Windows.Forms.Button();
            this._ReplaceEvmButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 15);
            this.label1.TabIndex = 100;
            this.label1.Text = "ソースフォルダ：";
            // 
            // _SourceDirectoryTextBox
            // 
            this._SourceDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._SourceDirectoryTextBox.Location = new System.Drawing.Point(98, 6);
            this._SourceDirectoryTextBox.Name = "_SourceDirectoryTextBox";
            this._SourceDirectoryTextBox.Size = new System.Drawing.Size(593, 23);
            this._SourceDirectoryTextBox.TabIndex = 0;
            this._SourceDirectoryTextBox.TextChanged += new System.EventHandler(this._SourceDirectoryTextBox_TextChanged);
            // 
            // _DirectorySelectButton
            // 
            this._DirectorySelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._DirectorySelectButton.Location = new System.Drawing.Point(697, 6);
            this._DirectorySelectButton.Name = "_DirectorySelectButton";
            this._DirectorySelectButton.Size = new System.Drawing.Size(75, 23);
            this._DirectorySelectButton.TabIndex = 1;
            this._DirectorySelectButton.Text = "フォルダ選択";
            this._DirectorySelectButton.UseVisualStyleBackColor = true;
            this._DirectorySelectButton.Click += new System.EventHandler(this._DirectorySelectButton_Click);
            // 
            // _SenScriptsCheckedListBox
            // 
            this._SenScriptsCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._SenScriptsCheckedListBox.CheckOnClick = true;
            this._SenScriptsCheckedListBox.FormattingEnabled = true;
            this._SenScriptsCheckedListBox.Location = new System.Drawing.Point(12, 35);
            this._SenScriptsCheckedListBox.Name = "_SenScriptsCheckedListBox";
            this._SenScriptsCheckedListBox.ScrollAlwaysVisible = true;
            this._SenScriptsCheckedListBox.Size = new System.Drawing.Size(262, 454);
            this._SenScriptsCheckedListBox.TabIndex = 2;
            // 
            // _LogTextBox
            // 
            this._LogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._LogTextBox.BackColor = System.Drawing.SystemColors.Window;
            this._LogTextBox.Location = new System.Drawing.Point(280, 35);
            this._LogTextBox.Multiline = true;
            this._LogTextBox.Name = "_LogTextBox";
            this._LogTextBox.ReadOnly = true;
            this._LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._LogTextBox.Size = new System.Drawing.Size(492, 454);
            this._LogTextBox.TabIndex = 3;
            this._LogTextBox.TextChanged += new System.EventHandler(this._LogTextBox_TextChanged);
            // 
            // _ReplaceSenScriptButton
            // 
            this._ReplaceSenScriptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ReplaceSenScriptButton.BackgroundImage = global::Evec1000DX.Properties.Resources.ReplacingTextButton;
            this._ReplaceSenScriptButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._ReplaceSenScriptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._ReplaceSenScriptButton.ForeColor = System.Drawing.SystemColors.Control;
            this._ReplaceSenScriptButton.Location = new System.Drawing.Point(348, 499);
            this._ReplaceSenScriptButton.Name = "_ReplaceSenScriptButton";
            this._ReplaceSenScriptButton.Size = new System.Drawing.Size(200, 50);
            this._ReplaceSenScriptButton.TabIndex = 4;
            this._ReplaceSenScriptButton.UseVisualStyleBackColor = true;
            this._ReplaceSenScriptButton.Click += new System.EventHandler(this._ReplaceSenScriptButton_Click);
            // 
            // _ReplaceEvmButton
            // 
            this._ReplaceEvmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ReplaceEvmButton.BackColor = System.Drawing.SystemColors.Control;
            this._ReplaceEvmButton.BackgroundImage = global::Evec1000DX.Properties.Resources.ReplacingEvmButton;
            this._ReplaceEvmButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._ReplaceEvmButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._ReplaceEvmButton.ForeColor = System.Drawing.SystemColors.Control;
            this._ReplaceEvmButton.Location = new System.Drawing.Point(572, 499);
            this._ReplaceEvmButton.Name = "_ReplaceEvmButton";
            this._ReplaceEvmButton.Size = new System.Drawing.Size(200, 50);
            this._ReplaceEvmButton.TabIndex = 5;
            this._ReplaceEvmButton.UseVisualStyleBackColor = false;
            this._ReplaceEvmButton.Click += new System.EventHandler(this._ReplaceEvmButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this._ReplaceEvmButton);
            this.Controls.Add(this._ReplaceSenScriptButton);
            this.Controls.Add(this._LogTextBox);
            this.Controls.Add(this._SenScriptsCheckedListBox);
            this.Controls.Add(this._DirectorySelectButton);
            this.Controls.Add(this._SourceDirectoryTextBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = global::Evec1000DX.Properties.Resources.Evec1000DXIcon;
            this.MinimumSize = new System.Drawing.Size(550, 312);
            this.Name = "MainForm";
            this.Text = "千階堂DX";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _SourceDirectoryTextBox;
        private System.Windows.Forms.Button _DirectorySelectButton;
        private System.Windows.Forms.CheckedListBox _SenScriptsCheckedListBox;
        private System.Windows.Forms.TextBox _LogTextBox;
        private System.Windows.Forms.Button _ReplaceSenScriptButton;
        private System.Windows.Forms.Button _ReplaceEvmButton;
    }
}

