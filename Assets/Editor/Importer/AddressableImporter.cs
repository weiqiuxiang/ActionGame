using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Project.Editor
{
    /// <summary>
    /// Addressableフォルダ内のアセットのimport設定
    /// </summary>
    public class AddressableImporter : AssetPostprocessor
    {
        private static readonly string AddressableRootPath = "Assets/Addressable/";
        
        /// <summary>
        /// 全アセットに対して判定
        /// </summary>
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            
            OnImportAssets(settings, importedAssets);
            OnMoveAssets(settings, movedAssets, movedFromAssetPaths);
        }

        /// <summary>
        /// インポート時
        /// </summary>
        private static void OnImportAssets(AddressableAssetSettings settings, string[] importedAssets)
        {
            bool hasRegistered = false;
            foreach (var asset in importedAssets)
            {
                if (!IsValidAssetPath(asset)) continue;
                if (RegisterAsset(settings, asset))
                {
                    hasRegistered = true;
                }
            }
            
            if (hasRegistered) AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 移動時
        /// </summary>
        private static void OnMoveAssets(AddressableAssetSettings settings, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool hasChanged = false;
            for (int i = 0; i < movedAssets.Length; i++)
            {
                // 移動後もAddressableのフォルダ内
                if (IsValidAssetPath(movedAssets[i]) && RegisterAsset(settings, movedAssets[i]))
                {
                    hasChanged = true;
                    continue;
                }
                
                // 移動後はAddressableのフォルダ外
                if (IsValidMovedFromAssetPath(movedFromAssetPaths[i]) && settings.RemoveAssetEntry(AssetDatabase.AssetPathToGUID(movedAssets[i])))
                {
                    hasChanged = true;
                }
            }
            
            if (hasChanged) AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// アセットパスを見て、Addressableのグループ名とaddressableNameを取得
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private static (string groupName, string addressableName) GetFileAddressableData(string asset)
        {
            string path = asset.Remove(0, asset.IndexOf(AddressableRootPath, StringComparison.Ordinal) + AddressableRootPath.Length);
            int separateIndex = path.IndexOf("/", StringComparison.Ordinal);
            string groupName = path.Substring(0, separateIndex);
            string addressableName = path.Substring(0, path.IndexOf(".", StringComparison.Ordinal)); // 拡張子を取り除く

            return (groupName, addressableName);
        }

        private static AddressableAssetGroup TryCreateAddressableGroup(AddressableAssetSettings settings, string groupName)
        {
            var template = settings.GetGroupTemplateObject(0) as AddressableAssetGroupTemplate;
            if (template == null) return null;
            AddressableAssetGroup group = settings.CreateGroup(groupName, false, false, true, null, template.GetTypes());
            template.ApplyToAddressableAssetGroup(group);
            return group;
        }
        
        /// <summary>
        /// アセットをAddressableに登録
        /// </summary>
        /// <returns></returns>
        private static bool RegisterAsset(AddressableAssetSettings settings, string asset)
        {
            var data = GetFileAddressableData(asset);

            AddressableAssetGroup group = settings.FindGroup(data.groupName);
            if (group == null) group = TryCreateAddressableGroup(settings, data.groupName);
            if (group == null)
            {
                UnityEngine.Debug.LogError($"Addressableのグループ作成失敗、groupName={data.groupName}");
                return false;
            }
            
            string guid = AssetDatabase.AssetPathToGUID(asset);
            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = data.addressableName;

            return true;
        }

        /// <summary>
        /// 有効なアセットパスか？
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private static bool IsValidAssetPath(string asset)
        {
            if (!File.Exists(asset)) return false;  // フォルダは対象外
            if (!asset.Contains(AddressableRootPath)) return false;
            if (asset.Substring(0, asset.LastIndexOf("/", StringComparison.Ordinal) + 1).EndsWith(AddressableRootPath)) return false;
            return true;
        }
        
        private static bool IsValidMovedFromAssetPath(string asset)
        {
            if (!asset.Contains(AddressableRootPath)) return false;
            if (asset.Substring(0, asset.LastIndexOf("/", StringComparison.Ordinal) + 1).EndsWith(AddressableRootPath)) return false;
            return true;
        }
    }
}
