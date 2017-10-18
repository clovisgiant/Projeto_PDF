namespace PostgreSQL_CRUD
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.btnExibir = new System.Windows.Forms.Button();
            this.btnInserir = new System.Windows.Forms.Button();
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.btnDeletar = new System.Windows.Forms.Button();
            this.dgvFunci = new System.Windows.Forms.DataGridView();
            this.txtNomeold = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtArquivo = new System.Windows.Forms.TextBox();
            this.btnSelecionarArquivos = new System.Windows.Forms.Button();
            this.ofd1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFunci)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(779, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Visualização do PDF";
            // 
            // btnExibir
            // 
            this.btnExibir.Location = new System.Drawing.Point(45, 101);
            this.btnExibir.Name = "btnExibir";
            this.btnExibir.Size = new System.Drawing.Size(75, 23);
            this.btnExibir.TabIndex = 2;
            this.btnExibir.Text = "Exibir";
            this.btnExibir.UseVisualStyleBackColor = true;
            this.btnExibir.Click += new System.EventHandler(this.btnExibir_Click);
            // 
            // btnInserir
            // 
            this.btnInserir.Location = new System.Drawing.Point(142, 101);
            this.btnInserir.Name = "btnInserir";
            this.btnInserir.Size = new System.Drawing.Size(75, 23);
            this.btnInserir.TabIndex = 2;
            this.btnInserir.Text = "Inserir";
            this.btnInserir.UseVisualStyleBackColor = true;
            this.btnInserir.Click += new System.EventHandler(this.btnInserir_Click);
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Location = new System.Drawing.Point(239, 101);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(75, 23);
            this.btnAtualizar.TabIndex = 2;
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // btnDeletar
            // 
            this.btnDeletar.Location = new System.Drawing.Point(336, 101);
            this.btnDeletar.Name = "btnDeletar";
            this.btnDeletar.Size = new System.Drawing.Size(75, 23);
            this.btnDeletar.TabIndex = 2;
            this.btnDeletar.Text = "Deletar";
            this.btnDeletar.UseVisualStyleBackColor = true;
            this.btnDeletar.Click += new System.EventHandler(this.btnDeletar_Click);
            // 
            // dgvFunci
            // 
            this.dgvFunci.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFunci.Location = new System.Drawing.Point(16, 130);
            this.dgvFunci.Name = "dgvFunci";
            this.dgvFunci.Size = new System.Drawing.Size(461, 430);
            this.dgvFunci.TabIndex = 3;
            this.dgvFunci.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFunci_CellEnter);
            // 
            // txtNomeold
            // 
            this.txtNomeold.Location = new System.Drawing.Point(483, 23);
            this.txtNomeold.Name = "txtNomeold";
            this.txtNomeold.Size = new System.Drawing.Size(682, 537);
            this.txtNomeold.TabIndex = 4;
            this.txtNomeold.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(338, 52);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 29);
            this.button1.TabIndex = 5;
            this.button1.Text = "Enviar para Bysoft";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtArquivo
            // 
            this.txtArquivo.Location = new System.Drawing.Point(16, 16);
            this.txtArquivo.Multiline = true;
            this.txtArquivo.Name = "txtArquivo";
            this.txtArquivo.Size = new System.Drawing.Size(316, 28);
            this.txtArquivo.TabIndex = 7;
            // 
            // btnSelecionarArquivos
            // 
            this.btnSelecionarArquivos.Location = new System.Drawing.Point(338, 16);
            this.btnSelecionarArquivos.Name = "btnSelecionarArquivos";
            this.btnSelecionarArquivos.Size = new System.Drawing.Size(130, 30);
            this.btnSelecionarArquivos.TabIndex = 6;
            this.btnSelecionarArquivos.Text = "Selecionar Arquivos";
            this.btnSelecionarArquivos.UseVisualStyleBackColor = true;
            this.btnSelecionarArquivos.Click += new System.EventHandler(this.btnSelecionarArquivos_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(1177, 572);
            this.Controls.Add(this.txtArquivo);
            this.Controls.Add(this.btnSelecionarArquivos);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtNomeold);
            this.Controls.Add(this.dgvFunci);
            this.Controls.Add(this.btnDeletar);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.btnInserir);
            this.Controls.Add(this.btnExibir);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GiantSysPDF";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFunci)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExibir;
        private System.Windows.Forms.Button btnInserir;
        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.Button btnDeletar;
        private System.Windows.Forms.DataGridView dgvFunci;
        private System.Windows.Forms.RichTextBox txtNomeold;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtArquivo;
        private System.Windows.Forms.Button btnSelecionarArquivos;
        private System.Windows.Forms.OpenFileDialog ofd1;
    }
}

