using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

using System.Data.SqlClient;
using Oracle.DataAccess.Client;    
 
namespace AcessoDados
{
    public class rpLerDados
    {

        #region "Métodos de Leitura Dados"

        public DataSet RetornarDataSet(string xQuery, string NomeTabela, string ConexaoOracle)
        {
            DataSet xDs = new DataSet("DS_" + NomeTabela);

            SqlConnection DB = new SqlConnection(DecriptaString(ConexaoOracle));
            SqlDataAdapter dtAdp = new SqlDataAdapter(xQuery, DB);
            dtAdp.Fill(xDs, NomeTabela);

            DB.Close();
            DB.Dispose();

            dtAdp.Dispose();

            return xDs;
        }

        public Int32 ExecutarComandoSQL(string xQuery, string NomeTabela, string ConexaoOracle)
        {
            Int32 RetornoLinhas = 0;

            OracleConnection DB = new OracleConnection(DecriptaString(ConexaoOracle));
            OracleCommand Cmd = new OracleCommand();

            DB.Open();

            Cmd.Connection = DB;
            Cmd.CommandText = xQuery;

            RetornoLinhas = Cmd.ExecuteNonQuery();

            DB.Close();
            DB.Dispose();

            return RetornoLinhas;
        }

        public string RetornarConsulta(string xQuery, string NomeTabela, string ConexaoOracle)
        {
            string Resposta = "0";

            SqlConnection DB = new SqlConnection(DecriptaString(ConexaoOracle));
            SqlCommand Cmd = new SqlCommand();
            SqlDataReader Dre;
            DB.Open();

            Cmd.Connection = DB;
            Cmd.CommandText = xQuery;
            Dre = Cmd.ExecuteReader();

            if (Dre.Read())
            {
                Resposta = Dre[0].ToString();
            }

            //Resposta = Cmd.ExecuteScalar(); //ANTIGO

            DB.Close();
            DB.Dispose();

            return Resposta;
        }

        #endregion

        #region "Decriptar"

        public string DecriptaString(string texto)
        {
            string TextoDecriptado;

            TextoDecriptado = Encoding.Unicode.GetString(Convert.FromBase64String(texto));
            return TextoDecriptado;
        }

        #endregion

    }
}
