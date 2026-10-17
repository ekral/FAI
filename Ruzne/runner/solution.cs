using System;

class Solution
{
    public static void KlientskyKod()
    {
        Button b = new()
        {
            Id = 1,
            X = 10,
            Y = 20,
            Text = "Click me!",
            Enabled = true
        };

        DropDown d = new()
        {
            Id = 2,
            X = 30,
            Y = 40,
            Items = ["Option 1", "Option 2", "Option 3"],
            SelectedIndex = 0
        };
    }
}

class Control()
{
    public int Id {get; set;}
    public int X {get; set;}
    public int Y {get; set;}
}

class Button : Control
{
    public string Text {get; set;} = string.Empty;
    public bool Enabled {get; set;}
}

class DropDown : Control
{
    public string[] Items {get; set;} = [];
    public int SelectedIndex {get; set;}
}