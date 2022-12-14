﻿using System;
using System.Collections.Generic;

namespace WinFormsApp_CTIKGT.Models;

public partial class Cocktail
{
    public int CocktailSk { get; set; }

    public string Name { get; set; } = null!;

    public byte? TypeFk { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Recipe> Recipes { get; } = new List<Recipe>();

    public virtual Type? TypeFkNavigation { get; set; }
}
