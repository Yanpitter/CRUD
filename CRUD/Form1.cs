using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

//Setar a biblioteca do MySQL
using MySql.Data.MySqlClient;

namespace CRUD
{
    public partial class Form1 : Form
    {
        private int paginaAtual = 1;
        private int totalPaginas = 1;
        private int registrosPorPagina = 10;

        // Variável de conexão com o banco de dados
        MySqlConnection Conexao;
        // String de conexão com o banco de dados
        string connString = "datasource=localhost;username=root;password=;database=db_agenda";

        //Variável para armazenar o ID do contato selecionado.
        //nt? é um nullable int, ou seja,um inteiro que pode conter um valor inteiro ou null.
        private int? id_contato_selecionado = null;
        public Form1()
        {
            #region Configurações da ListView
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
            #endregion

            txtNome.MaxLength = 150;
            txtEmail.MaxLength = 150;

            // Evento que carrega os contatos do banco de dados ao iniciar o formulário
            CarregarContatos();
        }

        private bool EmailValido(string email) // Método que verifica se o e-mail é válido e se contém um domínio (ex: @gmail.com)
        {
            // Expressão regular padrão para e-mails válidos:
            // ^                 => início da string
            // [a-zA-Z0-9._%+-]+ => letras, números e alguns símbolos válidos antes do @
            // @                 => obrigatório
            // [a-zA-Z0-9.-]+    => domínio (ex: gmail)
            // \.                => ponto literal (ex: .com)
            // [a-zA-Z]{2,}      => extensão (ex: com, org, br)
            // $                 => fim da string
            string padraoEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Verifica se o e-mail segue o padrão definido acima
            return System.Text.RegularExpressions.Regex.IsMatch(email, padraoEmail);
        }



        //Evento do botão Salvar
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // RF7 - Verificar se os campos estão preenchidos
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtTelefone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Todos os campos devem ser preenchidos antes de salvar.", "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar se o e-mail é válido // RF5 - Validar se o e-mail digitado é válido
            if (!EmailValido(txtEmail.Text))
            {
                MessageBox.Show("Digite um e-mail válido. Ex: fulano@gmail.com", "E-mail inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }


            try
            {

                //cria a conexão com o banco de dados 
                Conexao = new MySqlConnection(connString);

                //Abre a conexão com o banco de dados
                Conexao.Open();

                //Cria um comando SQL para inserir ou atualizar contatos
                MySqlCommand cmd = new MySqlCommand();

                //Define a conexão do comando com a conexão do banco de dados
                cmd.Connection = Conexao;

                // Verifica se o ID do contato selecionado é nulo (ou seja, se é um novo contato)
                // Se for nulo, insere um novo contato; caso contrário, atualiza o contato existente
                if (id_contato_selecionado == null)
                {
                    cmd.CommandText = "INSERT INTO contatos (nome, email, telefone) " +
                                      "VALUES " +
                                      "(@nome, @email, @telefone) ";

                    // Adiciona os parâmetros ao comando para evitar SQL Injection                    
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", mtxTelefone.Text);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato Inserido com Sucesso!",
                                    "Sucesso!", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                }
                else
                {
                    // Atualiza Contato
                    cmd.CommandText = "UPDATE contatos SET " +
                                      "nome=@nome, email=@email, telefone=@telefone " +
                                      "WHERE id=@id ";

                    // Adiciona os parâmetros ao comando para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@telefone", mtxTelefone.Text);
                    // Adiciona o ID do contato selecionado para atualizar o registro correto
                    cmd.Parameters.AddWithValue("@id", id_contato_selecionado);
                    // Prepara o comando para execução
                    cmd.Prepare();
                    // Executa o comando para atualizar o contato
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato Atualizado com Sucesso!",
                                    "Sucesso!", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                // Evento que carrega os contatos do banco de dados ao iniciar o formulário 
                CarregarContatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar contato!" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conexao?.Close();
            }

            //Este método é chamado para limpar os campos do formulário
            //após salvar ou atualizar um contato.
            zerar_formulario();
        }

        //Método para carregar os contatos do banco de dados e exibi-los na lista lstContatos
        private void CarregarContatos(int pagina = 1)
        {
            try
            {
                Conexao = new MySqlConnection(connString);

                // Conta o total de registros para calcular o total de páginas
                string sqlCount = "SELECT COUNT(*) FROM contatos";
                MySqlCommand cmdCount = new MySqlCommand(sqlCount, Conexao);
                Conexao.Open();
                int totalRegistros = Convert.ToInt32(cmdCount.ExecuteScalar());
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

                int offset = (pagina - 1) * registrosPorPagina;
                // Cria a consulta SQL para selecionar todos os contatos e ordená-los
                // por ID em ordem decrescente
                string sql = $"SELECT * FROM contatos ORDER BY id DESC LIMIT {registrosPorPagina} OFFSET {offset}";

                // Cria um comando MySqlCommand com a consulta SQL e a conexão
                MySqlCommand comando = new MySqlCommand(sql, Conexao);
                MySqlDataReader reader = comando.ExecuteReader();

                // Limpa a lista de contatos antes de adicionar os resultados da consulta
                lstContatos.Items.Clear();

                // Percorre o banco com os resultados da consulta e adiciona cada contato
                // à lista lstContatos   
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

        //Método para limpar os campos do formulário e preparar para um novo contato
        private void zerar_formulario()
        {
            id_contato_selecionado = null;

            txtNome.Text = String.Empty;
            txtEmail.Text = "";
            mtxTelefone.Text = "";

            txtNome.Focus();

            btnEditar.Enabled = false;
        }

        // Evento do botão Excluir, que é acionado quando o usuário clica
        // no botão de excluir um contato
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // Verifica se um contato foi selecionado
            if (id_contato_selecionado == null)
            {
                MessageBox.Show("Por favor, selecione um contato para excluir.",
                                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult conf = MessageBox.Show("Tem certeza que deseja excluir o registro?",
                                                "Confirmação",
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

                    cmd.CommandText = "DELETE FROM contatos WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", id_contato_selecionado);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato excluído com sucesso!",
                                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CarregarContatos();
                    lstContatos.Refresh();
                    zerar_formulario();
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
            // Verifica se um contato foi selecionado para edição
            zerar_formulario();
        }

        private void lstContatos_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Verifica se há itens selecionados na lista lstContatos
            ListView.SelectedListViewItemCollection itens_selecionados = lstContatos.SelectedItems;

            // Se houver itens selecionados, obtém o ID do contato selecionado
            foreach (ListViewItem item in itens_selecionados)
            {
                // Converte o texto do primeiro subitem (ID) para um inteiro
                id_contato_selecionado = Convert.ToInt32(item.SubItems[0].Text);

                // Preenche os campos do formulário com os dados do contato selecionado
                txtNome.Text = item.SubItems[1].Text;
                txtEmail.Text = item.SubItems[2].Text;
                mtxTelefone.Text = item.SubItems[3].Text;

                // Habilita o botão de editar, pois um contato foi selecionado
                btnEditar.Enabled = true;
            }
        }

        //Executa a consulta de contatos com base no texto digitado no campo txtLocalizar
        private void txtLocalizar_TextChanged(object sender, EventArgs e)
        {
            MySqlConnection Conexao = null;
            try
            {
                Conexao = new MySqlConnection(connString);

                Conexao.Open();

                //Cria um comando SQL para consultar contatos 
                MySqlCommand cmd = new MySqlCommand();

                //Define a conexão do comando com a conexão do banco de dados
                cmd.Connection = Conexao;

                //Define o comando SQL para buscar contatos com base no nome ou email
                cmd.CommandText = "SELECT * FROM contatos WHERE nome LIKE @q OR email LIKE @q ";



                //Adiciona o parâmetro @q com o valor do texto digitado no campo txtLocalizar
                cmd.Parameters.AddWithValue("@q", "%" + txtLocalizar.Text + "%");

                cmd.Prepare();

                //Executa o comando e obtém os resultados
                MySqlDataReader reader = cmd.ExecuteReader();

                //Limpa a lista de contatos antes de adicionar os resultados da consulta
                lstContatos.Items.Clear();

                //Percorre os resultados da consulta e adiciona cada contato à lista lstContatos
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
                //Certifica-se de que a conexão com o banco de dados seja fechada,
                //independentemente de ocorrer um erro ou não
                if (Conexao != null)
                {
                    Conexao.Close();
                }
            }
        }
        // Carrega 10 registro anteriores
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (paginaAtual > 1)
            {
                paginaAtual--;
                CarregarContatos(paginaAtual);
            }
        }

        // Carrega os proximos 10 registro
        private void btnProxima_Click(object sender, EventArgs e)
        {
            if (paginaAtual < totalPaginas)
            {
                paginaAtual++;
                CarregarContatos(paginaAtual);
            }
        }
    }
}
