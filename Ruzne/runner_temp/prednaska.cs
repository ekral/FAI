using System;
using System.Linq;

int[] pole = [1,2,3];
int[] kopie1 = [..pole];
int[] kopie2 = pole.ToArray();
int[] kopie3 = new int[3];
pole.CopyTo(kopie3, 0);
var vyrez = pole[1..];
ReadOnlySpan<int> slice = pole.AsSpan(0..2);


(int[]? original, int[]? kopie) tuple = (null, null);

tuple.original = [1,2,3];
tuple.kopie = tuple.original;

Inspect.MemoryGraph(tuple);
