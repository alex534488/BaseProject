using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechBank : Singleton<TechBank>
{

    public class TechBankSave
    {
        /// <summary>
        /// The 'bool' represent wether or not the tech is visible
        /// </summary>
        public Dictionary<string, bool> unresearchedTech = new Dictionary<string, bool>();
    }

    public List<TechCategory> categories = new List<TechCategory>();
    public TechLine otherTechnos;


    static public TechBankSave GetData()
    {
        TechBankSave save = new TechBankSave();

        //Standard categories
        foreach (TechCategory category in instance.categories)
        {
            foreach (TechLine techline in category.techLines)
            {
                foreach (Tech tech in techline.techs)
                {
                    if (!tech.researched)
                        save.unresearchedTech.Add(tech.Name, tech.visible);
                }
            }
        }

        //Other techs
        foreach (Tech tech in instance.otherTechnos.techs)
        {
            if (!tech.researched)
                save.unresearchedTech.Add(tech.Name, tech.visible);
        }

        return save;
    }

    static public void ApplyData(TechBankSave data)
    {
        //Standard categories
        foreach (TechCategory category in instance.categories)
        {
            foreach (TechLine techline in category.techLines)
            {
                foreach (Tech tech in techline.techs)
                {
                    if (data.unresearchedTech.ContainsKey(tech.Name))
                    {
                        tech.researched = false;
                        tech.visible = data.unresearchedTech[tech.Name];
                    }
                    else
                    {
                        tech.researched = true;
                        tech.visible = true;
                    }
                }
            }
        }

        //Other techs
        foreach (Tech tech in instance.otherTechnos.techs)
        {
            if (data.unresearchedTech.ContainsKey(tech.Name))
            {
                tech.researched = false;
                tech.visible = data.unresearchedTech[tech.Name];
            }
            else
            {
                tech.researched = true;
                tech.visible = true;
            }
        }
    }
}
