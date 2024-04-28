using System;
using System.Collections.Generic;

namespace LAB3.Models;

public partial class Autor
{
    public int IdAutor { get; set; }

    public int? CodigoLibro { get; set; }

    public string? Autor1 { get; set; }

    public string? NacionalidadAutor { get; set; }

    public string? Descripcion { get; set; }

    public virtual Libro? oLibro { get; set; }
}
