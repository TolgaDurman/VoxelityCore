using System;
using System.Runtime.CompilerServices;
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
        public string MethodName{ get; private set; }

        public VoxelitySettingsAttribute(string name,[CallerMemberName] string caller = null)
        {
            Name = name;
            MethodName = caller;
        }
        public VoxelitySettingsAttribute(string name, int priority,[CallerMemberName] string caller = null) : this(name)
        {
            Priority = priority;
            MethodName = caller;
        }

        public VoxelitySettingsAttribute(string name, string description,[CallerMemberName] string caller = null) : this(name)
        {
            Description = description;
            MethodName = caller;
        }

        public VoxelitySettingsAttribute(string name, string description, int priority,[CallerMemberName] string caller = null) : this(name, description)
        {
            Priority = priority;
            MethodName = caller;
        }

        public VoxelitySettingsAttribute(string name,string description,int priority, Color color,[CallerMemberName] string caller = null) :this(name, description,priority)
        {
            Color = color;
            MethodName = caller;
        }
        public VoxelitySettingsAttribute(string name,Color color,[CallerMemberName] string caller = null)
        {
            Name = name;
            Color = color;
            MethodName = caller;
        }
    }
}
