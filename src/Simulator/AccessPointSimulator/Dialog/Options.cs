using System;
using System.Collections.Generic;
using System.Reflection;

namespace AccessPointSimulator.Dialog
{
    public class Options
    {
        public static bool SelectOption<T>(out T option)
            where T : struct
        {
            var map = new Dictionary<string, T>();

            var descriptor = typeof(T).GetCustomAttribute<HelpTextAttribute>();
            if (descriptor != null)
                Console.WriteLine(descriptor.HelpText);

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                var attr = typeof(T).GetField(value.ToString()).GetCustomAttribute<CommandOptionAttribute>();
                map.Add(attr.Command, value);

                Console.WriteLine("{0} - {1}", attr.Command, attr.HelpText);
            }

            Console.WriteLine();
            Console.WriteLine("Type 'exit' to exit from this menu");


            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit")
                {
                    option = default(T);
                    return false;
                }

                T value;
                if (line == null || !map.TryGetValue(line, out value))
                    continue;

                option = value;
                return true;
            }
        }

        public static void With<T>(Action<T> proceed)
            where T : class, new()
        {
            var result = new T();
            foreach (var propInfo in typeof(T).GetProperties())
            {
                var attr = propInfo.GetCustomAttribute<HelpTextAttribute>();
                if (attr == null)
                    continue;

                while (true)
                {
                    var defaultValue = propInfo.GetValue(result);
                    if (defaultValue == null)
                        Console.Write("{0}: ", attr.HelpText);
                    else
                        Console.Write("{0} [Default {1}]: ", attr.HelpText, defaultValue);

                    var line = Console.ReadLine();

                    // use default
                    if (string.IsNullOrEmpty(line) && defaultValue != null)
                        break;

                    // parse value
                    try
                    {
                        var value = Convert.ChangeType(line, propInfo.PropertyType);
                        propInfo.SetValue(result, value);
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine();
                    }
                }

            }

            proceed(result);
        }
    }
}