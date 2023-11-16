// See https://aka.ms/new-console-template for more information
using System.ComponentModel;
using System.Runtime.CompilerServices;


Console.WriteLine("Hello, World!");

int[] pole = [1, 2, 3, 4];
List<int> cisla = [1, 2, 3, 4];

List<int> spojene = [.. pole, .. cisla];

Vypis([10,9,8,9]);

Test t = new Test();
int res = Inv.GetVal(t, 5);

Console.WriteLine(res);

static void Vypis(IEnumerable<int> neco)
{
	string.Join(',', neco);
}

public class Test
{
	private int Get(int a) => 2 * a;
}
public class Inv
{
	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "Get")]
	public static extern int GetVal(Test t, int val);
}
