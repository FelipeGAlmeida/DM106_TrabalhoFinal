using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DM106_TF.Models {
    public class Product {

        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string nome { get; set; }

        public string descricao { get; set; }

        public string cor { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "O campo modelo é obrigatório")]
        [StringLength(50, ErrorMessage = "O tamanho	máximo do modelo é 50 caracteres")]
        public string modelo { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "O campo codigo é obrigatório")]
        [StringLength(8, ErrorMessage = "O tamanho	máximo do código é 8 caracteres")]
        public string codigo { get; set; }

        [Required(ErrorMessage = "O campo preço é obrigatório")]
        [Range(1, 999, ErrorMessage = "O preço deverá ser entre 1 e 999.")]
        public decimal preco { get; set; }

        [Required(ErrorMessage = "O campo peso é obrigatório")]
        public decimal peso { get; set; }

        [Required(ErrorMessage = "O campo altura é obrigatório")]
        public decimal altura { get; set; }

        [Required(ErrorMessage = "O campo largura é obrigatório")]
        public decimal largura { get; set; }

        [Required(ErrorMessage = "O campo comprimento é obrigatório")]
        public decimal comprimento { get; set; }

        [Required(ErrorMessage = "O campo diametro é obrigatório")]
        public decimal diametro { get; set; }

        [StringLength(80, ErrorMessage = "O tamanho máximo da url é 80 caracteres")]
        public string Url { get; set; }
    }
}