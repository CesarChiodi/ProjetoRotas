namespace MicroServicoUsuario.Configuracao
{
    public interface IUsuarioMicroServico
    {
        string UsuarioCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
