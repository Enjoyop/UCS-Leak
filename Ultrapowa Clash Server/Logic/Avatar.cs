using System;
using System.Collections.Generic;
using UCS.Core;
using UCS.Files.Logic;

namespace UCS.Logic
{
    internal class Avatar
    {
        protected List<DataSlot> m_heroHealth;
        protected List<DataSlot> m_heroState;
        protected List<DataSlot> m_heroUpgradeLevel;
        protected List<DataSlot> m_resourceCaps;
        protected List<DataSlot> m_resources;
        protected List<DataSlot> m_spellCount;
        protected List<DataSlot> m_spellUpgradeLevel;
        protected List<DataSlot> m_unitCount;
        protected List<DataSlot> m_unitUpgradeLevel;

        protected int m_castleLevel = -1;
        protected int m_castleTotalCapacity;
        protected int m_castleUsedCapacity;
        internal int m_townHallLevel;

        public Avatar()
        {
            this.m_resources         = new List<DataSlot>();
            this.m_resourceCaps      = new List<DataSlot>();
            this.m_unitCount         = new List<DataSlot>();
            this.m_unitUpgradeLevel  = new List<DataSlot>();
            this.m_heroHealth        = new List<DataSlot>();
            this.m_heroUpgradeLevel  = new List<DataSlot>();
            this.m_heroState         = new List<DataSlot>();
            this.m_spellCount        = new List<DataSlot>();
            this.m_spellUpgradeLevel = new List<DataSlot>();
        }

        public static int GetDataIndex(List<DataSlot> dsl, Data d) => dsl.FindIndex(ds => ds.Data == d);

        public void CommodityCountChangeHelper(int commodityType, Data data, int count)
        {
            if (data.GetDataType() == 2)
            {
                if (commodityType == 0)
                {
                    int resourceCount = GetResourceCount((ResourceData) data);
                    int newResourceValue = Math.Max(resourceCount + count, 0);

                    if (count >= 1)
                    {
                        int resourceCap = GetResourceCap((ResourceData) data);

                        if (resourceCount < resourceCap)
                        {
                            if (newResourceValue > resourceCap)
                            {
                                newResourceValue = resourceCap;
                            }
                        }
                    }

                    SetResourceCount((ResourceData) data, newResourceValue);
                }
            }
        }

        public int GetAllianceCastleLevel()
        {
            return this.m_castleLevel;
        }

        public int GetAllianceCastleTotalCapacity()
        {
            return this.m_castleTotalCapacity;
        }

        public int GetAllianceCastleUsedCapacity()
        {
            return this.m_castleUsedCapacity;
        }

        public int GetResourceCap(ResourceData rd)
        {
            int index = GetDataIndex(this.m_resourceCaps, rd);
            int count = 0;

            if (index != -1)
            {
                count = m_resourceCaps[index].Value;
            }

            return count;
        }

        public List<DataSlot> GetResourceCaps()
        {
             return this.m_resourceCaps;
        }

        public int GetResourceCount(ResourceData rd)
        {
            int index = GetDataIndex(this.m_resources, rd);
            int count = 0;

            if (index != -1)
            {
                count = m_resources[index].Value;
            }

            return count;
        }

        public List<DataSlot> GetResources()
        {
            return this.m_resources;
        }

        public List<DataSlot> GetSpells()
        {
            return this.m_spellCount;
        }

        public int GetUnitCount(CombatItemData cd)
        {
            int result = 0;

            if (cd.GetCombatItemType() == 1)
            {
                int index = GetDataIndex(this.m_spellCount, cd);

                if (index != -1)
                {
                    result = m_spellCount[index].Value;
                }
            }

            else
            {
                int index = GetDataIndex(m_vUnitCount, cd);

                if (index != -1)
                {
                    result = m_unitCount[index].Value;
                }
            }

            return result;
        }

        public List<DataSlot> GetUnits()
        {
            return this.m_unitCount;
        }

        public int GetUnitUpgradeLevel(CombatItemData cd)
        {
            int result = 0;

            switch (cd.GetCombatItemType())
            {
                case 2:
                {
                    int index = GetDataIndex(this.m_heroUpgradeLevel, cd);

                    if (index != -1)
                    {
                        result = m_heroUpgradeLevel[index].Value;
                    }

                    break;
                }

                case 1:
                {
                    int index = GetDataIndex(this.m_spellUpgradeLevel, cd);

                    if (index != -1)
                    {
                            result = m_spellUpgradeLevel[index].Value;
                    }

                    break;
                }

                default:
                {
                    int index = GetDataIndex(this.m_unitUpgradeLevel, cd);

                    if (index != -1)
                    {
                        result = m_unitUpgradeLevel[index].Value;
                    }

                    break;
                }
            }

            return result;
        }

        public int GetUnusedResourceCap(ResourceData rd)
        {
            int resourceCount = GetResourceCount(rd);
            int resourceCap   = GetResourceCap(rd);

            return Math.Max(resourceCap - resourceCount, 0);
        }

        public void SetAllianceCastleLevel(int lvl)
        {
            this.m_castleLevel = lvl;
        }

        public void IncrementAllianceCastleLevel()
        {
            this.m_castleLevel++;
        }

        public void DeIncrementAllianceCastleLevel()
        {
            this.m_castleLevel--;
        }

        public void SetTownHallLevel(int lvl)
        {
            this.m_townHallLevel = lvl;
        }

        public void IncrementTownHallLevel()
        {
            this.m_townHallLevel++;
        }

        public void DeIncrementTownHallLevel()
        {
            this.m_townHallLevel--;
        }

        public void SetAllianceCastleTotalCapacity(int totalCapacity)
        {
            this.m_castleTotalCapacity = totalCapacity;
        }

        public void SetAllianceCastleUsedCapacity(int usedCapacity)
        {
            this.m_castleUsedCapacity = usedCapacity;
        }

        public void SetHeroHealth(HeroData hd, int health)
        {
            int index = GetDataIndex(this.m_heroHealth, hd);

            if (index == -1)
            {
                DataSlot ds = new DataSlot(hd, health);
                m_heroHealth.Add(ds);
            }

            else
            {
                m_heroHealth[index].Value = health;
            }
        }

        public void SetHeroState(HeroData hd, int state)
        {
            int index = GetDataIndex(this.m_heroState, hd);

            if (index == -1)
            {
                DataSlot ds = new DataSlot(hd, state);
                m_heroState.Add(ds);
            }

            else
            {
                m_heroState[index].Value = state;
            }
        }

        public void SetResourceCap(ResourceData rd, int value)
        {
            int index = GetDataIndex(this.m_resourceCaps, rd);

            if (index == -1)
            {
                DataSlot ds = new DataSlot(rd, value);
                m_resourceCaps.Add(ds);
            }

            else
            {
                m_resourceCaps[index].Value = value;
            }
        }

        public void SetResourceCount(ResourceData rd, int value)
        {
            int index = GetDataIndex(this.m_resources, rd);

            if (index == -1)
            {
                DataSlot ds = new DataSlot(rd, value);
                m_resources.Add(ds);
            }

            else
            {
                m_resources[index].Value = value;
            }
        }

        public void SetUnitCount(CombatItemData cd, int count)
        {
            switch (cd.GetCombatItemType())
            {
                case 1:
                {
                    int index = GetDataIndex(this.m_spellCount, cd);

                    if (index != -1)
                    {
                        m_spellCount[index].Value = count;
                    }

                    else
                    {
                        DataSlot ds = new DataSlot(cd, count);
                        m_spellCount.Add(ds);
                    }

                    break;
                }

                default:
                {
                    int index = GetDataIndex(this.m_unitCount, cd);

                    if (index != -1)
                    {
                        m_unitCount[index].Value = count;
                    }

                    else
                    {
                        DataSlot ds = new DataSlot(cd, count);
                        m_unitCount.Add(ds);
                    }

                    break;
                }
            }
        }

        public void SetUnitUpgradeLevel(CombatItemData cd, int level)
        {
            switch (cd.GetCombatItemType())
            {
                case 2:
                {
                    int index = GetDataIndex(this.m_heroUpgradeLevel, cd);

                    if (index != -1)
                    {
                        m_heroUpgradeLevel[index].Value = level;
                    }

                    else
                    {
                        DataSlot ds = new DataSlot(cd, level);
                        m_heroUpgradeLevel.Add(ds);
                    }

                    break;
                }

                case 1:
                {
                    int index = GetDataIndex(this.m_spellUpgradeLevel, cd);

                    if (index != -1)
                    {
                        m_spellUpgradeLevel[index].Value = level;
                    }

                    else
                    {
                        DataSlot ds = new DataSlot(cd, level);
                        m_spellUpgradeLevel.Add(ds);
                    }

                    break;
                }

                default:
                {
                    int index = GetDataIndex(this.m_unitUpgradeLevel, cd);

                    if (index != -1)
                    {
                        m_unitUpgradeLevel[index].Value = level;
                    }

                    else
                    {
                        DataSlot ds = new DataSlot(cd, level);
                        m_unitUpgradeLevel.Add(ds);
                    }

                    break;
                }
            }
        }
    }
}
