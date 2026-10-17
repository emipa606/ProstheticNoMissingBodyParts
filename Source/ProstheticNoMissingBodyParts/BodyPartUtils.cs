using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ProstheticNoMissingBodyParts;

public static class BodyPartUtils
{
    public static bool ExistsDeep(BodyPartRecord part, BodyPartGroupDef groupDef)
    {
        // check if correct part is founded
        if (part.groups.Exists(x => x == groupDef))
        {
            return true;
        }

        return part.parts != null &&
               // recursive check for nested parts
               Enumerable.Any(part.parts, nestedPart => ExistsDeep(nestedPart, groupDef));
    }

    public static bool ExistsByGroupAndParent(List<BodyPartRecord> parts, BodyPartDef parentDef, BodyPartGroupDef groupDef)
    {
        var parentParts = parts.FindAll(x => x?.def == parentDef);
        return parentParts.Exists(x => ExistsDeep(x, groupDef));
    }

    public static List<T> ResolveDefs<T>(List<string> defNames) where T : Def
    {
        var list = new List<T>(defNames.Count);
        defNames.Sort();
        foreach (var defName in defNames)
        {
            var part = DefDatabase<T>.GetNamedSilentFail(defName);
            if (part != null)
            {
                list.Add(part);
            }
        }

        return list;
    }

}
