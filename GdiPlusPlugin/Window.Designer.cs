﻿namespace AntMe.Plugin.GdiPlusPlugin {
    partial class Window {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent() {
			System.Windows.Forms.Label label4;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
			System.Windows.Forms.Label label1;
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.btnResetView = new System.Windows.Forms.Button();
			this.cbUseAntiAliasing = new System.Windows.Forms.CheckBox();
			this.cbShowScore = new System.Windows.Forms.CheckBox();
			this.insectsPanel = new System.Windows.Forms.FlowLayoutPanel();
			label4 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// label4
			// 
			label4.AccessibleDescription = null;
			label4.AccessibleName = null;
			resources.ApplyResources(label4, "label4");
			label4.BackColor = System.Drawing.SystemColors.ActiveCaption;
			label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			label4.Name = "label4";
			// 
			// splitContainer
			// 
			this.splitContainer.AccessibleDescription = null;
			this.splitContainer.AccessibleName = null;
			resources.ApplyResources(this.splitContainer, "splitContainer");
			this.splitContainer.BackgroundImage = null;
			this.splitContainer.Font = null;
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.AccessibleDescription = null;
			this.splitContainer.Panel1.AccessibleName = null;
			resources.ApplyResources(this.splitContainer.Panel1, "splitContainer.Panel1");
			this.splitContainer.Panel1.BackgroundImage = null;
			this.splitContainer.Panel1.Font = null;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.AccessibleDescription = null;
			this.splitContainer.Panel2.AccessibleName = null;
			resources.ApplyResources(this.splitContainer.Panel2, "splitContainer.Panel2");
			this.splitContainer.Panel2.BackColor = System.Drawing.Color.White;
			this.splitContainer.Panel2.BackgroundImage = null;
			this.splitContainer.Panel2.Controls.Add(this.btnResetView);
			this.splitContainer.Panel2.Controls.Add(this.cbUseAntiAliasing);
			this.splitContainer.Panel2.Controls.Add(label1);
			this.splitContainer.Panel2.Controls.Add(label4);
			this.splitContainer.Panel2.Controls.Add(this.cbShowScore);
			this.splitContainer.Panel2.Controls.Add(this.insectsPanel);
			this.splitContainer.Panel2.Font = null;
			// 
			// btnResetView
			// 
			this.btnResetView.AccessibleDescription = null;
			this.btnResetView.AccessibleName = null;
			resources.ApplyResources(this.btnResetView, "btnResetView");
			this.btnResetView.BackgroundImage = null;
			this.btnResetView.Font = null;
			this.btnResetView.Name = "btnResetView";
			this.btnResetView.UseVisualStyleBackColor = true;
			this.btnResetView.Click += new System.EventHandler(this.resetButton_Click);
			// 
			// cbUseAntiAliasing
			// 
			this.cbUseAntiAliasing.AccessibleDescription = null;
			this.cbUseAntiAliasing.AccessibleName = null;
			resources.ApplyResources(this.cbUseAntiAliasing, "cbUseAntiAliasing");
			this.cbUseAntiAliasing.BackgroundImage = null;
			this.cbUseAntiAliasing.Font = null;
			this.cbUseAntiAliasing.Name = "cbUseAntiAliasing";
			this.cbUseAntiAliasing.UseVisualStyleBackColor = true;
			this.cbUseAntiAliasing.CheckedChanged += new System.EventHandler(this.antialiasingCheckbox_CheckedChanged);
			// 
			// label1
			// 
			label1.AccessibleDescription = null;
			label1.AccessibleName = null;
			resources.ApplyResources(label1, "label1");
			label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
			label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			label1.Name = "label1";
			// 
			// cbShowScore
			// 
			this.cbShowScore.AccessibleDescription = null;
			this.cbShowScore.AccessibleName = null;
			resources.ApplyResources(this.cbShowScore, "cbShowScore");
			this.cbShowScore.BackgroundImage = null;
			this.cbShowScore.Font = null;
			this.cbShowScore.Name = "cbShowScore";
			this.cbShowScore.UseVisualStyleBackColor = true;
			this.cbShowScore.CheckedChanged += new System.EventHandler(this.showPointsCheckbox_CheckedChanged);
			// 
			// insectsPanel
			// 
			this.insectsPanel.AccessibleDescription = null;
			this.insectsPanel.AccessibleName = null;
			resources.ApplyResources(this.insectsPanel, "insectsPanel");
			this.insectsPanel.BackgroundImage = null;
			this.insectsPanel.Font = null;
			this.insectsPanel.Name = "insectsPanel";
			// 
			// Window
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.Controls.Add(this.splitContainer);
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "Window";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Window_FormClosing);
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.Panel2.PerformLayout();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.FlowLayoutPanel insectsPanel;
        private System.Windows.Forms.CheckBox cbUseAntiAliasing;
        private System.Windows.Forms.CheckBox cbShowScore;
        private System.Windows.Forms.Button btnResetView;

    }
}