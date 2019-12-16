namespace UniGreenModules.CommandTerminal.Scripts
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class RegisterCommandAttribute : Attribute
    {
        public int MinArgCount { get; set; } = 0;

        public int MaxArgCount { get; set; } = -1;

        public string Name { get; set; }
        public string Help { get; set; }

        public CommandPermissionLevel PermissionLevel { get; set; } = CommandPermissionLevel.Any;

        public RegisterCommandAttribute(string command_name = null) {
            Name = command_name;
        }

        public RegisterCommandAttribute(string command_name, CommandPermissionLevel level) {
            Name = command_name;
            PermissionLevel = level;
        }
    }
}
