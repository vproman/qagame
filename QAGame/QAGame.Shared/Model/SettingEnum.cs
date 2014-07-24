using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace QAGame.Model
{
    public class SettingEnum
    {
        /// <summary>
        /// The name of the enum value - must be unique, and can't change as this will be stored
        /// in the UserSettings
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// What to display to the user
        /// </summary>
        public string DisplayName { get; set; }
        public bool HasForward { get; set; }
        public bool HasBackward { get; set; }
        private static SettingEnum _forward = new SettingEnum() { Name = "forward", DisplayName = "Forward", HasBackward = false, HasForward = true };
        private static SettingEnum _backwards = new SettingEnum() { Name = "backwards", DisplayName = "Backward", HasBackward = true, HasForward = false };
        private static SettingEnum _both = new SettingEnum() { Name = "both", DisplayName = "Forward + Backward", HasBackward = true, HasForward = true };
        private static Collection<SettingEnum> _allSettings = null;
        public static SettingEnum Default
        {
            get
            {
                return _both;
            }
        }
        public static Collection<SettingEnum> AllSettings
        {
            get
            {
                if (_allSettings == null)
                {
                    _allSettings = new Collection<SettingEnum>();
                    _allSettings.Add(_forward);
                    _allSettings.Add(_backwards);
                    _allSettings.Add(_both);
                }
                return _allSettings;
            }
        }
        public override string ToString()
        {
            return DisplayName;
        }
        public override bool Equals(object obj)
        {
            SettingEnum other = obj as SettingEnum;
            if (other != null)
            {
                return Name == other.Name;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

    }
}
