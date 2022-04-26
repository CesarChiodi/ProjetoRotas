using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Servico
{
    public class ReadFilePath
    {
        public static List<string> LerCabecalho(string caminhoRoot)
        {
            List<string> cabecalho = new List<string>();

            FileInfo existingFile = new FileInfo(caminhoRoot + "\\file\\Gerador.de.Rotas.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[PositionID: 0];
                int columnCount = worksheet.Dimension.End.Column;

                for (int coluna = 1; coluna < columnCount; coluna++)
                {
                    cabecalho.Add(worksheet?.Cells[Row: 1, coluna]?.Value?.ToString());
                }
            }
            return cabecalho;
        }

        public static List<IDictionary<string, string>> LerArquivoXls(List<string> columns, string caminhoWebRoot)
        {
            List<string> plan = new List<string>();
            List<IDictionary<string, string>> listDictonary = new();

            FileInfo arquivoXls = new FileInfo(caminhoWebRoot + "\\file\\Gerador.de.Rotas.xlsx");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new(arquivoXls))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int numeroColunas = worksheet.Dimension.End.Column;
                int numeroLinhas = worksheet.Dimension.End.Row;

                IDictionary<string, string> data = new Dictionary<string, string>();

                for (int linha = 2; linha < numeroLinhas; linha++)
                {
                    data = new Dictionary<string, string>();

                    for (int col = 1; col < numeroColunas; col++)
                    {
                        columns.ForEach(coluna =>
                        {
                            if (worksheet.Cells[1, col].Value.ToString() == coluna)
                            {
                                if (worksheet.Cells[linha, col].Value == null)
                                {
                                    data.Add(coluna, "");
                                }
                                else
                                {
                                    data.Add(coluna, worksheet.Cells[linha, col].Value.ToString());
                                }
                            }
                        });
                    }
                    if (data.Count > 1)
                    {
                        listDictonary.Add(data);
                    }
                }
            }

            return listDictonary;
        }

        public static List<string> LerColuna(string columnName, string caminhoWebRoot)
        {
            List<string> conteudoColuna = new List<string>();

            FileInfo arquivoXls = new FileInfo(caminhoWebRoot + "\\file\\Gerador.de.Rotas.xlsx");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new(arquivoXls))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int numeroColunas = worksheet.Dimension.End.Column;
                int numeroLinhas = worksheet.Dimension.End.Row;

                for (int coluna = 1; coluna < numeroColunas; coluna++)
                {
                    if (worksheet.Cells[1, coluna].Value.ToString() == columnName)
                    {
                        for (int linha = 2; linha < numeroLinhas; linha++)
                        {
                            if (worksheet.Cells[linha, coluna].Value == null)
                            {
                                conteudoColuna.Add("");
                            }
                            else
                            {
                                conteudoColuna.Add(worksheet.Cells[linha, coluna].Value.ToString());
                            }
                        }
                    }
                }
            }

            return conteudoColuna;
        }

        //public void EscreverDocx(List<Rotas> listaRota)
        //{
        //    using (StreamWriter sw = new StreamWriter(CaminhoArquivo))
        //    {
        //        foreach (Rotas rota in listaRota)
        //        {
        //            string linha = $"Os:{rota.Os}\n" + $"\nBase:{rota.Base}\n" + $"\nCep: {rota.Cep}\n" + $"\nEndereço: {rota.Endereco}\n" + $"\nNumero: {rota.Numero}\n" + $"\nBairro: {rota.Bairro}\n" + $"\nComplemento: {rota.Complemento}\n" + $"\nServiço: {rota.Servico}\n" + $"\nTime: {rota.Time}\n\n\n\n";

        //            sw.WriteLine(linha);
        //        }
        //    }
        //}
    }
}
