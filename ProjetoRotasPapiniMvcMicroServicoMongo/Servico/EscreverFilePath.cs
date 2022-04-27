using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Servico
{
    public class EscreverFilePath
    {
        public void EscreverWord(List<string> listaCabecalhos, List<string> listaTimes, string servico, string cidade, string caminhoRoot)
        {
            List<IDictionary<string, string>> conteudo = ReadFilePath.LerArquivoXls(listaCabecalhos, caminhoRoot);
            List<IDictionary<string, string>> servicos = new();

            foreach (var item in conteudo)
            {
                if ((item["CIDADE"] == cidade) && (item["SERVIÇO"] == servico))
                {
                    servicos.Add(item);
                }
            }

            int servicoParaCadaTime;

            if (servicos.Count > listaTimes.Count)
            {
                servicoParaCadaTime = servicos.Count / listaTimes.Count;
            }
            else if (servicos.Count > listaTimes.Count)
            {
                servicoParaCadaTime = servicos.Count / listaTimes.Count;
            }
            else
            {
                servicoParaCadaTime = 1;
            }

            List<string> outrosObjetos = new List<string>();

            string os = "", baseBase = "", cep = "", endereco = "", numero = "", bairro = "", complemento = "";
            int count = 0;
            int contTime = 0;
            string caminho = caminhoRoot + "\\file\\RotasGeradas.docx";

            using (StreamWriter streamwriter = new StreamWriter(caminho, false, Encoding.GetEncoding("iso-8859-1")))
            {
                foreach (var item in servicos)
                {
                    foreach (var cabecalho in listaCabecalhos)
                    {
                        if (cabecalho == "OS")
                        {
                            os = cabecalho + ": " + item[cabecalho];
                        }
                        else if (cabecalho == "BASE")
                        {
                            baseBase = cabecalho + ": " + item[cabecalho];
                        }
                        else if (cabecalho == "CEP")
                        {
                            cep = cabecalho + ": " + item[cabecalho];
                        }
                        else if (cabecalho == "ENDEREÇO")
                        {
                            endereco = cabecalho + ": " + item[cabecalho];
                        }
                        else if (cabecalho == "NUMERO")
                        {
                            numero = cabecalho + ": " + item[cabecalho];
                        }
                        else if (cabecalho == "BAIRRO")
                        {
                            bairro = cabecalho + ": " + item[cabecalho];
                        }
                        else if (cabecalho == "COMPLEMENTO")
                        {
                            complemento = cabecalho + ": " + item[cabecalho];
                        }
                        else if (listaCabecalhos.Count > 9 && cabecalho != "SERVIÇO" && cabecalho != "CIDADE")
                        {
                            outrosObjetos.Add("\n" + cabecalho + ": " + item[cabecalho]);
                        }
                    }

                    if (count < servicoParaCadaTime)
                    {
                        string linha = "";
                        string outraString = "";

                        foreach (var outro in outrosObjetos)
                        {
                            outraString = outraString + outro;
                        }

                        linha = DateTime.Now.ToShortDateString() + " ROTAS: " + $"\nOS: {os}" + $"\nSERVIÇO: {servico}" + $"\nTIME: {listaTimes[contTime]} " + $"\nCIDADE: {cidade}" + $"\n{baseBase}" + $"\n{endereco}\n{numero}\n{cep}" + $"\n{bairro}\n{complemento}" + $"\n{outraString}\n_-_-_-\n";

                        count++;
                        if (contTime < listaTimes.Count - 1 && count == servicoParaCadaTime)
                        {
                            count = 0;
                            contTime++;
                        }

                        streamwriter.WriteLine(linha);
                    }
                }
            }
        }
    }
}
