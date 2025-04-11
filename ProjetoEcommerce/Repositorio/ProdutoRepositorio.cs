﻿using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.Repositorio
{
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public void Cadastrar(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = MySqlCommand("Insert into Produto (Nome, Descricao, Quantidade, Preco) values (@nome, @descricao, @quantidade, @preco)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public bool Atualizar(Produto produto)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("Update Produto set Nome = @nome, Descricao = @descricao, Quantidade = @quantidade, Preco = @preco where CodProd = @codigo", conexao);
                    cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = produto.CodProd;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar produto: {ex.Message}");
                return false;
            }
        }

        public |Enumerable<Produto> TodosProdutos()
        {
            List<Produto> Prodlist = new List<Produto>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * from produto", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Prodlist.Add(
                        new Produto
                        {
                            CodProd = Convert.ToInt32(dr["CodProd"]),
                            Nome = ((string)dr["Nome"]),
                            Descricao = ((string)dr["Descricao"]),
                            Quantidade = Convert.ToInt32(dr["Quantidade"]),
                            Preco = Convert.ToDecimal(dr["Preco"]),
                        });
                }
                return Prodlist;
            }
        }
    }
}
