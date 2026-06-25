using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    partial class AICarAdvisor
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
            this.ConsulationPanel = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetAdvice = new System.Windows.Forms.Button();
            this.txtSuggestions = new System.Windows.Forms.TextBox();
            this.txtRequirements = new System.Windows.Forms.TextBox();
            this.lbllName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ConsulationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConsulationPanel
            // 
            this.ConsulationPanel.BackColor = System.Drawing.Color.Transparent;
            this.ConsulationPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ConsulationPanel.Controls.Add(this.button2);
            this.ConsulationPanel.Controls.Add(this.label1);
            this.ConsulationPanel.Controls.Add(this.btnGetAdvice);
            this.ConsulationPanel.Controls.Add(this.txtSuggestions);
            this.ConsulationPanel.Controls.Add(this.txtRequirements);
            this.ConsulationPanel.Controls.Add(this.lbllName);
            this.ConsulationPanel.Controls.Add(this.label2);
            this.ConsulationPanel.Location = new System.Drawing.Point(-1, -1);
            this.ConsulationPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ConsulationPanel.Name = "ConsulationPanel";
            this.ConsulationPanel.Size = new System.Drawing.Size(926, 427);
            this.ConsulationPanel.TabIndex = 0;
            this.ConsulationPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ConsulationPanel_Paint);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.RoyalBlue;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(650, 18);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(197, 38);
            this.button2.TabIndex = 131;
            this.button2.Text = "Price Prediction";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(45, 218);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 38);
            this.label1.TabIndex = 129;
            this.label1.Text = "Recommended Cars";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnGetAdvice
            // 
            this.btnGetAdvice.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnGetAdvice.FlatAppearance.BorderSize = 0;
            this.btnGetAdvice.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnGetAdvice.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.btnGetAdvice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetAdvice.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetAdvice.ForeColor = System.Drawing.Color.White;
            this.btnGetAdvice.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGetAdvice.Location = new System.Drawing.Point(334, 176);
            this.btnGetAdvice.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGetAdvice.Name = "btnGetAdvice";
            this.btnGetAdvice.Size = new System.Drawing.Size(197, 38);
            this.btnGetAdvice.TabIndex = 128;
            this.btnGetAdvice.Text = "Get Suggestions";
            this.btnGetAdvice.UseVisualStyleBackColor = false;
            this.btnGetAdvice.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSuggestions
            // 
            this.txtSuggestions.BackColor = System.Drawing.Color.Black;
            this.txtSuggestions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSuggestions.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSuggestions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(160)))), ((int)(((byte)(166)))));
            this.txtSuggestions.Location = new System.Drawing.Point(51, 258);
            this.txtSuggestions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSuggestions.Multiline = true;
            this.txtSuggestions.Name = "txtSuggestions";
            this.txtSuggestions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSuggestions.Size = new System.Drawing.Size(810, 126);
            this.txtSuggestions.TabIndex = 127;
            this.txtSuggestions.TextChanged += new System.EventHandler(this.textName_TextChanged);
            // 
            // txtRequirements
            // 
            this.txtRequirements.BackColor = System.Drawing.Color.Black;
            this.txtRequirements.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRequirements.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRequirements.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(160)))), ((int)(((byte)(166)))));
            this.txtRequirements.Location = new System.Drawing.Point(50, 89);
            this.txtRequirements.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtRequirements.Multiline = true;
            this.txtRequirements.Name = "txtRequirements";
            this.txtRequirements.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRequirements.Size = new System.Drawing.Size(797, 80);
            this.txtRequirements.TabIndex = 127;
            this.txtRequirements.TextChanged += new System.EventHandler(this.textName_TextChanged);
            // 
            // lbllName
            // 
            this.lbllName.BackColor = System.Drawing.Color.Transparent;
            this.lbllName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllName.ForeColor = System.Drawing.Color.White;
            this.lbllName.Location = new System.Drawing.Point(45, 54);
            this.lbllName.Name = "lbllName";
            this.lbllName.Size = new System.Drawing.Size(302, 38);
            this.lbllName.TabIndex = 126;
            this.lbllName.Text = "Enter Your Car Requirements ";
            this.lbllName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(160)))), ((int)(((byte)(166)))));
            this.label2.Location = new System.Drawing.Point(42, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 45);
            this.label2.TabIndex = 125;
            this.label2.Text = "AI Car Advisor";
            // 
            // AICarAdvisor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = global::AutoNova_Car_Showroom.Properties.Resources.backpanel;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(926, 424);
            this.Controls.Add(this.ConsulationPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AICarAdvisor";
            this.Text = "AICarAdvisor";
            this.Load += new System.EventHandler(this.AICarAdvisor_Load);
            this.ConsulationPanel.ResumeLayout(false);
            this.ConsulationPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ConsulationPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGetAdvice;
        private System.Windows.Forms.TextBox txtRequirements;
        private System.Windows.Forms.Label lbllName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtSuggestions;
    }
}