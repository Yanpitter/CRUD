using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Setar a biblioteca do MySQL
using MySql.Data.MySqlClient;

namespace CRUD
{
    public partial class Form1 : Form
    {

        // Vari�vel de conex�o com o banco de dados
        MySqlConnection Conexao;
        //ff
        // String de conex�o com o banco de dados
        string connString = "datasource=localhost;username=root;password=;database=db_agenda";

        //Vari�vel para armazenar o ID do contato selecionado.
        //nt? � um nullable int, ou seja,um inteiro que pode conter um valor inteiro ou null.
        private int? id_contato_selecionado = null;
        public Form1()
        {
            // ### LINHA ADICIONADA PARA CENTRALIZAR O FORMUL�RIO ###
            this.StartPosition = FormStartPosition.CenterScreen;
            // Inicializa os componentes do list
            InitializeComponent();

            lstContatos.View = View.Details;
            lstContatos.LabelEdit = true;
            lstContatos.AllowColumnReorder = true;
            lstContatos.FullRowSelect = true;
            lstContatos.GridLines = true;

            // Adiciona colunas ao ListView
            lstContatos.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lstContatos.Columns.Add("E-mail", 150, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Telefone", 150, HorizontalAlignment.Left);
            btnEditar.Enabled = false;

            // Evento que carrega os contatos do banco de dados ao iniciar o formul�rio
            CarregarContatos();

        }
        private bool EmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        //Evento do bot�o Salvar
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {

                //cria a conex�o com o banco de dados 
                Conexao = new MySqlConnection(connString);

                //Abre a conex�o com o banco de dados
                Conexao.Open();

                //Cria um comando SQL para inserir ou atualizar contatos
                MySqlCommand cmd = new MySqlCommand();

                //Define a conex�o do comando com a conex�o do banco de dados
                cmd.Connection = Conexao;

                // Verifica se o ID do contato selecionado � nulo (ou seja, se � um novo contato)
                // Se for nulo, insere um novo contato; caso contr�rio, atualiza o contato existente
                if (id_contato_selecionado == null)
                {
                    cmd.CommandText = "INSERT INTO Contatos (nome, email, telefone) " +
                                      "VALUES " +
                                      "(@nome, @email, @telefone) ";

                    // Adiciona os par�metros ao comando para evitar SQL Injection                    
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                }
                else
                {
                    // Atualiza Contato
                    cmd.CommandText = "UPDATE Contatos SET " +
                                      "nome=@nome, email=@email, telefone=@telefone " +
                                      "WHERE id=@id ";

                    // Adiciona os par�metros ao comando para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    // Adiciona o ID do contato selecionado para atualizar o registro correto
                    cmd.Parameters.AddWithValue("@id", id_contato_selecionado);
                    // Prepara o comando para execu��o
                    cmd.Prepare();
                    // Executa o comando para atualizar o contato
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato Atualizado com Sucesso!",
                                    "Sucesso!", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                // Evento que carrega os contatos do banco de dados ao iniciar o formul�rio 
                CarregarContatos();
                MessageBox.Show("Contato Inserido com Sucesso!",
                                   "Sucesso!", MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar contato!" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conexao?.Close();
            }

            //Este m�todo � chamado para limpar os campos do formul�rio
            //ap�s salvar ou atualizar um contato.
            zerar_formulario();
        }

        //Executa a consulta de contatos com base no texto digitado no campo txtLocalizar
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            MySqlConnection Conexao = null;
            try
            {
                Conexao = new MySqlConnection(connString);

                Conexao.Open();

                //Cria um comando SQL para consultar contatos 
                MySqlCommand cmd = new MySqlCommand();

                //Define a conex�o do comando com a conex�o do banco de dados
                cmd.Connection = Conexao;

                //Define o comando SQL para buscar contatos com base no nome ou email
                cmd.CommandText = "SELECT * FROM Contatos WHERE nome LIKE @q OR email LIKE @q ";



                //Adiciona o par�metro @q com o valor do texto digitado no campo txtLocalizar
                cmd.Parameters.AddWithValue("@q", "%" + txtLocalizar.Text + "%");

                cmd.Prepare();

                //Executa o comando e obt�m os resultados
                MySqlDataReader reader = cmd.ExecuteReader();

                //Limpa a lista de contatos antes de adicionar os resultados da consulta
                lstContatos.Items.Clear();

                //Percorre os resultados da consulta e adiciona cada contato � lista lstContatos
                while (reader.Read())
                {
                    //Cria um vetor de strings para armazenar os dados do contato
                    string[] row =
                    {
                        reader[0].ToString(),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        reader[3].ToString()
                    };

                    //Cria um novo ListViewItem com os dados do contato
                    lstContatos.Items.Add(new ListViewItem(row));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao consultar contatos!" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Certifica-se de que a conex�o com o banco de dados seja fechada,
                //independentemente de ocorrer um erro ou n�o
                if (Conexao != null)
                {
                    Conexao.Close();
                }
            }
        }

        //M�todo para carregar os contatos do banco de dados e exibi-los na lista lstContatos
        private void CarregarContatos()
        {
            try
            {
                Conexao = new MySqlConnection(connString);
                // Cria a consulta SQL para selecionar todos os contatos e orden�-los
                // por ID em ordem decrescente
                string sql = "SELECT * FROM Contatos ORDER BY id DESC ";

                // Cria um comando MySqlCommand com a consulta SQL e a conex�o
                MySqlCommand comando = new MySqlCommand(sql, Conexao);
                Conexao.Open();
                MySqlDataReader reader = comando.ExecuteReader();

                // Limpa a lista de contatos antes de adicionar os resultados da consulta
                lstContatos.Items.Clear();

                // Percorre o banco com os resultados da consulta e adiciona cada contato
                // � lista lstContatos   
                while (reader.Read())
                {
                    string[] row =
                    {
                        reader[0].ToString(),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        reader[3].ToString()
                    };
                    var linha_listview = new ListViewItem(row);
                    lstContatos.Items.Add(linha_listview);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar contatos!" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Conexao != null)
                {
                    Conexao.Close();
                }
            }
        }

        //M�todo para limpar os campos do formul�rio e preparar para um novo contato
        private void zerar_formulario()
        {
            id_contato_selecionado = null;

            txtNome.Text = String.Empty;
            txtEmail.Text = "";
            txtTelefone.Text = "";

            txtNome.Focus();

            btnEditar.Enabled = false;
        }

        // Evento do bot�o Excluir, que � acionado quando o usu�rio clica
        // no bot�o de excluir um contato
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // Verifica se um contato foi selecionado
            if (id_contato_selecionado == null)
            {
                MessageBox.Show("Por favor, selecione um contato para excluir.",
                                "Aten��o", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult conf = MessageBox.Show("Tem certeza que deseja excluir o registro?",
                                                "Confirma��o",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);

            if (conf == DialogResult.Yes)
            {
                try
                {
                    Conexao = new MySqlConnection(connString);
                    Conexao.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = Conexao;

                    cmd.CommandText = "DELETE FROM Contatos WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", id_contato_selecionado);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    CarregarContatos();
                    lstContatos.Refresh();
                    zerar_formulario();

                    MessageBox.Show("Contato exclu�do com sucesso!",
                                   "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                   "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocorreu: " + ex.Message,
                                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Conexao?.Close();
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Verifica se um contato foi selecionado para edi��o
            zerar_formulario();
        }

        private void lstContatos_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Verifica se h� itens selecionados na lista lstContatos
            ListView.SelectedListViewItemCollection itens_selecionados = lstContatos.SelectedItems;

            // Se houver itens selecionados, obt�m o ID do contato selecionado
            foreach (ListViewItem item in itens_selecionados)
            {
                // Converte o texto do primeiro subitem (ID) para um inteiro
                id_contato_selecionado = Convert.ToInt32(item.SubItems[0].Text);

                // Preenche os campos do formul�rio com os dados do contato selecionado
                txtNome.Text = item.SubItems[1].Text;
                txtEmail.Text = item.SubItems[2].Text;
                txtTelefone.Text = item.SubItems[3].Text;

                // Habilita o bot�o de editar, pois um contato foi selecionado
                btnEditar.Enabled = true;
            }
        }

        private void txtTelefone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada n�o � um controle (como Backspace)
            // e n�o � um d�gito.
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Se n�o for um n�mero nem um controle, impede a entrada do caractere
                e.Handled = true;
            }
        }
    }
}
