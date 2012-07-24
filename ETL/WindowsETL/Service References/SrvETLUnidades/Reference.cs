
namespace WindowsETL.SrvETLUnidades
{
    using System.Data;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://microsoft.com/webservices/", ConfigurationName = "SrvETLUnidades.WSETLUnidadesSoap")]
    public interface WSETLUnidadesSoap
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://microsoft.com/webservices/RetornarUnidades", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        System.Data.DataSet RetornarUnidades();

        [System.ServiceModel.OperationContractAttribute(Action = "http://microsoft.com/webservices/ApagarPacientesSatelite", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        void ApagarPacientesSatelite(string m_sUnidade);

        [System.ServiceModel.OperationContractAttribute(Action = "http://microsoft.com/webservices/ApagarAtendimentosSatelite", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        void ApagarAtendimentosSatelite(string m_sUnidade);


    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WSETLUnidadesSoapChannel : WindowsETL.SrvETLUnidades.WSETLUnidadesSoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WSETLUnidadesSoapClient: System.ServiceModel.ClientBase<WindowsETL.SrvETLUnidades.WSETLUnidadesSoap>, WindowsETL.SrvETLUnidades.WSETLUnidadesSoap
    {
        public System.Data.DataSet RetornarUnidades()
        {
            return base.Channel.RetornarUnidades();
        }


        public void ApagarPacientesSatelite(string m_sUnidade)
        { 
        }

        public void ApagarAtendimentosSatelite(string m_sUnidade)
        {
        }

    }
}



