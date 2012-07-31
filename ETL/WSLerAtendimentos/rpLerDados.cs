using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace WSLerAtendimentos
{
    public class rpLerDados
    {

        #region "Métodos de Leitura Dados"

        public DataSet RetornarDataSet(string xQuery, string NomeTabela, string ConexaoOracle)
        {
            DataSet xDs = new DataSet("DS_"+NomeTabela); 

            //OracleConnection DB = new OracleConnection(DecriptaString(ConexaoOracle));       
            //OracleDataAdapter dtAdp = new OracleDataAdapter(xQuery, DB);
            //dtAdp.Fill(xDs, NomeTabela);   

            //DB.Close();
            //DB.Dispose();

            //dtAdp.Dispose();

            SqlConnection DB = new SqlConnection(DecriptaString(ConexaoOracle));
            SqlDataAdapter dtAdp = new SqlDataAdapter(xQuery, DB);
            dtAdp.Fill(xDs, NomeTabela);

            DB.Close();
            DB.Dispose();

            dtAdp.Dispose();

            return xDs;
        }

        public Boolean ExecutarComandoSQL(string xQuery, string NomeTabela, string ConexaoOracle)
        {
            //OracleConnection DB = new OracleConnection(DecriptaString(ConexaoOracle));
            //OracleCommand Cmd = new OracleCommand(); 

            //DB.Open();

            //Cmd.Connection = DB;
            //Cmd.CommandText = xQuery;

            //Cmd.ExecuteNonQuery();

            //DB.Close();
            //DB.Dispose();
            
            SqlConnection DB = new SqlConnection(DecriptaString(ConexaoOracle));
            SqlCommand Cmd = new SqlCommand();

            DB.Open();

            Cmd.Connection = DB;
            Cmd.CommandText = xQuery;

            Cmd.ExecuteNonQuery();

            DB.Close();
            DB.Dispose();

            

            return true;
        }

        public string RetornarConsulta(string xQuery, string NomeTabela, string ConexaoOracle)
        {
      
            //OracleConnection DB = new OracleConnection(DecriptaString(ConexaoOracle));
            //OracleCommand Cmd = new OracleCommand();
            //OracleDataReader Dre;
            //DB.Open();

            //Cmd.Connection = DB;
            //Cmd.CommandText = xQuery;
            //Dre = Cmd.ExecuteReader();

            //if (Dre.Read())
            //{
            //    Resposta = Dre[0].ToString();
            //}

            ////Resposta = Cmd.ExecuteScalar(); //ANTIGO

            //DB.Close();
            //DB.Dispose();

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
