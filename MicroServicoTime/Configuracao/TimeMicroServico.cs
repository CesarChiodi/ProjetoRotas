namespace MicroServicoTime.Configuracao
{
    public class TimeMicroServico : ITimeMicroServico
    {
        public string TimeCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}