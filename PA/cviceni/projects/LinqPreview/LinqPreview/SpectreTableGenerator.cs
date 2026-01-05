using System.Runtime.CompilerServices;

namespace LinqPreview;

using System;
using System.Collections.Generic;
using System.Reflection;
using Spectre.Console;

public class SpectreTableGenerator
{
    public static void RenderTable<T>(List<T> items, bool noWrap = false) where T : class
    {
        // Create a new table
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue)
            .Centered();

        // Get all public properties of the type
        PropertyInfo[] properties = typeof(T).GetProperties();

        // Add columns based on property names
        foreach (var property in properties)
        {
            var column = new TableColumn($"[bold]{property.Name}[/]").Centered();
            if (noWrap)
                if (property.PropertyType == typeof(string))
                {
                    //column.NoWrap();
                    //column.Width(5);
                }
            
            if (property.PropertyType == typeof(bool))
            {
                column.Width(1);
            }
            if (property.PropertyType == typeof(int))
            {
                column.Width(3);
            }
            table.AddColumn(column);
        }

        // Add rows to the table
        foreach (var item in items)
        {
            var rowValues = new List<string>();
            
            foreach (var property in properties)
            {
                // Get the value of the property for this item
                var value = property.GetValue(item)?.ToString() ?? "[grey]NULL[/]";
                
                // Apply some basic formatting
                rowValues.Add(FormatValue(value, property.PropertyType));
            }

            table.AddRow(rowValues.ToArray());
        }
        
        // Render the table
        AnsiConsole.Write(table);
    }

    private static string FormatValue(string value, Type propertyType)
    {
        // Custom formatting based on type
        if (propertyType == typeof(decimal) || propertyType == typeof(double))
            return $"[green]{value}[/]";
        
        if (propertyType == typeof(int))
            return $"[blue]{value}[/]";
        
        if (propertyType == typeof(bool))
            return value.ToLower() == "true" 
                ? "[green]✓[/]" 
                : "[red]✗[/]";
        
        if (propertyType == typeof(DateTime))
            return $"[yellow]{value}[/]";

        return value;
    }
}