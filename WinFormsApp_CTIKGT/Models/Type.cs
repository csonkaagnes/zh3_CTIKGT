using System;
using System.Collections.Generic;

namespace WinFormsApp_CTIKGT.Models;

public partial class Type
{
    public byte TypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Cocktail> Cocktails { get; } = new List<Cocktail>();
}
