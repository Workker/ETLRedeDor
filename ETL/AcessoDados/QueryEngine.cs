using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace AcessoDados
{
    public class QueryEngine
    {
        private Engine engine;
        private EngineFactory engineFactory;
        private IDataParameter parameter;
        private List<IDataParameter> listaParameter;

        /// <summary>
        /// Inicia as configurações basicas na classe
        /// </summary>
        public QueryEngine(string str, Banco banco)
        {
            engine = new Engine(str, banco);
            engineFactory = new EngineFactory(Banco.SQLSERVER);
            listaParameter = new List<IDataParameter>();
        }

        /// <summary>
        /// Método recursivo para preenchimento de uma coleção do tipo<see cref="IDataParameter">IDataParameter</see>
        /// </summary>
        /// <param name="source">source</param>
        /// <returns>IDataParemeter</returns>
        [CatchException]
        public List<IDataParameter> Fill(object source)
        {
            return Fill(source, string.Empty);
        }

        /// <summary>
        /// Método recursivo para preenchimento de uma coleção do tipo<see cref="IDataParameter">IDataParameter</see>
        /// </summary>
        /// <param name="source">source</param>
        /// <returns>IDataParemeter</returns>
        [CatchException]
        public List<IDataParameter> Fill(object source, string parentClass)
        {
            PropertyInfo[] sourcePropertyInfo = source.GetType().GetProperties();

            foreach (PropertyInfo property in sourcePropertyInfo)
            {
                object value = property.GetValue(source, new object[] { });

                if (value == null)
                {
                    parameter = CreateNullParameter(property, parentClass);
                    listaParameter.Add(parameter);
                }

                if (value.GetType().Namespace != "System")
                {
                    Fill(value, property.Name);
                }
                else
                {
                    parameter = CreateParameter(property, parentClass, value);
                    listaParameter.Add(parameter);
                }

            }

            return listaParameter;
        }

        [CatchException]
        public IDataParameter CreateParameter(PropertyInfo propertyParameter, string parentClass, object value)
        {

            if (propertyParameter.PropertyType.FullName == "System.String")
            {
                string strValue = (string)value;
                if (!string.IsNullOrEmpty(strValue))
                {
                    parameter = engineFactory.CriarParameter(string.Concat(parentClass, propertyParameter.Name), DbType.String);
                    parameter.Value = strValue;
                    return parameter;
                }
                else
                    return CreateNullParameter(propertyParameter, parentClass);
            }
            else if (propertyParameter.PropertyType.Namespace.Equals("System.Collections.Generic"))
            {
                string strValue = (string)value;
                if (!string.IsNullOrEmpty(strValue))
                {
                    parameter = engineFactory.CriarParameter(string.Concat(parentClass, propertyParameter.Name), DbType.String);
                    parameter.Value = strValue;
                    return parameter;
                }
                else
                    return CreateNullParameter(propertyParameter, parentClass);
            }
            else if (propertyParameter.PropertyType.FullName.Contains("System.Boolean"))
            {

                bool valuecontinue = (value == null) ? false : true;

                if (valuecontinue)
                {
                    bool boolValue = (bool)value;
                    parameter = engineFactory.CriarParameter(string.Concat(parentClass, propertyParameter.Name), DbType.Boolean);
                    parameter.Value = boolValue;
                    return parameter;
                }
                else
                    return CreateNullParameter(propertyParameter, parentClass);
            }

            else if (propertyParameter.PropertyType.FullName == "System.DateTime")
            {
                bool valuecontinue = (value == null) ? false : true;


                if (valuecontinue)
                {
                    DateTime dataValue = (DateTime)value;
                    parameter = engineFactory.CriarParameter(string.Concat(parentClass, propertyParameter.Name), DbType.DateTime);
                    parameter.Value = dataValue;
                    return parameter;
                }
                else
                    return CreateNullParameter(propertyParameter, parentClass);

            }
            else if (propertyParameter.PropertyType.FullName == "System.Decimal")
            {
                bool valueContinue = (value == null) ? false : true;

                if (valueContinue)
                {
                    decimal decimalvalue = (decimal)value;
                    parameter = engineFactory.CriarParameter(string.Concat(parentClass, propertyParameter.Name), DbType.Decimal);
                    parameter.Value = value;
                    return parameter;
                }
                else
                    return CreateNullParameter(propertyParameter, parentClass);
            }
            else if (propertyParameter.PropertyType.FullName == "System.Int32")
            {
                bool valueContinue = (value == null) ? false : true;

                if (valueContinue)
                {
                    int decimalvalue = (int)value;
                    parameter = engineFactory.CriarParameter(string.Concat(parentClass, propertyParameter.Name), DbType.Int32);
                    parameter.Value = value;
                    return parameter;
                }
                else
                    return CreateNullParameter(propertyParameter, parentClass);
            }



            return null;
        }

        /// <summary>
        /// Cria um parameter null para um criterio de consulta aonde o parametro é nulo
        /// </summary>
        /// <param name="sourceProperty">informações da propriedade</param>
        /// <param name="parentClass">nome da classe da qual a propriedade pertence</param>
        /// <returns></returns>
        [CatchException]
        public IDataParameter CreateNullParameter(PropertyInfo sourceProperty, string parentClass)
        {
            parameter = engineFactory.CriarParameter(string.Concat(parentClass, sourceProperty.Name), DBNull.Value);
            parameter.Value = null;
            return parameter;
        }

        /// <summary>
        /// executa a procedure com o critério
        /// </summary>
        /// <param name="procName">Nome da procedure</param>
        /// <returns>IDataReader</returns>
        [CatchException]
        public IDataReader ExecuteQueryProcedure(string procName, object criterio)
        {
            try
            {
                //preenchendo os parameters
                Fill(criterio);
                return engine.ExecuteQueryProcedure(procName, listaParameter);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// executa a procedure com o critério
        /// </summary>
        /// <param name="procName">Nome da procedure</param>
        /// <returns>Object</returns>
        [CatchException]
        public object ExecuteEscalarProcedure(string procName, object criterio)
        {
               //preenchendo os parameters
                Fill(criterio);
                return engine.ExecuteEscalarProcedure(procName, listaParameter);
            
        }
    }
}
