using System;
using System.Linq;
using System.Collections.Generic;

public class OrdensDeServico
{
    public List<DadosOrdemDeServico> dadosOrdemDeServico { get; set; }

    public OrdensDeServico()
    {
        dadosOrdemDeServico = new List<DadosOrdemDeServico>();
    }

    public int CriarNovaOrdemDeServico(DadosOrdemDeServico dados)
    {
        int codigoNovaOrdem = 1;
        if (dadosOrdemDeServico.Count > 0)
        {
            codigoNovaOrdem = dadosOrdemDeServico.Last().CodigoOrdem + 1;
        }
        DadosOrdemDeServico novaOrdem = new DadosOrdemDeServico(codigoNovaOrdem);
        dadosOrdemDeServico.Add(novaOrdem);

        var dadosNovaOrdemDeServico = dadosOrdemDeServico[dadosOrdemDeServico.FindLastIndex(item => true)];

        dadosNovaOrdemDeServico.Descricao = dados.Descricao;
        dadosNovaOrdemDeServico.Valor = dados.Valor;
        dadosNovaOrdemDeServico.Cliente.NomeCliente = dados.Cliente.NomeCliente;
        dadosNovaOrdemDeServico.Cliente.TelefoneCliente = dados.Cliente.TelefoneCliente;
        dadosNovaOrdemDeServico.PrazoConclusao = dados.PrazoConclusao;

        return codigoNovaOrdem;
    }

    public List<DadosOrdemDeServico.StatusServico> OpcoesDeStatusDisponiveis(DadosOrdemDeServico.StatusServico statusAtual)
    {

        List<DadosOrdemDeServico.StatusServico> statusDisponiveis = new List<DadosOrdemDeServico.StatusServico>();

        switch (statusAtual)
        {
            case DadosOrdemDeServico.StatusServico.Aberta:
                statusDisponiveis.Add(DadosOrdemDeServico.StatusServico.Cancelada);
                statusDisponiveis.Add(DadosOrdemDeServico.StatusServico.Em_Andamento);
                break;
            case DadosOrdemDeServico.StatusServico.Em_Andamento:
                statusDisponiveis.Add(DadosOrdemDeServico.StatusServico.Cancelada);
                statusDisponiveis.Add(DadosOrdemDeServico.StatusServico.Em_Espera);
                statusDisponiveis.Add(DadosOrdemDeServico.StatusServico.Concluida);
                break;
            case DadosOrdemDeServico.StatusServico.Em_Espera:
                statusDisponiveis.Add(DadosOrdemDeServico.StatusServico.Cancelada);
                statusDisponiveis.Add(DadosOrdemDeServico.StatusServico.Em_Andamento);
                break;
            case DadosOrdemDeServico.StatusServico.Concluida:
                statusDisponiveis.Add(DadosOrdemDeServico.StatusServico.Entregue);
                break;
            default:
                return null;
        }

        return statusDisponiveis;
    }

    public bool DefinirNovoStatus(DadosOrdemDeServico ordemDeServicoParaEditar, DadosOrdemDeServico.StatusServico novoStatus)
    {
        try
        {
            ordemDeServicoParaEditar.Status = novoStatus;

            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class DadosOrdemDeServico
{
    public enum StatusServico
    {
        Aberta,
        Em_Espera,
        Em_Andamento,
        Cancelada,
        Concluida,
        Entregue
    }

    private StatusServico _status;

    public int CodigoOrdem { get; private set; }
    public double Valor { get; set; }
    public string Descricao { get; set; }
    public DateTime DataAbertura { get; private set; }
    public DateTime PrazoConclusao { get; set; }
    public StatusServico Status
    {
        get { return _status; }
        set { _status = value; }
    }
    public string GetStringStatus
    {
        get { return Enum.GetName(typeof(DadosOrdemDeServico.StatusServico), _status); }
    }

    public Cliente Cliente { get; set; }

    // Metodo construtor padrao para uso como objeto para envio de dados
    public DadosOrdemDeServico()
    {
        Cliente = new Cliente();
    }

    // Metodo construtor para criar uma nova ordem de servico na lista de ordens do objeto 'OrdensDeServico'
    public DadosOrdemDeServico(int codigo)
    {
        CodigoOrdem = codigo;
        DataAbertura = DateTime.Now;
        Status = StatusServico.Aberta;
        Cliente = new Cliente();
    }
}

public class Cliente
{
    public string NomeCliente { get; set; }
    public string TelefoneCliente { get; set; }
}
