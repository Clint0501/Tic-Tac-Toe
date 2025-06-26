using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Script.GameCore.PlayerCtrl
{
    public class PlayerAttribute : Attribute
    {
        public GameModeEnum m_Mode;
        public PlayerAttribute(GameModeEnum key)
        {
            m_Mode = key;
        }
    }
    
    public static class AIPlayerFactory
    {
        private static Dictionary<GameModeEnum, Type> key2Import = new Dictionary<GameModeEnum, Type>();

        public static BasePlayer GetPlayer(GameModeEnum mode, params object[] args)
        {
            if (key2Import.Count == 0)
            {
                Init();
            }

            if (key2Import.TryGetValue(mode, out Type aiPlayerType))
            {
                try
                {
                    // 创建产品实例
                    return (BasePlayer) Activator.CreateInstance(aiPlayerType, args);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"创建 {mode}的玩家信息{aiPlayerType.FullName} 失败: {ex.Message}");
                    return null;
                }
            }
            else
            {
                Debug.LogWarning($"{mode}玩家没有注册。");
                return null;
            }
        }

        private static void Init()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    Type[] types = assembly.GetTypes();

                    foreach (Type type in types)
                    {
                        // 检查类型是否是类，并且不是抽象类或接口
                        if (type.IsClass && !type.IsAbstract && typeof(BasePlayer).IsAssignableFrom(type))
                        {
                            PlayerAttribute attribute = type.GetCustomAttribute<PlayerAttribute>(false); // false表示不继承

                            if (attribute != null)
                            {
                                if (key2Import.ContainsKey(attribute.m_Mode))
                                {
                                    Debug.LogWarning($"发现重复的刷新功能: {attribute.m_Mode}。类 {type.FullName} 将覆盖 {key2Import[attribute.m_Mode].FullName}");
                                }

                                key2Import[attribute.m_Mode] = type;
                            }
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Debug.LogError($"IPlayerFactory 初始化错误：无法从程序集 {assembly.FullName} 加载类型: {ex.Message}");
                    foreach (var loaderException in ex.LoaderExceptions)
                    {
                        Debug.LogError($"加载失败信息: {loaderException.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"IPlayerFactory 初始化错误：处理程序集 {assembly.FullName} 时发生错误: {ex.Message}");
                }
            }
        }
    }
}