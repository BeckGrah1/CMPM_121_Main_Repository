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
        // SpellData data = SpellDataManager.Instance.Get("arcane_bolt");
        // return new Spell(owner, data);
        SpellData baseData = SpellDataManager.Instance.Get("arcane_bolt");
        SpellData geoData = SpellDataManager.Instance.Get("geometry");
        SpellData vampData = SpellDataManager.Instance.Get("vampire");
        SpellData homingData = SpellDataManager.Instance.Get("homing");

        Spell baseSpell = new Spell(owner, baseData);
        Spell geoSpell = new GeometrySpell(owner, geoData, baseSpell);
        Spell vampSpell = new VampireSpell(owner, vampData, geoSpell);
        return new ModifierSpell(owner, homingData, vampSpell);
    }

   
    public SpellBuilder()
    {        
    }

}
