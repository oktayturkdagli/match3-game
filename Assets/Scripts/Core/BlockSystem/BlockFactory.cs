using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public static class BlockFactory
{
    private static Dictionary<BlockTypes, Type> blocksByName;

    private static bool IsInitialized => blocksByName != null;

    private static void InitializeBlockFactory()
    {
        if (IsInitialized)
            return;
        
        var blockTypes = Assembly.GetAssembly(typeof(Block))
            .GetTypes()
            .Where(type => IsBlockClass(type) && IsNotCubeBlock(type));

        blocksByName = new Dictionary<BlockTypes, Type>();

        foreach (var type in blockTypes)
        {
            var blockInstance = CreateBlockInstance(type);
            blocksByName.Add(blockInstance.blockType, type);
        }
    }

    private static bool IsBlockClass(Type type)
    {
        return type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Block));
    }

    private static bool IsNotCubeBlock(Type type)
    {
        return !type.IsSubclassOf(typeof(CubeBlock));
    }

    private static Block CreateBlockInstance(Type type)
    {
        return Activator.CreateInstance(type) as Block;
    }

    public static Block GetBlockWithBlockType(BlockTypes blockType)
    {
        InitializeBlockFactory();

        if (blocksByName.TryGetValue(blockType, out var type))
        {
            var block = CreateBlockInstance(type);
            return block;
        }

        return null;
    }
}