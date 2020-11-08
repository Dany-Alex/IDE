namespace IDE
{
    partial class IDE
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
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
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDE));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.salirBoton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.limpiarCodigoBoton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonGenerarArbol = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.exportarErrorBoton = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.entradaRichTextBox = new System.Windows.Forms.RichTextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.richTextBoxConsola = new System.Windows.Forms.RichTextBox();
            this.getLineaLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.autocompleteMenu1 = new AutocompleteMenuNS.AutocompleteMenu();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1106, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoToolStripMenuItem,
            this.cargarToolStripMenuItem,
            this.guardarToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // nuevoToolStripMenuItem
            // 
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.nuevoToolStripMenuItem.Text = "Nuevo";
            this.nuevoToolStripMenuItem.Click += new System.EventHandler(this.nuevoToolStripMenuItem_Click);
            // 
            // cargarToolStripMenuItem
            // 
            this.cargarToolStripMenuItem.Name = "cargarToolStripMenuItem";
            this.cargarToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.cargarToolStripMenuItem.Text = "Cargar";
            this.cargarToolStripMenuItem.Click += new System.EventHandler(this.cargarToolStripMenuItem_Click);
            // 
            // guardarToolStripMenuItem
            // 
            this.guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            this.guardarToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.guardarToolStripMenuItem.Text = "Guardar Como";
            this.guardarToolStripMenuItem.Click += new System.EventHandler(this.guardarToolStripMenuItem_Click);
            // 
            // salirBoton
            // 
            this.salirBoton.Location = new System.Drawing.Point(8, 280);
            this.salirBoton.Margin = new System.Windows.Forms.Padding(4);
            this.salirBoton.Name = "salirBoton";
            this.salirBoton.Size = new System.Drawing.Size(127, 28);
            this.salirBoton.TabIndex = 2;
            this.salirBoton.Text = "Salir";
            this.salirBoton.UseVisualStyleBackColor = true;
            this.salirBoton.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 109);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 39);
            this.button1.TabIndex = 1;
            this.button1.Text = "Guardar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // limpiarCodigoBoton
            // 
            this.limpiarCodigoBoton.Location = new System.Drawing.Point(7, 26);
            this.limpiarCodigoBoton.Margin = new System.Windows.Forms.Padding(4);
            this.limpiarCodigoBoton.Name = "limpiarCodigoBoton";
            this.limpiarCodigoBoton.Size = new System.Drawing.Size(128, 49);
            this.limpiarCodigoBoton.TabIndex = 3;
            this.limpiarCodigoBoton.Text = "Compilar";
            this.limpiarCodigoBoton.UseVisualStyleBackColor = true;
            this.limpiarCodigoBoton.Click += new System.EventHandler(this.limpiarCodigoBoton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonGenerarArbol);
            this.panel1.Controls.Add(this.limpiarCodigoBoton);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.salirBoton);
            this.panel1.Location = new System.Drawing.Point(963, 594);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(146, 317);
            this.panel1.TabIndex = 10;
            // 
            // buttonGenerarArbol
            // 
            this.buttonGenerarArbol.Location = new System.Drawing.Point(7, 219);
            this.buttonGenerarArbol.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGenerarArbol.Name = "buttonGenerarArbol";
            this.buttonGenerarArbol.Size = new System.Drawing.Size(128, 39);
            this.buttonGenerarArbol.TabIndex = 4;
            this.buttonGenerarArbol.Text = "Generar Arbol";
            this.buttonGenerarArbol.UseVisualStyleBackColor = true;
            this.buttonGenerarArbol.Click += new System.EventHandler(this.buttonGenerarArbol_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.exportarErrorBoton);
            this.tabPage2.Controls.Add(this.dataGridView2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(943, 286);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Errores";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // exportarErrorBoton
            // 
            this.exportarErrorBoton.Location = new System.Drawing.Point(855, 254);
            this.exportarErrorBoton.Margin = new System.Windows.Forms.Padding(4);
            this.exportarErrorBoton.Name = "exportarErrorBoton";
            this.exportarErrorBoton.Size = new System.Drawing.Size(80, 28);
            this.exportarErrorBoton.TabIndex = 5;
            this.exportarErrorBoton.Text = "Exportar";
            this.exportarErrorBoton.UseVisualStyleBackColor = true;
            this.exportarErrorBoton.Click += new System.EventHandler(this.exportarErrorBoton_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(-4, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(852, 286);
            this.dataGridView2.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(943, 286);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Salida";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(935, 278);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(4, 595);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(951, 315);
            this.tabControl1.TabIndex = 3;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(4, 4);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1098, 576);
            this.tabControl2.TabIndex = 9;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.entradaRichTextBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage3.Size = new System.Drawing.Size(1090, 547);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Editor";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // entradaRichTextBox
            // 
            this.entradaRichTextBox.AcceptsTab = true;
            this.autocompleteMenu1.SetAutocompleteMenu(this.entradaRichTextBox, this.autocompleteMenu1);
            this.entradaRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entradaRichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.entradaRichTextBox.Location = new System.Drawing.Point(4, 4);
            this.entradaRichTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.entradaRichTextBox.Name = "entradaRichTextBox";
            this.entradaRichTextBox.Size = new System.Drawing.Size(1082, 539);
            this.entradaRichTextBox.TabIndex = 2;
            this.entradaRichTextBox.Text = "";
            this.entradaRichTextBox.Click += new System.EventHandler(this.entradaRichTextBox_Click);
            this.entradaRichTextBox.TextChanged += new System.EventHandler(this.entradaRichTextBox_TextChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.richTextBoxConsola);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1090, 547);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Consola";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // richTextBoxConsola
            // 
            this.autocompleteMenu1.SetAutocompleteMenu(this.richTextBoxConsola, null);
            this.richTextBoxConsola.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxConsola.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxConsola.Name = "richTextBoxConsola";
            this.richTextBoxConsola.Size = new System.Drawing.Size(1084, 541);
            this.richTextBoxConsola.TabIndex = 0;
            this.richTextBoxConsola.Text = "";
            // 
            // getLineaLabel
            // 
            this.getLineaLabel.AutoSize = true;
            this.getLineaLabel.Location = new System.Drawing.Point(941, 4);
            this.getLineaLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.getLineaLabel.Name = "getLineaLabel";
            this.getLineaLabel.Size = new System.Drawing.Size(122, 17);
            this.getLineaLabel.TabIndex = 5;
            this.getLineaLabel.Text = " Linea:   Columna:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.33093F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.66906F));
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 31);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.66084F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.33917F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1112, 914);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // panel2
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel2, 2);
            this.panel2.Controls.Add(this.getLineaLabel);
            this.panel2.Controls.Add(this.tabControl2);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1106, 585);
            this.panel2.TabIndex = 13;
            // 
            // autocompleteMenu1
            // 
            this.autocompleteMenu1.Colors = ((AutocompleteMenuNS.Colors)(resources.GetObject("autocompleteMenu1.Colors")));
            this.autocompleteMenu1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.autocompleteMenu1.ImageList = null;
            this.autocompleteMenu1.Items = new string[] {
        "SI(){ }",
        "SINO{ }",
        "SINO_SI(){ }",
        "MIENTRAS(){ }",
        "HACER (){ } MIENTRAS()",
        "DESDE ; HASTA ; INCREMENTO { }",
        "entero",
        "cadena",
        "caracter",
        "boolean",
        "decimal",
        "principal(){ }",
        "leer(){ }",
        "escribir(){ }"};
            this.autocompleteMenu1.MaximumSize = new System.Drawing.Size(250, 300);
            this.autocompleteMenu1.MinFragmentLength = 1;
            this.autocompleteMenu1.TargetControlWrapper = null;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // IDE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1106, 947);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IDE";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IDE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IDE_FormClosing);
            this.Load += new System.EventHandler(this.IDE_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cargarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label getLineaLabel;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox entradaRichTextBox;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.RichTextBox richTextBoxConsola;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button exportarErrorBoton;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button limpiarCodigoBoton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button salirBoton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private AutocompleteMenuNS.AutocompleteMenu autocompleteMenu1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button buttonGenerarArbol;
    }
}

