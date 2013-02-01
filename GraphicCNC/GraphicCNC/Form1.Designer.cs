namespace GraphicCNC
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboxFresado = new System.Windows.Forms.GroupBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lblComando = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboxFresado
            // 
            this.cboxFresado.Location = new System.Drawing.Point(394, 12);
            this.cboxFresado.Name = "cboxFresado";
            this.cboxFresado.Size = new System.Drawing.Size(275, 389);
            this.cboxFresado.TabIndex = 0;
            this.cboxFresado.TabStop = false;
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(12, 12);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 23);
            this.btnFile.TabIndex = 1;
            this.btnFile.Text = "Open File";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(12, 41);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(75, 23);
            this.btnStep.TabIndex = 2;
            this.btnStep.Text = "Make Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 70);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // lblComando
            // 
            this.lblComando.AutoSize = true;
            this.lblComando.Location = new System.Drawing.Point(12, 100);
            this.lblComando.Name = "lblComando";
            this.lblComando.Size = new System.Drawing.Size(85, 13);
            this.lblComando.TabIndex = 4;
            this.lblComando.Text = "Comando Actual";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 413);
            this.Controls.Add(this.lblComando);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.cboxFresado);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox cboxFresado;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label lblComando;
    }
}

