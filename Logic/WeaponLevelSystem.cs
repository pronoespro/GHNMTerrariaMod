namespace GodsHaveNoMercy.Logic
{
    public class WeaponLevelSystem
    {
        string weapon;

        int exp = 0;
        int expToFirstLevel = 10;
        float expIncrementPerLevel = 0.5f;
        int level = 1;
        int maxLevel = 3;

        public void SetDefault(string name, int expToFLvl = 10, int maxlvl = 3)
        {
            weapon = name;
            expToFirstLevel = expToFLvl;
            maxLevel = maxlvl;
        }

        public void GainExp(int expGained)
        {
            exp += expGained;
            if (exp > expToFirstLevel * (1 + level * expIncrementPerLevel) && level < maxLevel)
            {
                level++;
                exp = 0;
            }
        }

        public int GetLevel() { return level; }

        public bool GetUltimate()
        {
            if (level == maxLevel && exp > expToFirstLevel * (1 + expIncrementPerLevel * level))
            {
                level = 1;
                exp = 0;
                return true;
            }
            else return false;
        }

    }
}
