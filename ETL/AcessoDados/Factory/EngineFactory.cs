using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace AcessoDados
{
    public enum Banco : short
    {
        SQLSERVER = 1,
        ORACLE = 2
    }

    /// <summary>
    /// Fabrica de objetos de Banco de dados
    /// </summary>
    public class EngineFactory
    {
        private IDbConnection dbConnection;
        private IDbCommand dbCommand;
        private IDbDataParameter dbParameter;
        private IDbDataAdapter dbAdapter;
        private Banco banco;

        public EngineFactory(Banco banco)
        {
            this.banco = banco;
        }

        /// <summary>
        /// Cria uma instancia de objeto que implementa <see cref="IDbConnection">IDbConnection</see>
        /// </summary>
        /// <param name="connectionString">String de conexão</param>
        /// <returns><see cref="Objeto">SqlConnection</see></returns>
        [CatchException]
        public IDbConnection CriarConnection(string connectionString)
        {

            switch (this.banco)
            {
                case Banco.SQLSERVER:
                    dbConnection = new SqlConnection(connectionString);
                    break;
                case Banco.ORACLE:
                   // dbConnection = new OracleConnection(connectionString);
                    dbConnection = new SqlConnection(connectionString);
                    break;
                default:
                    break;
            }

            return dbConnection;
        }

        /// <summary>
        /// Cria uma instancia de objeto que implementa <see cref="IDBCommand">IDBCommand</see>
        /// </summary>
        /// <returns><see cref="Objeto"/>SqlCommand</returns>
        [CatchException]
        public IDbCommand CriarCommand()
        {

            switch (this.banco)
            {
                case Banco.SQLSERVER:
                    dbCommand = new SqlCommand();
                    break;
                case Banco.ORACLE:
                    //dbCommand = new OracleCommand();
                    dbCommand = new SqlCommand();
                    break;
                default:
                    break;
            }

            
            return dbCommand;
        }

      

        /// <summary>
        /// Cria uma instancia do objeto que implementa <see cref="IDBDataParameter">IDBDataParameter</see>
        /// </summary>
        /// <param name="nomeParameter">nome do parametro</param>
        /// <param name="dbType">Tipo de dados de banco</param>
        /// <returns></returns>
        [CatchException]
        public IDataParameter CriarParameter(string nomeParameter, DbType dbType)
        {
            switch (this.banco)
            {
                case Banco.SQLSERVER:
                    dbParameter = new SqlParameter(nomeParameter, dbType);
                    break;
                case Banco.ORACLE:
                    //dbParameter = new OracleParameter(nomeParameter, dbType);
                    dbParameter = new SqlParameter(nomeParameter, dbType);
                    break;
                default:
                    break;
            }
                        
            return dbParameter;
        }

        [CatchException]
        public IDbDataAdapter CriarAdapter(string xQuery) 
        {
            switch (banco)
            {
                case Banco.SQLSERVER:
                    dbAdapter = new SqlDataAdapter(xQuery,(SqlConnection) dbConnection);
                    break;
                case Banco.ORACLE:
                    //dbAdapter = new OracleDataAdapter(xQuery, (OracleConnection)dbConnection);
                    dbAdapter = new SqlDataAdapter(xQuery, (SqlConnection)dbConnection);
                    break;
                default:
                    break;
            }

            return dbAdapter;
        }


        [CatchException]
        public string DecriptaString(string texto)
        {
            string TextoDecriptado;

            TextoDecriptado = Encoding.Unicode.GetString(Convert.FromBase64String(texto));
            return TextoDecriptado;
        }

        /// <summary>
        /// Cria uma instancia do objeto que implementa <see cref="IDBDataParameter">IDBDataParameter</see>
        /// </summary>
        /// <param name="nomeParameter">nome do parametro</param>
        /// <param name="dbType">Tipo de dados nulo do banco de dados</param>
        /// <returns></returns>
        [CatchException]
        public IDataParameter CriarParameter(string nomeParameter, DBNull dbType)
        {
            switch (this.banco)
            {
                case Banco.SQLSERVER:
                    dbParameter = new SqlParameter(nomeParameter, dbType);
                    break;
                case Banco.ORACLE:
                    //dbParameter = new OracleParameter(nomeParameter, dbType);
                    dbParameter = new SqlParameter(nomeParameter, dbType);
                    break;
                default:
                    break;
            }

            return dbParameter;
        }

    }
}
