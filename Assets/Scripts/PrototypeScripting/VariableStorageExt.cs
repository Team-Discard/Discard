using System;
using Yarn.Unity;

namespace PrototypeScripting
{
    public static class VariableStorageExt
    {
        // todo: to:billy damn yarn spinner uses float for all numbers. This can lead to all sorts of problems
        public static void UpdateFloat(this VariableStorageBehaviour storage, string variable, Func<int, int> updater)
        {
            if (!storage.TryGetValue<float>(variable, out var val))
            {
                val = default;
            }

            val = updater((int)val);
            storage.SetValue(variable, val);
        }

        public static void IncrementFloat(this VariableStorageBehaviour storage, string variable)
        {
            storage.UpdateFloat(variable, val => val + 1);
        }
    }
}