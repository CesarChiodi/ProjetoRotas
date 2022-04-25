namespace MicroServicoUsuario.Configuracao
{
    public class UsuarioMicroServico : IUsuarioMicroServico
    {
        public string UsuarioCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
