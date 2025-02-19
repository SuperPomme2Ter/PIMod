using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIWeapon
{
    public static class AddToExistingSynergies
    {
        public static void AddItemToSynergy(this PickupObject obj, CustomSynergyType type,bool priority=false)
        {
            AddItemToSynergy(type, obj.PickupObjectId,priority);
        }

        public static void AddItemToSynergy(CustomSynergyType type, int id, bool priority)
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
                            List<int> priorityList;
                            if (priority) 
                            {
                                priorityList=entry.MandatoryGunIDs;
                            }
                            else
                            {
                                priorityList=entry.OptionalGunIDs;
                            }
                            if (priorityList != null)
                            {
                                priorityList.Add(id);
                            }
                            else
                            {
                                priorityList = new List<int> { id };
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
