namespace PropGridTest
{
    partial class FrmPropGrid
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.propGrid1 = new HHTech.CSMMSS.Framework.Util.PropGrid();
            this.SuspendLayout();
            // 
            // propGrid1
            // 
            this.propGrid1.Caption = "ultraGrid1";
            this.propGrid1.CaptionVisible = true;
            this.propGrid1.IsDisplayInGroup = false;
            this.propGrid1.Location = new System.Drawing.Point(12, 12);
            this.propGrid1.Name = "propGrid1";
            this.propGrid1.Size = new System.Drawing.Size(416, 506);
            this.propGrid1.TabIndex = 0;
            // 
            // FrmPropGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 530);
            this.Controls.Add(this.propGrid1);
            this.Name = "FrmPropGrid";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FrmPropGrid_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private HHTech.CSMMSS.Framework.Util.PropGrid propGrid1;

    }
}

