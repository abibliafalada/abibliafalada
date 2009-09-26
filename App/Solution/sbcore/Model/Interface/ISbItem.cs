using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace sbcore.Model.Interface
{
    public interface ISbItem
    {
        /// <summary>
        /// Uso geral
        /// </summary>
        string Tag { get; set; }

        /// <summary>
        /// Uso para exibição na tela de leitura
        /// </summary>
        string Display { get; }

        /// <summary>
        /// Listagem dos filhos, genérica para qualquer elemento
        /// </summary>
        IEnumerable<ISbItem> Children { get; }

        /// <summary>
        /// Pai do elemento, genérico para qualquer elemento
        /// </summary>
        ISbItem Parent { get; }
    }
}
