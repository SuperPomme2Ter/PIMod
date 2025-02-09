using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIWeapon
{
    public static class AddToExistingSynergies
    {
        public static void AddItemToSynergy(this PickupObject obj, CustomSynergyType type)
        {
            AddItemToSynergy(type, obj.PickupObjectId);
        }

        public static void AddItemToSynergy(CustomSynergyType type, int id)
        {
            foreach (AdvancedSynergyEntry entry in GameManager.Instance.SynergyManager.synergies)
            {
                if (entry.bonusSynergies.Contains(type))
                {
                    if (PickupObjectDatabase.GetById(id) != null)
                    {
                        PickupObject obj = PickupObjectDatabase.GetById(id);
                        if (obj is Gun)
                        {
                            if (entry.OptionalGunIDs != null)
                            {
                                entry.OptionalGunIDs.Add(id);
                            }
                            else
                            {
                                entry.OptionalGunIDs = new List<int> { id };
                            }
                        }
                        else
                        {
                            if (entry.OptionalItemIDs != null)
                            {
                                entry.OptionalItemIDs.Add(id);
                            }
                            else
                            {
                                entry.OptionalItemIDs = new List<int> { id };
                            }
                        }
                    }
                }
            }
        }
    }
}
