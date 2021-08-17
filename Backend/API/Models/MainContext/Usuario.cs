using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace API.Models.MainContext
{
    [Keyless]
    [Table("Usuario", Schema = "accesos")]
    public partial class Usuario
    {
        public int CodUsuario { get; set; }
        [Column("Usuario")]
        [StringLength(80)]
        public string Usuario1 { get; set; }
        public byte[] Clave { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaRegistro { get; set; }
        public int? CodInstitucion { get; set; }
        public int? CodMiembro { get; set; }
    }
}
