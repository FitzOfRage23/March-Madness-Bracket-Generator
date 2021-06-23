using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketGenerator
{
    class Team
    {
	    private String name;
        private int seed;
        private int index;
        private int[] seeds = new int[] { 1, 16, 8, 9, 5, 12, 4, 13, 6, 11, 3, 14, 7, 10, 2, 15 };
	    public Team(String Name, int Index)
	    {
		    name = Name;
            if (Index >= 0)
            {
                seed = seeds[Index % 16];
            }
            index = Index;
	    }
	
	    public int getIndex()
	    {
		    return index;
	    }
	
	    public int getChance()
	    {
		    int chance = (int)(Math.Sqrt(Math.Sqrt((double)(seed*seed*seed*seed*seed)))+.5);
		    return chance;
	    }
        public String getName()
        {
            return name;
        }
	    public String getDisplayName()
	    {
            if (name != "")
                return seed + ") " + name;
            else
                return "";
	    }
    }
}
