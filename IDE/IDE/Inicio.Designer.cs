namespace IDE
{
    partial class Inicio
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
            this.newProyect = new System.Windows.Forms.Button();
            this.loadProyect = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newProyect
            // 
            this.newProyect.Location = new System.Drawing.Point(211, 109);
            this.newProyect.Name = "newProyect";
            this.newProyect.Size = new System.Drawing.Size(174, 39);
            this.newProyect.TabIndex = 1;
            this.newProyect.Text = "Crear Nuevo Proyecto";
            this.newProyect.UseVisualStyleBackColor = true;
            this.newProyect.Click += new System.EventHandler(this.newProyect_Click);
            // 
            // loadProyect
            // 
            this.loadProyect.Location = new System.Drawing.Point(12, 109);
            this.loadProyect.Name = "loadProyect";
            this.loadProyect.Size = new System.Drawing.Size(174, 39);
            this.loadProyect.TabIndex = 2;
            this.loadProyect.Text = "Cargar Proyecto";
            this.loadProyect.UseVisualStyleBackColor = true;
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(112, 167);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(174, 39);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Salir";
            this.exitButton.UseVisualStyleBackColor = true;
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 225);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.loadProyect);
            this.Controls.Add(this.newProyect);
            this.Name = "Inicio";
            this.Text = "Inicio";
            this.Load += new System.EventHandler(this.Inicio_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button newProyect;
        private System.Windows.Forms.Button loadProyect;
        private System.Windows.Forms.Button exitButton;
    }
}