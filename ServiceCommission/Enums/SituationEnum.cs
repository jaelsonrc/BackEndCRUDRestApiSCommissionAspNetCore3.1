using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.Enums
{
    public enum SituationEnum
    {
        [Display(Name = "Esperando Execução")]
        EsperandoExecucao = 1,
        [Display(Name = "Esperando Pagamento")]
        EsperandoPgto = 2,
        [Display(Name = "Pagamento Não Aprovado")]
        PgtoNaoAprovado = 3,
        [Display(Name = "Pagamento Aprovado")]
        PgtoAprovado = 4,
        [Display(Name = "Order finalizado")]
        Finalizado = 5
    }
}
