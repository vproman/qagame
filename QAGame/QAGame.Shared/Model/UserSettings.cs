using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace QAGame.Model
{
    public class UserSettings
    {
        ApplicationDataContainer settings;
        public event EventHandler SettingsChanged;

        const string SettingBooleanKeyName = "SettingBoolean";
        const bool SettingBooleanDefault = true;
        public bool SettingBoolean
        {
            get
            {
                return GetValueOrDefault<bool>(SettingBooleanKeyName, SettingBooleanDefault);
            }
            set
            {
                if (AddOrUpdateValue(SettingBooleanKeyName, value))
                {
                    Save();
                }
            }
        }

        const string SettingIntegerKeyName = "SettingInteger";
        const int SettingIntegerDefault = 5;
        public int SettingInteger
        {
            get
            {
                return GetValueOrDefault<int>(SettingIntegerKeyName, SettingIntegerDefault);
            }
            set
            {
                if (AddOrUpdateValue(SettingIntegerKeyName, value))
                {
                    Save();
                }
            }
        }

        SettingEnum SettingEnumDefault = SettingEnum.Default;
        const string SettingEnumKeyName = "SettingEnum";
        public SettingEnum SettingEnum
        {
            get
            {
                var enumString = GetValueOrDefault<string>(SettingEnumKeyName, SettingEnumDefault.Name);
                return SettingEnum.AllSettings.FirstOrDefault(se => se.Name.Equals(enumString));
            }
            set
            {
                if (value != null && AddOrUpdateValue(SettingEnumKeyName, value.Name))
                {
                    Save();
                }
            }
        }
        public Collection<SettingEnum> AllSettingEnums
        {
            get
            {
                return SettingEnum.AllSettings;
            }
        }

        #region Boilerplate code
        private static UserSettings _instance = null;
        public static UserSettings Instance { get { return _instance; } }
        public UserSettings()
        {
            try
            {
                settings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                if (_instance == null)
                {
                    _instance = this;
                }
                else
                {
                    System.Diagnostics.Debug.Assert(false, "Created multiple UserSettings!");
                }
            }
            catch (Exception)
            {
                settings = null;
            }
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Values.ContainsKey(Key))
            {
                // If the value has changed
                if (settings.Values[Key] != value)
                {
                    // Store the new value
                    settings.Values[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Values.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Values.ContainsKey(Key))
            {
                value = (T)settings.Values[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            EventHandler settingsChanged = SettingsChanged;
            if (settingsChanged != null)
            {
                settingsChanged(this, new EventArgs());
            }
        }
        #endregion

    }
}
