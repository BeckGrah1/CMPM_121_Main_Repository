using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


public class SpellBuilder 
{

    public Spell Build(SpellCaster owner)
    {
        // currently just set to arcane bolt, can be modified to take in a name
        SpellData data = SpellDataManager.Instance.Get("arcane_bolt");
        return new Spell(owner, data);
    }

   
    public SpellBuilder()
    {        
    }

}
