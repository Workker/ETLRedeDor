using System;
using System.Collections.Generic;
using System.Web; 
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace WindowsETL.Passos
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

             MensagemErro +=  " - " + strMsgError.Trim() + " ";

             Caminho = Path.GetDirectoryName(Application.ExecutablePath) + "\\LOG";

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
                writer.WriteLine("-" + DateTime.Now.ToString("yyyyMMddHHmmss") + " - " + Ex.Message.ToString());
                writer.Close();
            }
        }


//Dim sw As StreamWriter = File.AppendText(Server.MapPath("~/logErro.txt"))
//sw.WriteLine(DateTime.Now.ToString & " : & ex.Message)
//sw.Close()



    }
}