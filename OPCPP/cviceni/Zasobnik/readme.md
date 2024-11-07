# Třída zásobník

Doplňte třídu Stack, tak aby byla jako zásobník implementovaný nad polem alokovaným na haldě.


```cpp
class Stack
{
private:
    int n;
    
};

int main()
{
    Stack stack(10);
    stack.push(10);
    int prvek = stack.pop();
    return 0;
}
```