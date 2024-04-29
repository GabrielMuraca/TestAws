// See https://aka.ms/new-console-template for more information
using System.Linq;

var insts = new int[];
var k = 8;

insts = insts.ToList().OrderBy(x => x).ToArray();

if(NewMethod(insts, k))
    Console.WriteLine("es");
else
    Console.WriteLine("no");

static bool NewMethod(int[] ints, int k)
{
    while (ints.Length > 0)
    {
        var mid = ints.Length / 2;

        if (ints[mid] == k)
            return true;
        else if (ints.Length == 1)
            return false;
        else if (ints[mid] > k)
            ints = ints.Take(mid).ToArray();
        else
            ints = ints.Skip(mid).ToArray();
    }
    return false;
}

