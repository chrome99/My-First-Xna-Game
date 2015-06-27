using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My_first_xna_game
{
    public class SkillTree
    {
        public SkillBranch[] skillBranches;

        public SkillTree()
        {
            skillBranches = new SkillBranch[] {
            new SkillBranch(SkillCollection.ballOfDestruction, new Hostile.Stats() {agility = -3, strength = 4},
                SkillCollection.wallOfFire, new Hostile.Stats() {maxHealth = 2}),

            new SkillBranch(SkillCollection.ballOfDestruction, new Hostile.Stats() {maxHealth = 5},
                SkillCollection.wallOfFire, new Hostile.Stats() {maxMana = 5})
            };
        }
    }

    public class SkillBranch
    {
        public Skill skill1;
        public Skill skill2;
        public Hostile.Stats changeStats1;
        public Hostile.Stats changeStats2;

        public SkillBranch(Skill skill1, Hostile.Stats changeStats1, Skill skill2, Hostile.Stats changeStats2)
        {
            this.skill1 = skill1;
            this.skill2 = skill2;
            this.changeStats1 = changeStats1;
            this.changeStats2 = changeStats2;
        }
    }
}
