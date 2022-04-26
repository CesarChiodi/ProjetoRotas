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

            List<string> outrosObjetos = new();
            string os = "", baseBase = "", cep = "", endereco = "", numero = "", bairro = "", complemento = "";
            int count = 0;
            string path = caminhoRoot + "\\file\\RotasGeradas.docx";

            using (StreamWriter streamwriter = new StreamWriter(path, false, Encoding.GetEncoding("iso-8859-1")))
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

                    while (count < servicoParaCadaTime)
                    {
                        string linha = "";
                        string outraString = "";

                        foreach (var outro in outrosObjetos)
                        {
                            outraString = outraString + outro;
                        }
                       
                        linha = "ROTA TRABALHO - " + DateTime.Now.ToShortDateString() + "\n\n"
                            + $"\nSERVIÇO: {servico}"
                            + $"\nTIME: {listaTimes[count]}, " + $"CIDADE: {cidade}"
                            + $"\n{baseBase}"
                            + $"\n{endereco}, {numero}   {cep}"
                            + $"\n{bairro}, {complemento}"
                            + $"\n{outraString}";

                        count++;
                        if (count < listaTimes.Count - 1 && count == 21)
                        {
                            count = 0;
                            count++;
                        }
                        streamwriter.WriteLine(linha);
                    }
                }

            }
        }
    }
}
