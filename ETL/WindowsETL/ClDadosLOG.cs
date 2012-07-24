using System;
using System.Collections.Generic;
using System.Web; 
using System.Linq;
using System.IO;
using Dor.Util;
using Dor.SumarioAlta;
 
namespace Dor.SumarioAlta
{
    public class ClDadosLOG
    {

        public void CriarArquivoLog(string strMsgError, string strUnidade)
        {
            string Caminho = string.Empty;
            string nomeArquivo = string.Empty;
            string MensagemErro = string.Empty;

            //ORIGINAL - MensagemErro = "- " + DateTime.Now.ToString("yyyyMMddHHmmss") + " - ID Sumario : " + UtSessao.Sessao["Id058"].ToString() + " / ID Usuario: " + UtSessao.Sessao["CODIGOUSR"].ToString() + "  - " + strMsgError.Trim() + " ";

             MensagemErro = "- " + DateTime.Now.ToString("yyyyMMddHHmmss");

             if (UtSessao.Sessao["Id058"] != null)
             {
                 MensagemErro += " - ID Sumario : " + UtSessao.Sessao["Id058"].ToString(); 
             }
             if (UtSessao.Sessao["CODIGOUSR"] != null)
             {
                 MensagemErro += " / ID Usuario: " + UtSessao.Sessao["CODIGOUSR"].ToString();
             }
            
             MensagemErro +=  " - " + strMsgError.Trim() + " ";

             ClEnvioEmail EnviarMensagem = new ClEnvioEmail();

             //ORIGINAL - EnviarMensagem.EnviarEmail(" ", "ALERTA - SUMARIO DE ALTA - ID Sumario : " + UtSessao.Sessao["Id058"].ToString() + " / ID Usuario: " + UtSessao.Sessao["CODIGOUSR"].ToString(), MensagemErro);

             if (UtSessao.Sessao["Id058"] != null)
             {
                 EnviarMensagem.EnviarEmail(" ", "ALERTA - SUMARIO DE ALTA - ID Sumario : " + UtSessao.Sessao["Id058"].ToString(), MensagemErro);
             }
             else
             {
                 EnviarMensagem.EnviarEmail(" ", "ALERTA - SUMARIO DE ALTA", MensagemErro);
             }

             Caminho = System.Web.HttpContext.Current.Server.MapPath("~\\LOG");

           // Caminho = "C://LOG";

            nomeArquivo = "Log_" + strUnidade+ "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

            DirectoryInfo Diretorio = new DirectoryInfo(@Caminho);

            if (!Diretorio.Exists)
            {
                Directory.CreateDirectory(Diretorio.FullName.Trim());
            }

            Caminho = Diretorio.FullName.Trim() + "\\" + nomeArquivo.Trim();

            StreamWriter writer = new StreamWriter(Caminho);

            try
            {
                //writer.WriteLine("-" + DateTime.Now.ToString("yyyyMMddHHmmss") + " - ID Sumario : " + UtSessao.Sessao["Id058"].ToString() + " / ID Usuario: " + UtSessao.Sessao["CODIGOUSR"].ToString() + "  - " + strMsgError.Trim() + " ");
                writer.WriteLine(MensagemErro);
                writer.Close();
                
            }
            catch (Exception Ex)
            {
                writer.WriteLine("-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "   ID Sumario : " + UtSessao.Sessao["Id058"].ToString() + " ID Usuario: " + UtSessao.Sessao["CODIGOUSR"].ToString() + " - " + Ex.Message.ToString());
                writer.Close();
            }
        }


//Dim sw As StreamWriter = File.AppendText(Server.MapPath("~/logErro.txt"))
//sw.WriteLine(DateTime.Now.ToString & " : & ex.Message)
//sw.Close()



    }
}