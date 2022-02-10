﻿using System;
using System.Collections.Generic;
using System.Text;

namespace lab3
{
    class VersionsInterval
    {
        const string maxVersionString = "123.123.123";
        const string minVersionString = "0.0.0";
        public SemVersion leftVersion { get; set; }
        public SemVersion rightVersion { get; set; }
        private static string sign = "";
        private static string[] signs = new string[2] { "", "" };
        public VersionsInterval(string versionsInterval)
        {
            if (IsCorrectVersionWithSign(versionsInterval, out sign) == true)
            {
                string noSingsVersions = versionsInterval.Remove(0, sign.Length);
                SemVersion tempVersion = new SemVersion(noSingsVersions);
                switch (sign)
                {
                    case (">"):
                        leftVersion = tempVersion;
                        leftVersion.Patch += 1;
                        rightVersion = new SemVersion(maxVersionString);
                        break;
                    case (">="):
                        leftVersion = tempVersion;
                        rightVersion = new SemVersion(maxVersionString);
                        break;
                    case ("<"):
                        rightVersion = tempVersion;
                        if (rightVersion.Patch != 0)
                        {
                            rightVersion.Patch -= 1;
                        }
                        else
                        {
                            if (rightVersion.Major != 0)
                            {
                                rightVersion.Patch = int.MaxValue;
                                rightVersion.Major -= 1;
                            }
                            else
                            {
                                if (rightVersion.Minor != 0)
                                {
                                    rightVersion.Patch = int.MaxValue;
                                    rightVersion.Major = int.MaxValue;
                                    rightVersion.Minor -= 1;
                                }
                                else
                                {
                                    throw new ArgumentException("Некорректный формат объявления интервала");
                                }
                            }
                        }
                        leftVersion = new SemVersion(minVersionString);
                        break;
                    case ("<="):
                        rightVersion = tempVersion;
                        leftVersion = new SemVersion(minVersionString);
                        break;
                    case ("="):
                        leftVersion = tempVersion;
                        rightVersion = tempVersion;
                        break;
                }

            }
            else
            {
                if (IsCorrectVersionsInterval(versionsInterval) == true)
                {
                    string[] splitedVersionsInterval = versionsInterval.Split(' ');
                    string noSingsVersion_0 = splitedVersionsInterval[0].Remove(0, signs[0].Length);
                    SemVersion tempVersion_0 = new SemVersion(noSingsVersion_0);
                    switch (signs[0])
                    {
                        case (">"):
                            leftVersion = tempVersion_0;
                            leftVersion.Patch += 1;
                            break;
                        case (">="):
                            leftVersion = tempVersion_0;
                            break;
                            throw new ArgumentException("Некорректный формат объявления интервала");
                    }
                    string noSingsVersion_1 = splitedVersionsInterval[1].Remove(0, signs[1].Length);
                    SemVersion tempVersion_1 = new SemVersion(noSingsVersion_1);
                    switch (signs[1])
                    {
                        case ("<"):
                            rightVersion = tempVersion_1;
                            if (rightVersion.Patch != 0)
                            {
                                rightVersion.Patch -= 1;
                            }
                            else
                            {
                                if (rightVersion.Minor != 0)
                                {
                                    rightVersion.Patch = int.MaxValue;
                                    rightVersion.Minor -= 1;
                                }
                                else
                                {
                                    if (rightVersion.Major != 0)
                                    {
                                        rightVersion.Patch = int.MaxValue;
                                        rightVersion.Minor = int.MaxValue;
                                        rightVersion.Major -= 1;
                                    }
                                    else
                                    {
                                        throw new ArgumentException("Некорректный формат объявления интервала");
                                    }
                                }
                            }

                            break;
                        case ("<="):
                            rightVersion = tempVersion_1;
                            break;
                            throw new ArgumentException("Некорректный формат объявления интервала");
                    }
                    signs[0] = "";
                    signs[1] = "";
                }
                else
                {
                    throw new ArgumentException("Недопустимый знак при объявлении версии");
                }
            }
            sign = "";
        }
        public VersionsInterval(SemVersion leftVersion, SemVersion rightVersion)
        {
            if (leftVersion < rightVersion)
            {
                this.leftVersion = leftVersion;
                this.rightVersion = rightVersion;
            }
            if (leftVersion > rightVersion)
            {
                this.leftVersion = rightVersion;
                this.rightVersion = leftVersion;
            }
        }

        private static bool IsCorrectVersionWithSign(string versionsInterval, out string relatedSign)
        {
            relatedSign = "";
            foreach (char c in versionsInterval)
            {
                if (c == '>' || c == '<' || c == '=')
                {
                    relatedSign += c;
                }
                else
                {
                    break;
                }
            }
            if (relatedSign == ">" || relatedSign == ">=" || relatedSign == "<" || relatedSign == "<=" || relatedSign == "=")
            {
                if (versionsInterval.Split('.').Length == 3)
                {
                    //if (versionsInterval.Split('.')[2] == "0" && relatedSign == "<")
                    //{
                    //return false;
                    //}
                    return true;
                }
            }
            relatedSign = "";
            return false;
        }

        private static bool IsCorrectVersionsInterval(string versionsInterval)
        {
            string[] splitedVersionsInterval = versionsInterval.Split(' ');
            if (splitedVersionsInterval.Length == 2)
            {
                if (IsCorrectVersionWithSign(splitedVersionsInterval[0], out signs[0]) == true &&
                    IsCorrectVersionWithSign(splitedVersionsInterval[1], out signs[1]) == true)
                {
                    return true;
                }
            }
            return false;
        }
        public static VersionsInterval[] Intersection(VersionsInterval version1, VersionsInterval version2)
        {
            if (version1.leftVersion <= version2.leftVersion)
            {
                if (version1.rightVersion >= version2.leftVersion)
                {
                    if (version1.rightVersion >= version2.rightVersion)
                    {
                        return new VersionsInterval[] { version2 };
                    }
                    if (version1.rightVersion <= version2.rightVersion)
                    {
                        return new VersionsInterval[] { new VersionsInterval(version1.rightVersion, version2.leftVersion) };
                    }
                }
                if (version1.rightVersion <= version2.leftVersion)
                {
                    return null;
                }
            }
            if (version2.leftVersion <= version1.leftVersion)
            {
                if (version2.rightVersion >= version1.leftVersion)
                {
                    if (version2.rightVersion >= version1.rightVersion)
                    {
                        return new VersionsInterval[] { version1 };
                    }
                    if (version2.rightVersion <= version1.rightVersion)
                    {
                        return new VersionsInterval[] { new VersionsInterval(version1.leftVersion, version2.rightVersion) };
                    }
                }
                if (version2.rightVersion <= version1.leftVersion)
                {
                    return null;
                }
            }
            return new VersionsInterval[0];
        }
        public static VersionsInterval Union(VersionsInterval version1, VersionsInterval version2)
        {
            if (version1.leftVersion <= version2.leftVersion)
            {
                if (version1.rightVersion >= version2.leftVersion)
                {
                    if (version1.rightVersion >= version2.rightVersion)
                    {
                        return version1;
                    }
                    if (version1.rightVersion <= version2.rightVersion)
                    {
                        return new VersionsInterval(version1.leftVersion, version2.rightVersion);
                    }
                }
                return null;
            }
            if (version2.leftVersion <= version1.leftVersion)
            {
                if (version2.rightVersion >= version1.leftVersion)
                {
                    if (version2.rightVersion >= version1.rightVersion)
                    {
                        return version2;
                    }
                    if (version2.rightVersion <= version1.rightVersion)
                    {
                        return new VersionsInterval(version2.leftVersion, version1.rightVersion);
                    }
                }
                return null;
            }
            if (version1.leftVersion <= version2.leftVersion)
            {
                if (version1.rightVersion <= version2.leftVersion)
                {
                    Nonunion(version1, version2);
                }
            }
            return null;
        }

        public static string Nonunion(VersionsInterval version1, VersionsInterval version2)
        {
            if (version1.leftVersion <= version2.leftVersion)
            {
                if (version1.rightVersion <= version2.leftVersion)
                {
                    string result = $"{version1.leftVersion} {version1.rightVersion} || {version2.leftVersion} {version2.rightVersion}";
                    return result;
                }
            }
            return null;
        }

        public static bool IsEqual(VersionsInterval version1, VersionsInterval version2)
        {
            if (version1.leftVersion == version2.leftVersion && version1.rightVersion == version2.rightVersion)
            {
                return true;
            }
            return false;
        }
        public static bool operator ==(VersionsInterval version1, VersionsInterval version2)
        {
            return IsEqual(version1, version2);
        }
        public static bool operator !=(VersionsInterval version1, VersionsInterval version2)
        {
            return !IsEqual(version1, version2);
        }
        public override string ToString()
        {
            return $"from {this.leftVersion.ToString()} to {this.rightVersion.ToString()}";
        }

        public override bool Equals(object obj)
        {
            return obj is VersionsInterval interval &&
                   EqualityComparer<SemVersion>.Default.Equals(leftVersion, interval.leftVersion) &&
                   EqualityComparer<SemVersion>.Default.Equals(rightVersion, interval.rightVersion);
        }
    }
}
