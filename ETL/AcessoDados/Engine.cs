using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace AcessoDados
{
    /// <summary>
    /// Objeto encarregado de acessar o banco de dados
    /// </summary>
    public class Engine
    {
        private IDbConnection dbConnection;
        private IDbCommand dbCommand;
        private IDbTransaction dbTransaction;
        private string connectionString;
        private EngineFactory factory;

        /// <summary>
        /// Define as configuraçoes basicas do objeto
        /// </summary>
        public Engine(string str, Banco banco)
        {
            factory = new EngineFactory(banco);
            connectionString = factory.DecriptaString(str);
            dbCommand = factory.CriarCommand();
            dbConnection = factory.CriarConnection(connectionString);
        }

        /// <summary>
        /// abre a conexao com o banco de dados
        /// </summary>
        /// <returns></returns>
        [CatchException]
        public IDbConnection GetConnection()
        {

            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();


            return dbConnection;
        }

        /// <summary>
        /// Fecha a conexão com o banco de dados
        /// </summary>
        [CatchException]
        public void CloseConnection()
        {
            if (dbConnection != null)
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
        }

        [CatchException]
        private void CriarComando(string text, CommandType command)
        {
            if (string.IsNullOrEmpty(text))
                throw new Exception("Query não informada");

            dbCommand.Connection = GetConnection();
            dbCommand.CommandType = command;
            dbCommand.CommandText = text;
        }

        [CatchException]
        public int ExecutaComando(string xQuery)
        {
            CriarComando(xQuery, CommandType.Text);
            var linhas = dbCommand.ExecuteNonQuery();
            CloseConnection();

            return linhas;
        }

        [CatchException]
        public DataSet RetornarDataSet(string xQuery, string nomeTabela)
        {
            DataSet dataSetDs = new DataSet(nomeTabela);


            IDbDataAdapter adapter = factory.CriarAdapter(xQuery);
            adapter.Fill(dataSetDs);

            CloseConnection();

            return dataSetDs;
        }

        [CatchException]
        public string RetornarConsulta(string xQuery, string NomeTabela)
        {

            string resposta;
            IDataReader reader;
            try
            {
                resposta = "0";

                CriarComando(xQuery, CommandType.Text);
                reader = dbCommand.ExecuteReader();

                if (reader.Read())
                {
                    resposta = reader[0].ToString();
                }
            }
            finally
            {
                CloseConnection();
            }
            return resposta;
        }

        /// <summary>
        /// Executa uma procedure do tipo query e retorna um IdataReader com as informações retornadas do banco
        /// </summary>
        /// <param name="procName">Nome da procedure</param>
        /// <param name="paramCollection">Coleção de parametros</param>
        /// <returns><see cref="Objeto">IDataReader</see></returns>
        [CatchException]
        public IDataReader ExecuteQueryProcedure(string procName, List<IDataParameter> paramCollection)
        {
            //Cria o comando na fabrica
            CriarComando(procName, CommandType.StoredProcedure);

            //Varre a coleção de parametros e adiciona no comand
            if (paramCollection != null)
            {
                for (int i = 0; i < paramCollection.Count; i++)
                {
                    dbCommand.Parameters.Add(paramCollection[i]);
                }
            }
            //executa a procedure e retorna um IdataReader
            return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Executa uma procedure do tipo query e retorna um IdataReader com as informações retornadas do banco
        /// </summary>
        /// <param name="procName">Nome da procedure</param>
        /// <param name="paramCollection">Coleção de parametros</param>
        /// <returns><see cref="Objeto">IDataReader</see></returns>
        [CatchException]
        public IDataReader ExecuteQuery(string command, List<IDataParameter> paramCollection)
        {
            //Cria o comando na fabrica
            CriarComando(command, CommandType.Text);

            //Varre a coleção de parametros e adiciona no comand
            if (paramCollection != null)
            {
                for (int i = 0; i < paramCollection.Count; i++)
                {
                    dbCommand.Parameters.Add(paramCollection[i]);
                }
            }
            //executa a procedure e retorna um IdataReader
            return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Executa uma procedure do tipo query e retorna um IdataReader com as informações retornadas do banco
        /// </summary>
        /// <param name="procName">Nome da procedure</param>
        /// <param name="paramCollection">Coleção de parametros</param>
        /// <returns><see cref="Objeto">IDataReader</see></returns>
        [CatchException]
        public int ExecuteNonQuery(string command, List<IDataParameter> paramCollection)
        {
            try
            {
                //Cria o comando na fabrica
                CriarComando(command, CommandType.Text);

                //Varre a coleção de parametros e adiciona no comand
                if (paramCollection != null)
                {
                    for (int i = 0; i < paramCollection.Count; i++)
                    {
                        dbCommand.Parameters.Add(paramCollection[i]);
                    }
                }
                //executa a procedure e retorna um IdataReader
                dbTransaction = dbConnection.BeginTransaction();

                int retorno = dbCommand.ExecuteNonQuery();

                if (Convert.ToBoolean(retorno))
                    dbTransaction.Commit();
                else
                    dbTransaction.Rollback();


                return retorno;
            }
            finally
            {
                dbTransaction.Dispose();
            }
        }

        /// <summary>
        /// Executa uma procedure do tipo Escalar e retorna um Tipo Escalar
        /// </summary>
        /// <param name="procName">Nome da procedure</param>
        /// <param name="paramCollection">Coleção de parametros</param>
        /// <returns><see cref="Objeto">Object</see></returns>
        [CatchException]
        public object ExecuteEscalarProcedure(string procName, List<IDataParameter> paramCollection)
        {
            try
            {
                //Cria o comando na fabrica
                CriarComando(procName, CommandType.StoredProcedure);

                //Varre a coleção de parametros e adiciona no comand
                if (paramCollection != null)
                {
                    ConvertDataParameters(paramCollection);
                    for (int i = 0; i < paramCollection.Count; i++)
                    {
                        dbCommand.Parameters.Add(paramCollection[i]);
                    }
                }
                //executa a procedure e retorna um Escalar
                return dbCommand.ExecuteScalar();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// converte todos os parametros DateTime para string
        /// </summary>
        /// <param name="paramCollection"></param>
        /// <returns></returns>
        [CatchException]
        public void ConvertDataParameters(List<IDataParameter> paramCollection)
        {
            foreach (IDataParameter param in paramCollection.Where(i => i.Value is DateTime))
            {
                Convert.ToString(param);
            }
        }

    }
}
