namespace MicroServicoCidade.Configuracao
{
    public class CidadeMicroServicoSettings : ICidadeMicroServicoSettings
    {
        public string CidadeCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}