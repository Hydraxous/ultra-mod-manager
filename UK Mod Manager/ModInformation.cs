﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UKMM.Loader;

namespace UKMM
{
    public class ModInformation
    {
        public ModType modType;
        public Type mod;
        public string modName;
        public string modDescription;
        public string modVersion;
        public bool supportsUnloading;
        public bool loadOnStart;
        public bool loaded;


        public ModInformation(Type mod, ModType modType)
        {
            this.modType = modType;
            this.mod = mod;

            // TODO: Read mod name from toml file
            if (modType == ModType.BepInPlugin)
            {
                //modName = GetBepinMetaData(mod).Name;
                modName = mod.Assembly.FullName;
                modDescription = "Mod descriptions are not supported by BepInEx plugins.";
            }
            else if (modType == ModType.UKMod)
            {
                UKPlugin metaData = UKModManager.GetUKMetaData(mod);
                modName = metaData.name;
                modDescription = metaData.description;
                modVersion = metaData.version;
                supportsUnloading = metaData.unloadingSupported;
            }
        }

        public void Clicked()
        {
            if (!loaded)
                LoadThisMod();
            else
                UnLoadThisMod();
        }

        public void LoadThisMod()
        {
            if (!loaded)
            {
                UKModManager.LoadMod(this);
                loaded = true;
            }
        }

        public void UnLoadThisMod()
        {
            if (loaded && supportsUnloading)
            {
                UKModManager.UnloadMod(this);
                loaded = false; 
            }
        }

        public enum ModType
        {
            UKMod,
            BepInPlugin
        }
    }
}