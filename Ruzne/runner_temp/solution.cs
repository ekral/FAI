using System.Collections.Generic;
using System.Linq;

class Solution
{
    public List<int> Run(int[] pole)
    {
        int delka = pole.Length;
       
        return [.. pole.Where(x => x > 0)];
    }
}