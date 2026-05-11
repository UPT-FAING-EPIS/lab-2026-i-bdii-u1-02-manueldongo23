using System;
using System.Collections.Generic;

namespace ClienteAPI.Models;

public partial class TiposDocumento
{
    public byte IdTipoDocumento { get; set; }

    public string DesTipoDocumento { get; set; } = null!;

    public virtual ICollection<ClientesDocumento> ClientesDocumentos { get; set; } = new List<ClientesDocumento>();
}
