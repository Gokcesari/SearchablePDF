namespace SearchablePDF
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.txtbx = new System.Windows.Forms.TextBox();
            this.lstbx = new System.Windows.Forms.ListBox();
            this.rtbx = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(537, 70);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(92, 23);
            this.btn.TabIndex = 0;
            this.btn.Text = "Dosya Seç";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn1
            // 
            this.btn1.Location = new System.Drawing.Point(309, 59);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(93, 34);
            this.btn1.TabIndex = 1;
            this.btn1.Text = "PDF\'e çevir";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // txtbx
            // 
            this.txtbx.Location = new System.Drawing.Point(516, 99);
            this.txtbx.Name = "txtbx";
            this.txtbx.Size = new System.Drawing.Size(134, 20);
            this.txtbx.TabIndex = 2;
            // 
            // lstbx
            // 
            this.lstbx.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstbx.FormattingEnabled = true;
            this.lstbx.Location = new System.Drawing.Point(268, 99);
            this.lstbx.Name = "lstbx";
            this.lstbx.Size = new System.Drawing.Size(173, 56);
            this.lstbx.TabIndex = 3;
            // 
            // rtbx
            // 
            this.rtbx.Location = new System.Drawing.Point(66, 161);
            this.rtbx.Name = "rtbx";
            this.rtbx.Size = new System.Drawing.Size(611, 231);
            this.rtbx.TabIndex = 4;
            this.rtbx.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rtbx);
            this.Controls.Add(this.lstbx);
            this.Controls.Add(this.txtbx);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.btn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.TextBox txtbx;
        private System.Windows.Forms.ListBox lstbx;
        private System.Windows.Forms.RichTextBox rtbx;
    }
}

