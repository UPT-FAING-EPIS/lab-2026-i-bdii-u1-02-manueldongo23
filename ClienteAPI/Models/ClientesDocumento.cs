using System;
using System.Collections.Generic;

namespace ClienteAPI.Models;

public partial class ClientesDocumento
{
    public int IdCliente { get; set; }

    public byte IdTipoDocumento { get; set; }

    public string NumDocumento { get; set; } = null!;

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual TiposDocumento IdTipoDocumentoNavigation { get; set; } = null!;
}
