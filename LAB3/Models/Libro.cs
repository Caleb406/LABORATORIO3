using System;
using System.Collections.Generic;

namespace LAB3.Models;

public partial class Libro
{
    public int CodigoLibro { get; set; }

    public string? Titulo { get; set; }

    public string? Isbn { get; set; }

    public int? AnioEdicion { get; set; }

    public string? Editorial { get; set; }

    public virtual ICollection<Autor> Autors{ get; set; } = new List<Autor>();
}
