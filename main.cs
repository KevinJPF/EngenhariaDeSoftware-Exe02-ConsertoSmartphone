using System;
using System.Linq;
using System.Globalization;

class SmartFixConsole
{

    private static OrdensDeServico ordensDeServico = new OrdensDeServico();

    public static void Main(string[] args)
    {
        bool continua = true;

        Console.WriteLine("Bem vindo ao SmartFixConsole, seu sistema de gestao de ordens de servico.");

        do
        {
            switch (Menu())
            {
                case 0:
                    continua = false;
                    break;
                case 1:
                    ListarOrdensDeServico();
                    break;
                case 2:
                    CadastrarOrdensDeServico();
                    break;
                case 3:
                    ListarOrdensDeServico();
                    break;
                default:
                    break;
            }
            Console.Clear();
        } while (continua);

        Console.WriteLine("Obrigado por usar o SmartFixConsole, ate a proxima.\n");
    }

    private static int Menu()
    {
        int opcaoSelecionada = 0;

        Console.WriteLine("\n|Menu de Opcoes - SmartFixConsole|");
        Console.WriteLine("\n1 - Listar Ordens de Servico.");
        Console.WriteLine("2 - Criar Nova Ordem de Servico.");
        Console.WriteLine("3 - Editar Status de uma Ordem de Servico.");
        Console.WriteLine("0 - Sair do SmartFixConsole.\n");

        opcaoSelecionada = ValidarValorInteiro();
        Console.Clear();

        return opcaoSelecionada;
    }

    private static int ListarOrdensDeServico()
    {
        int opcaoSelecionada = 0;

        if (ordensDeServico.dadosOrdemDeServico.Count > 0)
        {
            Console.WriteLine("\n|Lista de Ordens de Servico - SmartFixConsole|\n");

            for (int i = 0; i < ordensDeServico.dadosOrdemDeServico.Count; i++)
            {

                var ordemDeServicoAtual = ordensDeServico.dadosOrdemDeServico[i];

                int codigo = ordemDeServicoAtual.CodigoOrdem;
                string status = ordemDeServicoAtual.GetStringStatus;
                string descricao = ordemDeServicoAtual.Descricao;
                string valor = ordemDeServicoAtual.Valor.ToString("N2");
                string dataAbertura = ordemDeServicoAtual.DataAbertura.Date.ToString("dd/MM/yyyy");
                string prazoConclusao = ordemDeServicoAtual.PrazoConclusao.Date.ToString("dd/MM/yyyy");
                string nomeCliente = ordemDeServicoAtual.Cliente.NomeCliente;
                string telefoneCliente = ordemDeServicoAtual.Cliente.TelefoneCliente;

                Console.WriteLine($"{codigo} - Status: {status}.");
                Console.WriteLine($"Cliente: {nomeCliente} | Tel: {telefoneCliente}");
                Console.WriteLine($"Descricao: {descricao} \nValor: R${valor}");
                Console.WriteLine($"Abertura: {dataAbertura} \nPrazo: {prazoConclusao}\n");
            }

            Console.WriteLine("Digite o numero correspondente a ordem de servico que queira alterar o status, ou digite qualquer outro numero para voltar ao menu.");

            do
            {
                opcaoSelecionada = ValidarValorInteiro();
            } while (opcaoSelecionada < 0);

            if (opcaoSelecionada > 0 && opcaoSelecionada <= ordensDeServico.dadosOrdemDeServico.Count)
            {
                EditarStatusDeOrdemDeServico(ordensDeServico.dadosOrdemDeServico[opcaoSelecionada - 1].CodigoOrdem);
            }
        }
        else
        {
            Console.WriteLine("\nNao ha nenhuma ordem de servico cadastrada.\nPressione qualquer tecla para voltar ao menu.\n");
            Console.ReadKey();
            return 0;
        }

        return opcaoSelecionada;
    }

    private static void CadastrarOrdensDeServico()
    {
        bool continua = true;

        do
        {
            DadosOrdemDeServico dadosNovaOrdemDeServico = new DadosOrdemDeServico();

            Console.WriteLine("Insira os dados necessarios para o cadastro da nova ordem de servico.");

            Console.WriteLine("Descricao: ");
            dadosNovaOrdemDeServico.Descricao = Console.ReadLine();

            Console.WriteLine("Valor: ");
            dadosNovaOrdemDeServico.Valor = ValidarValorDouble();

            Console.WriteLine("Prazo de Conclusao(DD/MM/AAAA): ");
            dadosNovaOrdemDeServico.PrazoConclusao = ValidarValorDateTime();

            Console.WriteLine("Nome do cliente: ");
            dadosNovaOrdemDeServico.Cliente.NomeCliente = Console.ReadLine();

            Console.WriteLine("Telefone do cliente: ");
            dadosNovaOrdemDeServico.Cliente.TelefoneCliente = Console.ReadLine();

            int codigo = ordensDeServico.CriarNovaOrdemDeServico(dadosNovaOrdemDeServico);

            Console.WriteLine($"\nOrdem de servico ({codigo}) cadastrada com sucesso.");

            Console.WriteLine("\nDeseja criar uma nova ordem de servico? (Sim/Nao) ");

            continua = Console.ReadLine().ToUpper().Contains("S") ? true : false;

            Console.Clear();
        } while (continua);
    }

    private static void EditarStatusDeOrdemDeServico(int codigoOrdemDeServico)
    {
        var ordemDeServicoParaEditar = ordensDeServico.dadosOrdemDeServico.Where(ordem => ordem.CodigoOrdem.Equals(codigoOrdemDeServico)).First();

        var statusDisponiveis = ordensDeServico.OpcoesDeStatusDisponiveis(ordemDeServicoParaEditar.Status);

        Console.Clear();

        if (ordemDeServicoParaEditar.Status == DadosOrdemDeServico.StatusServico.Cancelada || ordemDeServicoParaEditar.Status == DadosOrdemDeServico.StatusServico.Entregue)
        {
            Console.WriteLine($"O status atual da ordem de servico ({ordemDeServicoParaEditar.CodigoOrdem}) e: {ordemDeServicoParaEditar.GetStringStatus}\nNao ha status disponiveis para alterar a ordem de servico.\n\nAperte qualquer tecla para voltar ao menu.");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine($"O status atual da ordem de servico ({ordemDeServicoParaEditar.CodigoOrdem}) e: {ordemDeServicoParaEditar.GetStringStatus}\nStatus disponiveis para alterar a ordem de servico:");

            for (int i = 0; i < statusDisponiveis.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {Enum.GetName(typeof(DadosOrdemDeServico.StatusServico), statusDisponiveis[i])}");
            }

            Console.WriteLine("\n\nDigite o numero correspondente a opcao desejada:");

            int opcaoEscolhida = ValidarValorInteiro();

            ordensDeServico.DefinirNovoStatus(ordemDeServicoParaEditar, statusDisponiveis[opcaoEscolhida - 1]);

            Console.WriteLine($"O status foi alterado com sucesso para '{ordemDeServicoParaEditar.GetStringStatus}'\n\nperte qualquer tecla para voltar ao menu.");
            Console.ReadKey();
        }
    }

    private static int ValidarValorInteiro()
    {
        int valorConvertido;

        while (!int.TryParse(Console.ReadLine(), out valorConvertido))
        {
            Console.WriteLine("Valor inserido inválido.");
        }

        return valorConvertido;
    }

    private static double ValidarValorDouble()
    {
        double valorConvertido;

        while (!double.TryParse(Console.ReadLine().Replace(",", "."), out valorConvertido))
        {
            Console.WriteLine("Valor inserido inválido.");
        }

        return valorConvertido;
    }

    private static DateTime ValidarValorDateTime()
    {
        DateTime dataConvertida = DateTime.MinValue;

        while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataConvertida))
        {
            Console.WriteLine("Formato de data inválido. Digite a data no seguinte formato: 'DD/MM/AAAA'");
        }

        return dataConvertida;
    }
}