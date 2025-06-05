namespace CRUD
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtNome = new TextBox();
            txtEmail = new TextBox();
            btnSalvar = new Button();
            lstContatos = new ListView();
            txtLocalizar = new TextBox();
            label4 = new Label();
            btnEditar = new Button();
            btnExcluir = new Button();
            mtxTelefone = new MaskedTextBox();
            btnAnterior = new Button();
            btnProxima = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 0;
            label1.Text = "Nome:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 53);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 1;
            label2.Text = "Telefone:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 97);
            label3.Name = "label3";
            label3.Size = new Size(44, 15);
            label3.TabIndex = 2;
            label3.Text = "E-mail:";
            // 
            // txtNome
            // 
            txtNome.BorderStyle = BorderStyle.FixedSingle;
            txtNome.Location = new Point(12, 27);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(339, 23);
            txtNome.TabIndex = 3;
            // 

            // txtTelefone
            // 
            mtxTelefone.Location = new Point(12, 71);
            mtxTelefone.Name = "txtTelefone";
            mtxTelefone.Size = new Size(191, 23);
            mtxTelefone.TabIndex = 4;
            mtxTelefone.KeyPress += txtTelefone_KeyPress;
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Location = new Point(12, 115);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(339, 23);
            txtEmail.TabIndex = 5;
            // 
            // btnSalvar
            // 
            btnSalvar.Image = Properties.Resources.Salvar;
            btnSalvar.Location = new Point(12, 321);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(96, 43);
            btnSalvar.TabIndex = 6;
            btnSalvar.UseVisualStyleBackColor = true;
            btnSalvar.Click += btnSalvar_Click;
            // 
            // lstContatos
            // 
            lstContatos.Location = new Point(373, 57);
            lstContatos.Name = "lstContatos";
            lstContatos.Size = new Size(462, 307);
            lstContatos.TabIndex = 7;
            lstContatos.UseCompatibleStateImageBehavior = false;
            lstContatos.SelectedIndexChanged += lstContatos_SelectedIndexChanged_1;
            // 
            // txtLocalizar
            // 
            txtLocalizar.BorderStyle = BorderStyle.FixedSingle;
            txtLocalizar.Location = new Point(373, 28);
            txtLocalizar.Name = "txtLocalizar";
            txtLocalizar.Size = new Size(462, 23);
            txtLocalizar.TabIndex = 8;
            txtLocalizar.TextChanged += txtLocalizar_TextChanged;
            txtLocalizar.Size = new Size(371, 23);
            txtLocalizar.TabIndex = 8;
            // 
            // btnConsultar
            // 
            
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(373, 9);
            label4.Name = "label4";
            label4.Size = new Size(56, 15);
            label4.TabIndex = 10;
            label4.Text = "Localizar:";
            // 
            // btnEditar
            // 
            btnEditar.Image = Properties.Resources.imagem__1_;
            btnEditar.Location = new Point(133, 321);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(96, 43);
            btnEditar.TabIndex = 11;
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Image = Properties.Resources.Apagar;
            btnExcluir.Location = new Point(255, 321);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(96, 43);
            btnExcluir.TabIndex = 12;
            btnExcluir.UseVisualStyleBackColor = true;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // mtxTelefone
            // 
            mtxTelefone.Location = new Point(12, 71);
            mtxTelefone.Mask = "(99) 00000-0000";
            mtxTelefone.Name = "mtxTelefone";
            mtxTelefone.Size = new Size(100, 23);
            mtxTelefone.TabIndex = 13;
            // 
            // btnAnterior
            // 
            btnAnterior.Location = new Point(385, 330);
            btnAnterior.Name = "btnAnterior";
            btnAnterior.Size = new Size(75, 23);
            btnAnterior.TabIndex = 14;
            btnAnterior.Text = "Anterior";
            btnAnterior.UseVisualStyleBackColor = true;
            btnAnterior.Click += btnAnterior_Click;
            // 
            // btnProxima
            // 
            btnProxima.Location = new Point(466, 330);
            btnProxima.Name = "btnProxima";
            btnProxima.Size = new Size(75, 23);
            btnProxima.TabIndex = 15;
            btnProxima.Text = "Próxima";
            btnProxima.UseVisualStyleBackColor = true;
            btnProxima.Click += btnProxima_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(847, 376);
            Controls.Add(btnProxima);
            Controls.Add(btnAnterior);
            Controls.Add(mtxTelefone);
            Controls.Add(btnExcluir);
            Controls.Add(btnEditar);
            Controls.Add(label4);
            Controls.Add(txtLocalizar);
            Controls.Add(lstContatos);
            Controls.Add(btnSalvar);
            Controls.Add(txtEmail);
            Controls.Add(txtNome);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "Agenda de Contatos";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtNome;
        private TextBox txtEmail;
        private Button btnSalvar;
        private ListView lstContatos;
        private TextBox txtLocalizar;
        private Label label4;
        private Button btnEditar;
        private Button btnExcluir;
        private MaskedTextBox mtxTelefone;
        private Button btnAnterior;
        private Button btnProxima;
    }
}
