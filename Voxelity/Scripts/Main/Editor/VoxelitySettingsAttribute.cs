using System;
using UnityEngine;

namespace Voxelity.Editor
{
    [AttributeUsage(AttributeTargets.Method)]
    public class VoxelitySettingsAttribute  :  Attribute
    {
        public string Name { get; private set; } = null;
        public string Description { get; private set; } = null;
        public int Priority { get; private set; } = 0;
        public Color Color { get; private set; }

        public VoxelitySettingsAttribute(string name)
        {
            Name = name;
        }
        public VoxelitySettingsAttribute(string name, int priority) : this(name)
        {
            Priority = priority;
        }

        public VoxelitySettingsAttribute(string name, string description) : this(name)
        {
            Description = description;
        }

        public VoxelitySettingsAttribute(string name, string description, int priority) : this(name, description)
        {
            Priority = priority;
        }

        public VoxelitySettingsAttribute(string name,string description,int priority, Color color) :this(name, description,priority)
        {
            Color = color;
        }
        public VoxelitySettingsAttribute(string name,Color color)
        {
            Name = name;
            Color = color;
        }
    }
}
