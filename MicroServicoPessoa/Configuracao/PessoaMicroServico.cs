namespace MicroServicoPessoa.Configuracao
{
    public class PessoaMicroServico : IPessoaMicroServico
    {
        public string PessoaCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
