namespace MicroServicoCidade.Configuracao
{
    public interface ICidadeMicroServicoSettings
    {
        string CidadeCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
