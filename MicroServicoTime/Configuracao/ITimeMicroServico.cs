namespace MicroServicoTime.Configuracao
{
    public interface ITimeMicroServico
    {
        string TimeCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}