namespace MicroServicoPessoa.Configuracao
{
    public interface IPessoaMicroServico
    {
        string PessoaCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
